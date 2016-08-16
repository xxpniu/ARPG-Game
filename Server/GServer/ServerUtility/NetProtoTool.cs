using System;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using XNet.Libs.Net;

namespace ServerUtility
{
    public sealed class NetProtoTool
    {
        public NetProtoTool()
        {
        }

        public static bool EnableLog = false;

        public static Message ToNetMessage(MessageClass @class,  Proto.ISerializerable m)
        {
            int flag;
            Proto.MessageHandleTypes.GetTypeIndex(m.GetType(), out flag);
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    #if DEBUG
                    var json = JsonTool.Serialize(m);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    bw.Write(bytes);
                    #else
                    m.ToBinary(bw);
                    #endif
                }

                return new Message(@class, flag, mem.ToArray());
            }
        }
    }
}

