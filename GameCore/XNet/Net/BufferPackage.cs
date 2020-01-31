using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#pragma warning disable XS0001
namespace XNet.Libs.Net
{


    /// <summary>
    /// 消息封包
    /// </summary>
    public class MessageBufferPackage
    {

        public void AddMessage(Message message)
        {
            MessageQueue.Enqueue(message);
        }

        private Queue<Message> MessageQueue = new Queue<Message>();

        public MessageBufferPackage(Message message)
            : base()
        {
            AddMessage(message);
        }

        public MessageBufferPackage()
        { }

        public List<Message> Messages { get { return MessageQueue.ToList(); } }

        public Message ToMessage()
        {
            int count = MessageQueue.Count;
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(count);
                    foreach (var i in MessageQueue)
                    {
                        bw.Write(i.ToBytes());
                    }
                }
                return new Message(MessageClass.Package, 0, 0, mem.ToArray());
            }
        }

        public static MessageBufferPackage ParseFromMessage(Message message)
        {
            if (message.Class != (byte)MessageClass.Package)
            {
                throw new Exception("Not A Package Message!");
            }

            var buffer = new MessageBufferPackage();
            using (var mem = new MemoryStream(message.Content))
            {
                using (var br = new BinaryReader(mem))
                {
                    var count = br.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        byte type = br.ReadByte();
                        int flag = br.ReadInt32();
                        int exFlag = br.ReadInt32();
                        int size = br.ReadInt32();
                        byte[] content = br.ReadBytes(size);
                        buffer.AddMessage(new Message((MessageClass)type, flag,exFlag, content));
                    }
                }
            }

            return buffer;
        }
    }
}
