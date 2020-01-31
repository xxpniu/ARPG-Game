using System;
using XNet.Libs.Net;
using System.IO;
using XNet.Libs.Utility;
using System.Reflection;
using Google.Protobuf;
using Proto;
using Proto.PServices;
using System.Collections.Generic;

namespace ServerUtility
{
    #region Request clinet



    public class RequestClient<T> : SocketClient,IChannel
        where T: TaskHandler, new()
    {
        private struct ApiRequest
        {
            public IApiBase Request { set; get; }

            public Type ResponseType { set; get; }
            public int Index { get; internal set; }
        }
        #region ResponserHandler 
        private class ResponseHandler: ServerMessageHandler
        {
            private SyncDictionary<int, ApiRequest> ApiRequests {  set; get; } = new SyncDictionary<int, ApiRequest>();

            private T TaskHandler { set; get; }

            public bool AttachRquest(ApiRequest apiRequest)
            {
                if (ApiRequests.HaveKey(apiRequest.Index)) return false;
                return ApiRequests.Add(apiRequest.Index, apiRequest);
            }

            private Dictionary<int, MethodInfo> TaskInvokes { set; get; }

            public ResponseHandler()
            {
                TaskHandler = new T();
                TaskInvokes = new Dictionary<int, MethodInfo>();
                var ms = typeof(T).GetMethods();//need to checkß
                foreach (var i in ms)
                {
                    var api = i.GetCustomAttribute<APIAttribute>();
                    if (api == null) continue;
                    TaskInvokes.Add(api.ApiID, i);
                }
            }

            public override void Handle(Message message)
            {
                if (message.Class == MessageClass.Response)
                {
                    int requestIndex = message.ExtendFlag;
                    if (ApiRequests.TryToGetValue(requestIndex, out ApiRequest req))
                    {
                        IMessage response;
                        response = Activator.CreateInstance(req.ResponseType) as IMessage;
                        response.MergeFrom(message.Content);
                        req.Request.FinishResponse(response);
                        ApiRequests.Remove(requestIndex);
                    }
                    else
                    {
                        Debuger.LogError($"No found API {message.Flag} by requestIndex {message.ExtendFlag}");
                    }
                }
                else if (message.Class == MessageClass.Task)
                {
                    if (TaskInvokes.TryGetValue(message.Flag, out MethodInfo m))
                    {
                        var task = Activator.CreateInstance(m.ReturnType) as IMessage;
                        task.MergeFrom(message.Content);
                        m.Invoke(TaskHandler, new object[] { task });
                    }
                    else
                    {
                        Debuger.LogError($"No found task {message.Flag}");
                    }
                }
            }
        }
        #endregion

        public RequestClient(string host, int port) : base(port, host)
        {
            Handler = new ResponseHandler();

            this.RegisterHandler(MessageClass.Response, Handler);
            this.RegisterHandler(MessageClass.Task, Handler);
        }


        private ResponseHandler Handler { set; get; }

        private volatile int lastIndex = 0;

        int IChannel.ProcessRequest<Request, Response>(APIBase<Request, Response> api)
        {
            lastIndex++;
            var requestIndex = lastIndex;
            if (this.IsConnect)
            {
                if (Handler.AttachRquest(new ApiRequest { Index= requestIndex, Request = api, ResponseType = typeof(Response) }))
                {
                    var result = new Message(MessageClass.Request,
                        api.API,
                        requestIndex,
                        api.QueryRequest.ToByteArray());
                    SendMessage(result);
                }
            }
            else
            {
                api.SetResponse(default);   
            }
            return requestIndex;
        }
    }
    #endregion
}

