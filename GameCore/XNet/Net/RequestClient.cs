using System;
namespace XNet.Libs
{
    #region Task Handles
    public class ServerTaskAttribute : Attribute
    {
        public ServerTaskAttribute(Type t)
        {
            TaskType = t;
        }

        public Type TaskType { set; get; }
    }

    public abstract class TaskHandler<T> where T : class, Proto.ISerializerable, new()
    {
        public abstract void DoTask(T task);
    }

    #endregion

    #region Request clinet
    public class RequestClient : SocketClient
    {
        #region Ihandler
        public interface IHandler
        {
            void OnHandle(bool isSuccess, ISerializerable message);
            void OnTimeOut();
        }
        #endregion

        #region  Request 
        public class Request<S, R> : IHandler where S : class, Proto.ISerializerable, new() where R : class, Proto.ISerializerable, new()
        {
            private volatile bool RequestCompleted;
            private bool IsSuccess = false;
            public void SendRequest()
            {
                isSync = false;
                RequestCompleted = false;
                DoSend();
                while (!RequestCompleted)
                {

                }
                if (OnCompleted != null)
                {
                    OnCompleted(IsSuccess, Result as R);
                }
            }

            public void SendRequestSync()
            {
                RequestCompleted = false;
                isSync = true;
                DoSend();
            }


            private void DoSend()
            {
                if (Client.AttachRequest(this, Index))
                {
                    this.Client.SendRequest(this.RequestMessage, this.Index);
                }
            }

            private ISerializerable Result;

            private bool isSync = true;

            public void OnHandle(bool isSuccess, ISerializerable message)
            {
                RequestCompleted = true;
                {
                    IsSuccess = isSuccess;
                    Result = message;
                    if (isSync)
                    {
                        if (OnCompleted != null)
                            OnCompleted(isSuccess, Result as R);
                    }
                }
            }

            public void OnTimeOut()
            {
                //throw new NotImplementedException();
            }

            public Request(RequestClient client, int index)
            {
                Client = client;
                RequestMessage = new S();
            }

            public int Index { private set; get; }

            public S RequestMessage { private set; get; }

            public RequestClient Client { private set; get; }

            public Action<bool, R> OnCompleted;

        }
        #endregion

        #region ResponserHandler 
        private class ResponseHandler : ServerMessageHandler
        {
            public SyncDictionary<int, IHandler> _handlers = new SyncDictionary<int, IHandler>();

            public SyncDictionary<int, Type> _taskHandler = new SyncDictionary<int, Type>();

            public override void Handle(Message message)
            {
                int requestIndex = 0;
                Type responseType = MessageHandleTypes.GetTypeByIndex(message.Flag);
                ISerializerable response;
                using (var mem = new MemoryStream(message.Content))
                {
                    using (var br = new BinaryReader(mem))
                    {
                        if (message.Class == MessageClass.Response)
                        {
                            requestIndex = br.ReadInt32();
                        }
                        response = Activator.CreateInstance(responseType) as Proto.ISerializerable;
                        response.ParseFormBinary(br);
                        if (NetProtoTool.EnableLog)
                            Debuger.Log(response.GetType() + "-->" + JsonTool.Serialize(response));
                    }
                }

                if (message.Class == MessageClass.Response)
                {
                    IHandler handler;
                    if (_handlers.TryToGetValue(requestIndex, out handler))
                    {
                        handler.OnHandle(true, response);
                        _handlers.Remove(requestIndex);
                    }
                }
                else if (message.Class == MessageClass.Task)
                {
                    Type handlerType;
                    if (_taskHandler.TryToGetValue(message.Flag, out handlerType))
                    {
                        var handler = Activator.CreateInstance(handlerType);
                        var m = handlerType.GetMethod("DoTask");
                        m.Invoke(handler, new object[] { response });
                    }
                    else {
                        Debuger.LogError("NoHanlderType:" + message.Flag);
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

        public void RegTaskHandlerFromAssembly(Assembly assembly)
        {
            foreach (var i in assembly.GetTypes())
            {
                var att = i.GetCustomAttribute<ServerTaskAttribute>();
                if (att == null) continue;
                var index = 0;
                if (Proto.MessageHandleTypes.GetTypeIndex(att.TaskType, out index))
                    Handler._taskHandler.Add(index, i);
            }
        }

        private ResponseHandler Handler;

        private volatile int lastIndex = 0;

        #region Request Create 
        public Request<S, R> CreateRequest<S, R>()
            where S : class, ISerializerable, new()
            where R : class, ISerializerable, new()
        {
            var req = new Request<S, R>(this, lastIndex++);
            return req;
        }

        public Request<S, R> R<S, R>()
            where S : class, ISerializerable, new()
            where R : class, ISerializerable, new()
        {
            return CreateRequest<S, R>();
        }
        #endregion

        private void SendRequest(ISerializerable request, int requestIndex)
        {
            if (this.IsConnect)
            {
                var index = 0;
                if (MessageHandleTypes.GetTypeIndex(request.GetType(), out index))
                {
                    using (var mem = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(mem))
                        {
                            bw.Write(requestIndex);
                            request.ToBinary(bw);
                            if (NetProtoTool.EnableLog)
                                Debuger.Log(request.GetType() + "-->" + JsonTool.Serialize(request));
                        }
                        var result = new Message(MessageClass.Request, index, mem.ToArray());
                        SendMessage(result);
                    }
                }
            }
            else {
                IHandler handler;
                if (Handler._handlers.TryToGetValue(requestIndex, out handler))
                {
                    handler.OnHandle(false, null);
                }
            }
        }

        private bool AttachRequest(IHandler hander, int requestIndex)
        {
            return Handler._handlers.Add(requestIndex, hander);
        }
    }
    #endregion
}
