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


    public class RequestHandle : MessageHandlerManager
    {
        static RequestHandle()
        {
            
        }

        public static void RegAssembly(Assembly assembly)
        { 
            var types = assembly.GetTypes();
            foreach (var i in types)
            {
                var attrs = i.GetCustomAttributes<HandleTypeAttribute>();
                if (attrs.Count() > 0)
                {
                    var index = 0;
                    if (MessageHandleTypes.GetTypeIndex(attrs.First().HandleType, out index))
                    {
                        _handler.Add(index, i);
                    }
                }

            }
        }

        private static Dictionary<int, Type> _handler = new Dictionary<int, Type>();

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

        private static void DoHandle(Message message, Client client)
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
                        var content = br.ReadBytes(message.Size - 4);
#if DEBUG
                        var json = Encoding.UTF8.GetString(content);
                        request = JsonTool.Deserialize(type, json) as Proto.ISerializerable;
                        Debuger.Log("Request:"+request.GetType()+"->"+json);
#else
                        request = Activator.CreateInstance(type) as Proto.ISerializerable;
                        request.ParseFormBinary(br);
                        //Debuger.Log(JsonTool.Serialize(request));
#endif
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

                var response = m.GetMethod("DoResponse")
                                .Invoke(responser, new object[] { request, client})
                                as ISerializerable;
                if (response == null) return;

                var index = 0;
                if (MessageHandleTypes.GetTypeIndex(response.GetType(), out index))
                {
                    using (var mem = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(mem))
                        {
                            bw.Write(requestIndex);
#if DEBUG
                            var json = JsonTool.Serialize(response);
                            var bytes = Encoding.UTF8.GetBytes(json);
                            bw.Write(bytes);
                            Debuger.Log("Response:"+response.GetType()+"->"+json);
#else
                            response.ToBinary(bw);
#endif
                        }
                        var result = new Message(MessageClass.Response,index, mem.ToArray());
                        client.SendMessage(result);
                    }
                }
            }
            else
            {
                Debuger.Log(string.Format("TypeID:{0} no Handle!", handlerID));
            }
        }
    }
}

