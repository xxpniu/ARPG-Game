using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Proto;

namespace PNet
{

    /// <summary>
    /// 登陆
    /// </summary>    
    [NetMessage]
    public class LoginMessage : NetMessage<C2S_Login, S2C_Login> { }

    /// <summary>
    /// 注册
    /// </summary>    
    [NetMessage]
    public class RegUserMessage : NetMessage<C2S_RegUser, S2C_RegUser> { }

}