using System;
using XNet.Libs.Net;
using System.Collections.Generic;
using Proto;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using XNet.Libs.Utility;
using System.Reflection;

namespace ServerUtility
{
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

    public class RequestClient : SocketClient
    {
        public interface IHandler
        {
            void OnHandle(bool isSuccess, ISerializerable message);
            void OnTimeOut();
        }

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
                        int offset = 0;
                        if (message.Class == MessageClass.Response)
                        {
                            requestIndex = br.ReadInt32();
                            offset = 4;
                        }
#if DEBUG
                        var json = Encoding.UTF8.GetString(br.ReadBytes(message.Size - offset));
                        response = JsonTool.Deserialize(responseType, json) as Proto.ISerializerable;
                        Debuger.Log(response.GetType() + "-->" + json);
#else
                        response=Activator.CreateInstance(responseType) as Proto.ISerializerable;
                        response.ParseFormBinary(br);
#endif
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

        public RequestClient(string host, int port) : base(port, host)
        {
            Handler = new ResponseHandler();

            this.RegisterHandler(MessageClass.Response, Handler);
            this.RegisterHandler(MessageClass.Task, Handler);
        }

        public void RegAssembly(Assembly assembly)
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


        public Request<S, R> CreateRequest<S, R>() where S : class, Proto.ISerializerable, new() where R : class, Proto.ISerializerable, new()
        {
            var req = new Request<S, R>(this, lastIndex++);
            return req;
        }

        private void SendRequest(Proto.ISerializerable request, int requestIndex)
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
#if DEBUG
                            var json = JsonTool.Serialize(request);
                            var bytes = Encoding.UTF8.GetBytes(json);
                            bw.Write(bytes);
                            Debuger.Log(request.GetType() + "-->" + json);
#else
                            request.ToBinary(bw);
#endif
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
}

