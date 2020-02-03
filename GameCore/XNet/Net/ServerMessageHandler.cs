using System;
namespace XNet.Libs.Net
{
    /// <summary>
    /// 消息处理抽象类
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public interface IServerMessageHandler
    {
        void Handle(Message message,SocketClient client);
    }
}

