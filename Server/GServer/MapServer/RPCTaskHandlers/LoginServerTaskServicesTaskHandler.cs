using System;
using MapServer;
using MapServer.Managers;
using Proto;
using Proto.LoginServerTaskServices;
using ServerUtility;

namespace RPCTaskHandlers
{
    public class LoginServerTaskServicesTaskHandler : TaskHandler, ILoginServerTaskServices
    {
        public Task_L2B_ExitUser ExitUser(Task_L2B_ExitUser task)
        {
            MonitorPool.Singleton.GetMointor<MapSimulaterManager>().KickUser(task.UserID);
            return task;
        }

        public Task_L2B_StartBattle StartBattle(Task_L2B_StartBattle task)
        {
            foreach (var i in task.Users)
            {
                MonitorPool.Singleton.GetMointor<MapSimulaterManager>()
                           .AddUser(i, task.MapID);
                Appliaction.Current.TryConnectUserServer(i);
            }
            return task;
        }
    }
}
