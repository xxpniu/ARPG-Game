using System;
using Proto;
using ServerUtility;
using MapServer.Managers;

namespace MapServer
{
    [ServerTask(typeof(Task_L2B_StartBattle))]
    public class Task_StartBattle:TaskHandler<Task_L2B_StartBattle>
    {
        public Task_StartBattle()
        {
            
        }

        public override void DoTask(Task_L2B_StartBattle task)
        {
            //G2L_EndBattle
            foreach (var i in task.Users)
            {
                MapSimulaterManager.Singleton.AddUser(i, task.MapID);
                Appliaction.Current.TryConnectUserServer(i);
            }
        }
    }
}

