using System;
using MapServer.Managers;
using Proto;
using ServerUtility;

namespace MapServer.TaskHandlers
{
    [ServerTask(typeof(Task_L2B_ExitUser))]
    public class Task_ExitUser:TaskHandler<Task_L2B_ExitUser>
    {
        public Task_ExitUser()
        {
        }

        public override void DoTask(Task_L2B_ExitUser task)
        {
            MapSimulaterManager.Singleton.KickUser(task.UserID);
        }
    }
}

