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

        /// <summary>
        /// add a message
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            MessageQueue.Enqueue(message);
        }

        private readonly Queue<Message> MessageQueue = new Queue<Message>();

        public MessageBufferPackage(Message message)
            : base()
        {
            AddMessage(message);
        }

        public MessageBufferPackage()
        { }

        /// <summary>
        /// all message
        /// </summary>
        public List<Message> Messages { get { return MessageQueue.ToList(); } }

        /// <summary>
        /// up package messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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
                        buffer.AddMessage(new Message((MessageClass)type, flag, exFlag, content));
                    }
                }
            }
            return buffer;
        }

        public Message ToPackage()
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(MessageQueue.Count);
                    while (MessageQueue.Count > 0)
                    {
                        var m = MessageQueue.Dequeue();
                        bw.Write(m.ToBytes()); 
                    }
                }
                return new Message(MessageClass.Package, 0, 0, mem.ToArray());
            }
        }

    }
}