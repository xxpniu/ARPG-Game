using System;
using XNet.Libs.Utility;
using System.Reflection;
using Google.Protobuf;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Proto.PServices;

namespace XNet.Libs.Net
{
    #region Request clinet

    public class RequestClient<T> : SocketClient, IChannel
        where T : TaskHandler, new()
    {
        #region ResponserHandler 
        private struct ApiRequest
        {
            public IApiBase Request { set; get; }
            public Type ResponseType { set; get; }
            public int Index { get; internal set; }
            public DateTime Start { set; get; }
        }
        private class ResponseHandler : IServerMessageHandler
        {
            private ConcurrentDictionary<int, ApiRequest> ApiRequests { set; get; } = new ConcurrentDictionary<int, ApiRequest>();

            private T TaskHandler { set; get; }

            public bool AttachRquest(ApiRequest apiRequest)
            {
                if (ApiRequests.ContainsKey(apiRequest.Index)) return false;
                return ApiRequests.TryAdd(apiRequest.Index, apiRequest);
            }

            private Dictionary<int, MethodInfo> TaskInvokes { set; get; }

            public ResponseHandler()
            {
                TaskHandler = new T();
                TaskInvokes = new Dictionary<int, MethodInfo>();
                var att = typeof(T).GetCustomAttribute<TaskHandlerAttribute>();
                if (att == null) return;
                var ms = att.RType.GetMethods();//need to checkß

                foreach (var i in ms)
                {
                    var api = i.GetBaseDefinition().GetCustomAttribute<APIAttribute>();
                    if (api == null) continue;
                    TaskInvokes.Add(api.ApiID, i);
                }
            }

            public void Handle(Message message, SocketClient client)
            {
                if (message.Class == MessageClass.Response)
                {
                    int requestIndex = message.ExtendFlag;
                    if (ApiRequests.TryRemove(requestIndex, out ApiRequest req))
                    {
                        IMessage response;
                        response = Activator.CreateInstance(req.ResponseType) as IMessage;
                        response.MergeFrom(message.Content);
                        req.Request.FinishResponse(response);
                        Debuger.DebugLog($"[{requestIndex}]cost[{ (DateTime.Now - req.Start) }] Response -> {response}  ");
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

        private ResponseHandler Handler { set; get; }
        private volatile int lastIndex = 0;

        public RequestClient(string host, int port, bool useThread = true)
            : base(port, host, useThread)
        {
            Handler = new ResponseHandler();
            this.RegisterHandler(MessageClass.Response, Handler);
            this.RegisterHandler(MessageClass.Task, Handler);
        }

        int IChannel.ProcessRequest<Request, Response>(APIBase<Request, Response> api)
        {
            lastIndex++;
            var requestIndex = lastIndex;
            if (this.IsConnect)
            {
                if (Handler.AttachRquest(new ApiRequest
                {
                    Index = requestIndex,
                    Request = api,
                    ResponseType = typeof(Response),
                    Start = DateTime.Now
                }))
                {
                    Debuger.DebugLog($"[{requestIndex}] Send {api.QueryRequest.GetType()}-->{api.QueryRequest}");

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

