using System;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using Proto;
using XNet.Libs.Net;

namespace ServerUtility
{
    public sealed class NetProtoTool
    {
        public NetProtoTool()
        {
        }

        public static bool EnableLog = false;

        public static Message ToNetMessage(MessageClass @class,  ISerializerable m)
        {
            int flag;
            MessageHandleTypes.GetTypeIndex(m.GetType(), out flag);
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    m.ToBinary(bw);
                }
                return new Message(@class, flag, mem.ToArray());
            }
        }

        public static ISerializerable GetProtoMessage(Message message)
        {
            var type = MessageHandleTypes.GetTypeByIndex(message.Flag);
            var protoMsg = Activator.CreateInstance(type) as ISerializerable;
            using (var mem = new MemoryStream(message.Content))
            {
                using (var br = new BinaryReader(mem))
                {
                    protoMsg.ParseFormBinary(br);
                }
            }
            return protoMsg;
        }
    }
}

