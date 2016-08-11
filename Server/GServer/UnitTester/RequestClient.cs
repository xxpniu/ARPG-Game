using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using Proto;
using XNet.Libs.Net;
using XNet.Libs.Utility;

namespace UnitTester
{
   public class RequestClient : SocketClient
    {
        public interface IHandler
        {
            void OnHandle(Proto.ISerializerable message);
            void OnTimeOut();
        }

        public class Request<S, R> : IHandler where S : class, Proto.ISerializerable, new() where R : class, Proto.ISerializerable, new()
        {
            public void SendRequest()
            {
                if (Client.AttachRequest(this, Index))
                    this.Client.SendRequest(this.RequestMessage, this.Index);
            }

            public void OnHandle(ISerializerable message)
            {
                if (OnCompleted != null)
                {
                    OnCompleted(message as R);
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

            public Action<R> OnCompleted;

        }

        public class ResponseHandler : ServerMessageHandler
        {
            public Dictionary<int, IHandler> _handlers = new Dictionary<int, IHandler>();
            public override void Handle(Message message)
            {
                int requestIndex = 0;
                Type responseType = MessageHandleTypes.GetTypeByIndex(message.Flag);
                ISerializerable response;
                using (var mem = new MemoryStream(message.Content))
                {
                    using (var br = new BinaryReader(mem))
                    {
                        requestIndex = br.ReadInt32();
#if DEBUG
                        var json = Encoding.UTF8.GetString(br.ReadBytes(message.Size - 4));
                        response = JsonTool.Deserialize(responseType, json) as Proto.ISerializerable;
                        Debuger.Log(json);
#else
                        response=Activator.CreateInstance(responseType) as Proto.ISerializerable;
                        response.ParseFormBinary(br);
#endif
                    }
                }

                IHandler handler;
                if (_handlers.TryGetValue(requestIndex, out handler))
                {
                    handler.OnHandle(response);
                    _handlers.Remove(requestIndex);
                }
            }
        }

        public RequestClient(string host, int port) : base(port, host)
        {
            Handler = new ResponseHandler();
            this.RegisterHandler(MessageClass.Response, Handler);
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
#else
                        request.ToBinary(bw);
#endif
                    }
                    var result = new Message(MessageClass.Request, index, mem.ToArray());
                    SendMessage(result);
                }
            }
        }

        private bool AttachRequest(IHandler hander, int requestIndex)
        {
            if (Handler._handlers.ContainsKey(requestIndex)) return false;
            Handler._handlers.Add(requestIndex, hander);
            return true;
        }
    }
}

