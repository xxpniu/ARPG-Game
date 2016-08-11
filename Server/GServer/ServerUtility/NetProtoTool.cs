using System;
using System.IO;
using XNet.Libs.Net;

namespace ServerUtility
{
    public sealed class NetProtoTool
    {
        public NetProtoTool()
        {
        }

        public static Message ToNetMessage(MessageClass @class,  Proto.ISerializerable m)
        {
            int flag;
            Proto.MessageHandleTypes.GetTypeIndex(m.GetType(), out flag);
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    m.ToBinary(bw);
                }

                return new Message(@class, flag, mem.ToArray());
            }
        }
    }
}

