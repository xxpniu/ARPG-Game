using System;
using Proto;
using Proto.LoginServerTaskServices;
using ServerUtility;
using XNet.Libs.Net;

namespace RPCResponsers
{
    public  class LoginServerTaskServices: XSingleton<LoginServerTaskServices>, ILoginServerTaskServices
    {
        public Task_L2B_ExitUser ExitUser(Task_L2B_ExitUser req)
        {
            return req;
        }
    }
}
