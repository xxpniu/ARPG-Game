using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XNet.Libs.Net
{

    /// <summary>
    /// 消息处理管理抽象
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public interface IClientMessageHandlerManager
    {
        void Handle(Message netMessage, Client client);
    }

    /// <summary>
    /// 普通的消息处理管理
    /// @author:xxp
    /// @date:2020/02/03
    /// </summary>
    public class DefaultMessageHandlerManager : IClientMessageHandlerManager
    {
        private readonly Dictionary<MessageClass, Type> Handlers = new Dictionary<MessageClass, Type>();
        /// <summary>
        /// 注册一个消息处理者
        /// </summary>
        /// <param name="listenMessageNo"></param>
        /// <param name="handlerClass"></param>
        public void RegsiterHandler(MessageClass listenMessageNo, Type handlerClass)
        {
            if (Handlers.ContainsKey(listenMessageNo))
            {
                throw new ExistHandlerException(listenMessageNo);
            }
            Handlers.Add(listenMessageNo, handlerClass);
        }

        public void Handle(Message netMessage, Client client)
        {
            var no = netMessage.Class;
            if (Handlers.ContainsKey(no))
            {
                var handler = Activator.CreateInstance(Handlers[no]);
                var method = Handlers[no].GetMethod("Handle");
                method.Invoke(handler, new object[]{ netMessage, client});
            }
            else
            {
                Utility.Debuger.LogWaring(string.Format("No handle Message NO:{0}", no));
            }
        }
    }

}
