//#define USEJSON

using System;
using System.Collections.Generic;
using System.Reflection;
using XNet.Libs.Utility;
using System.Linq;
using Google.Protobuf;
using Proto.PServices;
using System.Threading.Tasks;
using System.Threading;

namespace XNet.Libs.Net
{

    /// <summary>
    /// 请求响应
    /// </summary>
    public class RequestHandle<T> : IClientMessageHandlerManager where T : Responser
    {

        private struct HandleMethodInfo
        {
            public bool NeedAdmission { set; get; }
            public MethodInfo Info { set; get; }

        }

        /// <summary>
        /// 注册一个类型
        /// </summary>
        public RequestHandle()
        {
            var type = typeof(T);
            var attrs = type.GetCustomAttributes<HandleAttribute>();
            if (attrs.Any())
            {
                var handlers = attrs.First()?.RType.GetMethods();

                foreach (var i in handlers)
                {
                    var apis = i.GetCustomAttributes<APIAttribute>();
                    if (!apis.Any()) continue;
                    var api = apis.First();
                    var dm = type.GetMethod(i.Name);
                    if (dm == null) { Debuger.LogError($"No found {i.Name}"); continue; }
                    var needAdmission = !dm.GetCustomAttributes<IgnoreAdmissionAttribute>().Any();
                    var info = new HandleMethodInfo { Info = dm, NeedAdmission = needAdmission };
                    Handlers.Add(api.ApiID, info);
                    Debuger.Log($"Handle {i.Name} [{info.NeedAdmission}]");
                }
            }
        }

        private Dictionary<int, HandleMethodInfo> Handlers { set; get; } = new Dictionary<int, HandleMethodInfo>();
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        /// <summary>
        /// Handle the specified netMessage and client.
        /// </summary>
        /// <param name="netMessage">Net message.</param>
        /// <param name="client">Client.</param>
        public void Handle(Message netMessage, Client client)
        {
            if (netMessage.Class == MessageClass.Request)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        DoHandle(netMessage, client);
                    }
                    catch (Exception ex)
                    {
                        Debuger.LogError(ex.ToString());
                    }
                 }, tokenSource.Token);
            }
        }

        private void DoHandle(Message message, Client client)
        {
            var handlerID = message.Flag;
            if (Handlers.TryGetValue(handlerID, out HandleMethodInfo m))
            {
                var begin = DateTime.Now;
                if (m.NeedAdmission && !client.HaveAdmission)
                {
                    client.Server.DisConnectClient(client); return;
                }

                var rType = m.Info.GetParameters().First().ParameterType;
                var request = Activator.CreateInstance(rType) as IMessage;
                if (message.Content == null)
                {
                    Debuger.LogError($"empty request ->{request}");
                }
                else
                {
                    request.MergeFrom(message.Content);
                }
                var responser = Activator.CreateInstance(typeof(T), client) as T;
                IMessage result = null;
                try
                {
                    result = m.Info.Invoke(responser, new object[] { request }) as IMessage;
                }
                catch (Exception ex)
                {
                    Debuger.LogError(ex.ToString());
                }

                if (result == null)
                {
                    result = Activator.CreateInstance(m.Info.ReturnType) as IMessage;
                }

                if (NetProtoTool.EnableLog)
                {
                    var processTime = DateTime.Now - begin;
                    Debuger.Log($"({processTime.TotalMilliseconds}ms)[{client.Socket.RemoteEndPoint}]{request.GetType()}{request}->{JsonFormatter.Default.Format(result)}");
                }

                var response = new Message(MessageClass.Response,
                    message.Flag, message.ExtendFlag,
                    result.ToByteArray());

                client.SendMessage(response);
            }
            else
            {
                Debuger.LogError(string.Format("TypeID:{0} no Handle!", handlerID));
            }
        }
    }
}

