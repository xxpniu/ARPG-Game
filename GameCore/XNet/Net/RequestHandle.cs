//#define USEJSON

using System;
using System.Collections.Generic;
using System.Reflection;
using XNet.Libs.Utility;
using System.Linq;
using Google.Protobuf;
using Proto.PServices;
using System.Threading.Tasks;

namespace XNet.Libs.Net
{

    /// <summary>
    /// 请求响应
    /// </summary>
    public class RequestHandle<T> : IClientMessageHandlerManager where T : Responser
    {

        public struct HandleMethodInfo
        {
            public bool NeedAdmission { set; get; }
            public MethodInfo Info { set; get; }

        }

        /// <summary>
        /// 注册一个类型
        /// </summary>
        public  RequestHandle() 
        {
            var type = typeof(T);
            var attrs =  type.GetCustomAttributes<HandleAttribute>();
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
                    var info = new HandleMethodInfo { Info = dm, NeedAdmission = needAdmission};
                    Handlers.Add(api.ApiID, info );
                    Debuger.Log($"Rpc {i.Name} needadmission {info.NeedAdmission}");
                }
            }
        }
        /// <summary>
        /// 当前注册的handler
        /// </summary>
        private Dictionary<int, HandleMethodInfo> Handlers { set; get; } = new Dictionary<int, HandleMethodInfo>();

        /// <summary>
        /// Handle the specified netMessage and client.
        /// </summary>
        /// <param name="netMessage">Net message.</param>
        /// <param name="client">Client.</param>
        public  void Handle(Message netMessage, Client client)
        {
            if (netMessage.Class == MessageClass.Request)
            {
                Task.Factory.StartNew(() =>
                  {
                      try
                      {
                          Debuger.DebugLog($"Handle messgae{netMessage.Flag} of {netMessage.ExtendFlag}");
                          DoHandle(netMessage, client);
                      }
                      catch (Exception ex)
                      {
                          Debuger.LogError(ex.ToString());
                      }
                  });
            }
        }

        private  void DoHandle(Message message, Client client)
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
                request.MergeFrom(message.Content);
                var responser = Activator.CreateInstance(typeof(T), client) as T;

                Debuger.DebugLog($"[{message.ExtendFlag}]{request}");
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
                    Debuger.Log($"{request.GetType()}({processTime.TotalMilliseconds}ms)-->{JsonFormatter.Default.Format(result)}");
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

