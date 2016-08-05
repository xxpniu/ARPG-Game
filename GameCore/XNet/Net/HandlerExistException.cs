using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.Net
{
    /// <summary>
    /// 消息处理者已经存在，不能重复注册
    /// </summary>
    public class ExistHandlerException : Exception
    {
        public MessageClass HandlerNo { set; get; }
        public ExistHandlerException(MessageClass handlerNo)
        {
            HandlerNo = handlerNo;
        }
        public override string ToString()
        {
            return string.Format("{0} is existed", HandlerNo);
        }
    }

}
