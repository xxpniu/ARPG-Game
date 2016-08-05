using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.Net
{


    /// <summary>
    /// 消息处理队列
    /// </summary>
    public class MessageQueue<T>
    {
        private Queue<T> WriteQueue { set; get; }
        private Queue<T> ReadQueue { set; get; }
        private object syncroot = new object();
        /// <summary>
        /// 获取队列中消息
        /// </summary>
        /// <returns></returns>
        public Queue<T> GetMessage()
        {
            lock (syncroot)
            {
                var temp = ReadQueue;
                ReadQueue = WriteQueue;
                WriteQueue = temp;
            }
            return ReadQueue;
        }
        /// <summary>
        /// 添加一个待发送消息
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(T message)
        {
            lock (syncroot)
            {
                WriteQueue.Enqueue(message);
            }
        }


        public MessageQueue()
        {
            WriteQueue = new Queue<T>();
            ReadQueue = new Queue<T>();
        }
    }
}
