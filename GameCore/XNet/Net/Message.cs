using System.IO;

namespace XNet.Libs.Net
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageClass
    {
        Package = 0,//打包消息
        Close = 1, //关闭消息
        Normal = 2,//一般消息
        Ping = 3,//ping
        Request = 4,//请求
        Response = 5,//handler响应
        Notify = 6,//广播消息
        Action = 7,
        Task = 8 //任务消息
    }

    /// <summary>
    /// 消息
    /// </summary>
    public class Message
    {
        /// <summary>
        /// content bytes
        /// </summary>
        public byte[] Content { get; set; }
        /// <summary>
        /// size
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// flag 
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// type of messag
        /// </summary>
        public MessageClass Class { get; set; }
        /// <summary>
        /// extend flag
        /// </summary>
        /// <value>The extend flag.</value>
        public int ExtendFlag { set; get; }

        public Message() { }

        public Message(MessageClass @class, int flag, int exFlag, byte[] content):this()
        {
            Class = @class;
            Flag = flag;
            Size = content.Length;
            Content = content;
            ExtendFlag = exFlag;
        }

        /// <summary>
        /// to bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(mem))
                {
                    writer.Write((byte)Class);
                    writer.Write(Flag);
                    writer.Write(ExtendFlag);
                    writer.Write(Size);
                    if (Size > 0) writer.Write(Content);
                    return mem.ToArray();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public static Message FromBytes(byte[] Buffer)
        {
            Message message = new Message();
            using (MemoryStream mem = new MemoryStream(Buffer))
            {
                using (BinaryReader reader = new BinaryReader(mem))
                {
                    message.Class = (MessageClass)reader.ReadByte();
                    message.Flag = reader.ReadInt32();
                    message.ExtendFlag = reader.ReadInt32();
                    message.Size = reader.ReadInt32();
                    if (message.Size > 0)
                    {
                        message.Content = reader.ReadBytes(message.Size);
                    }
                }
            }
            return message;
        }

    }

   
}
