//#define USEJSON

using System;
using System.Collections.Generic;
using XNet.Libs.Net;
using System.Reflection;
using System.IO;
using XNet.Libs.Utility;
using System.Linq;
using Proto;
using Google.Protobuf;
using Proto.PServices;
using System.Threading.Tasks;

namespace ServerUtility
{ 

    /// <summary>
    /// 请求响应
    /// </summary>
    public class RequestHandle<T> : MessageHandlerManager where T : Responser
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
        public override void Handle(Message netMessage, Client client)
        {
            if (netMessage.Class == MessageClass.Request)
            {
                _ = Task.Run(() =>
                  {
                      try
                      {
                          DoHandle(netMessage, client);
                      }
                      catch (Exception ex)
                      {
                          Debuger.LogError(ex.ToString());
                      }
                  });
            }
        }

        private void DoHandle(Message message, Client client)
        {
            var handlerID = message.Flag;
            if (Handlers.TryGetValue(handlerID, out HandleMethodInfo m))
            {
                if (m.NeedAdmission && !client.HaveAdmission)
                {
                    client.Server.DisConnectClient(client, 1);
                    return;
                }
                int requestIndex = 0;
                var arrs = m.Info.GetGenericArguments();
                var request = Activator.CreateInstance(arrs.First()) as IMessage;
                request.MergeFrom(message.Content);
                var responser = Activator.CreateInstance(typeof(T), client) as T;

                var begin = DateTime.Now;
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
                    var emptyResult = Activator.CreateInstance(m.Info.ReturnType) as IMessage;
                    result = emptyResult;
                }

                if (NetProtoTool.EnableLog)
                {
                    var processTime = DateTime.Now - begin;
                    Debuger.Log($"{request.GetType()}({processTime.TotalMilliseconds}ms)-->{JsonFormatter.Default.Format(result)}");
                }
                var response = new Message(MessageClass.Response, message.Flag, requestIndex, result.ToByteArray());
                client.SendMessage(response);
            }
            else
            {
                Debuger.LogError(string.Format("TypeID:{0} no Handle!", handlerID));
            }
        }
    }
}

