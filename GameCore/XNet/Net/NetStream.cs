using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#pragma warning disable XS0001
namespace XNet.Libs.Net
{

    /// <summary>
    /// 消息
    /// </summary>
    public class MessageStream
    {
        private byte[] _buffer;
        private int _position;
        private int _length;
        private int _capacity;

        /// <summary>
        /// 初始化
        /// </summary>
        public MessageStream()
        {
            _buffer = new byte[0];
            _position = 0;
            _length = 0;
            _capacity = 0;
        }

        private byte ReadByte()
        {
            if (this._position >= this._length)
            {
                return 0;
            }
            return this._buffer[this._position++];
        }

        private int ReadInt()
        {
            int num = this._position += 4;
            if (num > this._length)
            {
                this._position = this._length;
                return -1;
            }
            return (((this._buffer[num - 4] | (this._buffer[num - 3] << 8)) | (this._buffer[num - 2] << 0x10)) | (this._buffer[num - 1] << 0x18));
        }

        private byte[] ReadBytes(int count)
        {
            int num = this._length - this._position;
            if (num > count)
            {
                num = count;
            }
            if (num <= 0)
            {
                return null;
            }
            byte[] buffer = new byte[num];
            if (num <= 8)
            {
                int num2 = num;
                while (--num2 >= 0)
                {
                    buffer[num2] = this._buffer[this._position + num2];
                }
            }
            else
            {
                Buffer.BlockCopy(this._buffer, this._position, buffer, 0, num);
            }
            this._position += num;
            return buffer;
        }
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Read(out Message message)
        {
            message = null;
            _position = 0;
            if (_length > 9)
            {
                message = new Message();
                message.Class = (MessageClass)ReadByte();
                message.Flag = ReadInt();
                message.Size = ReadInt();
                if (message.Size <= 0 || message.Size <= _length - _position)
                {
                    if (message.Size > 0)
                    {
                        message.Content = ReadBytes(message.Size);
                    }
                    Remove(message.Size + 9);
                    return true;
                }
                else
                {
                    message = null;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void EnsureCapacity(int value)
        {
            if (value <= this._capacity)
                return;
            int num1 = value;
            if (num1 < 0x100)
                num1 = 0x100;
            if (num1 < (this._capacity * 2))
                num1 = this._capacity * 2;
            byte[] buffer1 = new byte[num1];
            if (this._length > 0)
                Buffer.BlockCopy(this._buffer, 0, buffer1, 0, this._length);
            this._buffer = buffer1;
            this._capacity = num1;
        }

        /// <summary>
        /// 写Buffer
        /// </summary>
        /// <param name="buffer">缓冲</param>
        /// <param name="offset">跳跃值</param>
        /// <param name="count">总计</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (buffer.Length - offset < count)
            {
                count = buffer.Length - offset;
            }
            EnsureCapacity(_length + count);
            Array.Clear(_buffer, _length, _capacity - _length);
            Buffer.BlockCopy(buffer, offset, _buffer, _length, count);
            _length += count;
        }

        private void Remove(int count)
        {
            if (_length >= count)
            {
                Buffer.BlockCopy(_buffer, count, _buffer, 0, _length - count);
                _length -= count;
                Array.Clear(_buffer, _length, _capacity - _length);
            }
            else
            {
                _length = 0;
                Array.Clear(_buffer, 0, _capacity);
            }
        }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public class Message
    {
        private MessageClass _class;
        private int _flag;
        private int _size;
        private byte[] _content;
        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// 资料长度
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public MessageClass Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public Message() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        /// <param name="flag"></param>
        /// <param name="content"></param>
        public Message(MessageClass @class, int flag, byte[] content)
        {
            _class = @class;
            _flag = flag;
            _size = content.Length;
            _content = content;
        }

   
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            byte[] _byte;
            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem);
                writer.Write((byte)_class);
                writer.Write(_flag);
                writer.Write(_size);
                if (_size > 0)
                {
                    writer.Write(_content);
                }
                _byte = mem.ToArray();
                writer.Close();
            }
            return _byte;
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
                BinaryReader reader = new BinaryReader(mem);
                message._class = (MessageClass)reader.ReadByte();
                message._flag = reader.ReadInt32();
                message._size = reader.ReadInt32();
                if (message._size > 0)
                {
                    message._content = reader.ReadBytes(message._size);
                }
                reader.Close();
            }
            return message;
        }

    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageClass
    {
        Package = 0, 
        Close = 1,
        Normal = 2,
        Ping=3,//ping
        Request=4,//请求
        Response=5,//handler响应
        Notify=6,//广播消息
        Task =7 //任务消息
    }
}
