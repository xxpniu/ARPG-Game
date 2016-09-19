//#define USEJSON

using System;
using System.Collections.Generic;
using XNet.Libs.Net;
using System.Reflection;
using Proto;
using System.IO;
using org.vxwo.csharp.json;
using System.Text;
using XNet.Libs.Utility;
using System.Linq;

namespace ServerUtility
{
    //处理请求类型
    public enum HandleResponserType
    { 
        /// <summary>
        /// 服务器之间的请求
        /// </summary>
        SERVER_SERVER,
        /// <summary>
        /// 客户端和服务器之间的请求
        /// </summary>
        CLIENT_SERVER
    }

    /// <summary>
    /// 请求响应
    /// </summary>
    public class RequestHandle : MessageHandlerManager
    {

        /// <summary>
        /// 注册一个程序集
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="rTy">R ty.</param>
        public void RegAssembly(Assembly assembly,HandleResponserType rTy)
        {
            var types = assembly.GetTypes();
            foreach (var i in types)
            {
                var attrs = i.GetCustomAttributes<HandleTypeAttribute>();
                if (attrs.Count() > 0)
                {
                    var attr = attrs.First();
                    if (attr.RType != rTy) continue;
                    var index = 0;
                    if (MessageHandleTypes.GetTypeIndex(attrs.First().HandleType, out index))
                    {
                        _handler.Add(index, i);
                    }
                }

            }
        }

        /// <summary>
        /// 注册一个类型
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void RegType<T>() where T :class, new()
        {
            var attrs = typeof(T).GetCustomAttributes<HandleTypeAttribute>();
            if (attrs.Count() > 0)
            {
                var index = 0;
                if (MessageHandleTypes.GetTypeIndex(attrs.First().HandleType, out index))
                {
                    _handler.Add(index, typeof(T));
                }
            }
        }

        /// <summary>
        /// 当前注册的handler
        /// </summary>
        private Dictionary<int, Type> _handler = new Dictionary<int, Type>();

        /// <summary>
        /// Handle the specified netMessage and client.
        /// </summary>
        /// <param name="netMessage">Net message.</param>
        /// <param name="client">Client.</param>
        public override void Handle(Message netMessage, Client client)
        {
            if (netMessage.Class == MessageClass.Request)
            {
                try
                {
                    DoHandle(netMessage, client);
                }
                catch (Exception ex)
                {
                    Debuger.LogError(ex.ToString());
                }
            }
        }

        private void DoHandle(Message message, Client client)
        {
            
            var handlerID = message.Flag;
            Type m;
            if (_handler.TryGetValue(handlerID, out m))
            {
                var type = MessageHandleTypes.GetTypeByIndex(handlerID);
                if (type == null)
                {
                    Debuger.Log(string.Format("TypeId:{0} no found!", handlerID));
                    return;
                }
                ISerializerable request;
                int requestIndex = 0;
                using (var mem = new MemoryStream(message.Content))
                {
                    using (var br = new BinaryReader(mem))
                    {
                        requestIndex = br.ReadInt32();
                        request = Activator.CreateInstance(type) as Proto.ISerializerable;
                        request.ParseFormBinary(br);
                        if (NetProtoTool.EnableLog)
                            Debuger.Log(request.GetType() + "-->" + JsonTool.Serialize(request));
                    }
                }
                var responser = Activator.CreateInstance(m);
                var NeedAccess = (bool)m.GetProperty("NeedAccess").GetValue(responser);

                if (NeedAccess)
                {
                    if (!client.HaveAdmission)
                    {
                        //直接断开
                        client.Server.DisConnectClient(client, 1);
                        return;
                    }
                }

                var begin = DateTime.Now;
                ISerializerable result =null;
                try
                {
                    result = m.GetMethod("DoResponse")
                                    .Invoke(responser, new object[] { request, client })
                                    as ISerializerable;
                }
                catch(Exception ex) 
                {
                    Debuger.LogError(ex.ToString());
                }

                if (result == null) 
                {
                    var rem = m.GetMethod("DoResponse");
                    var emptyResult = Activator.CreateInstance(rem.ReturnType) as ISerializerable;
                    result = emptyResult;
                    //Debuger.LogWaring("Empty")
                }

                if (NetProtoTool.EnableLog)
                {
                    var processTime = DateTime.Now - begin;

                    Debuger.Log(
                        string.Format("{1}({0}ms)-->{2}", 
                                      processTime.TotalMilliseconds, 
                                      request.GetType(),
                                      JsonTool.Serialize(result))
                    );
                }

                var index = 0;
                if (MessageHandleTypes.GetTypeIndex(result.GetType(), out index))
                {
                    using (var mem = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(mem))
                        {
                            bw.Write(requestIndex);
                            result.ToBinary(bw);
                        }
                        var response = new Message(MessageClass.Response, index, mem.ToArray());
                        client.SendMessage(response);
                    }
                }
            }
            else
            {
                Debuger.LogError(string.Format("TypeID:{0} no Handle!", handlerID));
            }
        }
    }
}

