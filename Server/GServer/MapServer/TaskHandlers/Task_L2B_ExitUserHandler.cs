using System;
using MapServer.Managers;
using Proto;
using ServerUtility;

namespace MapServer.TaskHandlers
{
    [ServerTask(typeof(Task_L2B_ExitUser))]
    public class Task_L2B_ExitUserHandler :TaskHandler<Task_L2B_ExitUser>
    {
        public Task_L2B_ExitUserHandler()
        {
            
        }

        public override void DoTask(Task_L2B_ExitUser task)
        {
            MonitorPool.Singleton.GetMointor<MapSimulaterManager>().KickUser(task.UserID);
        }
    }
}

