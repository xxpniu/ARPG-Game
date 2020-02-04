using System;
using Proto;
using Proto.LoginServerTaskServices;
using XNet.Libs.Net;

[TaskHandler(typeof(ILoginServerTaskServices))]
public class LoginServerTaskServiceHandler : TaskHandler, ILoginServerTaskServices
{
    public Task_L2B_ExitUser ExitUser(Task_L2B_ExitUser req)
    {
        BattleSimulater.S.KickUser(req.UserID);
        return req;
    }
}
