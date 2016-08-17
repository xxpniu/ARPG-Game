using System;
namespace XNet.Libs.Net
{
    /// <summary>
    /// 消息处理抽象类
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public abstract class ServerMessageHandler
    {
        /// <summary>
        /// 处理一个消息
        /// </summary>
        /// <param name="message"></param>
        public abstract void Handle(Message message);
        /// <summary>
        /// 更行
        /// </summary>
        public virtual void Update()
        {
        }
        /// <summary>
        /// 当前的连接
        /// </summary>
        public SocketClient Connection { set; get; }

    }



    /// <summary>
    /// 连接成功参数
    /// </summary>
    public class ConnectCommpletedArgs : EventArgs
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        public bool Success { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PingCompletedArgs : EventArgs
    {
        /// <summary>
        /// ticks
        /// </summary>
        public long DelayTicks { set; get; }
        /// <summary>
        /// Millisecond
        /// </summary>
        public double DelayMillisecond
        {
            get
            {
                return DelayTicks / TimeSpan.TicksPerMillisecond;
            }
        }
    }
}

