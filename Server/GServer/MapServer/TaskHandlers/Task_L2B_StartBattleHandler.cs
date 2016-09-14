using System;
using Proto;
using ServerUtility;
using MapServer.Managers;

namespace MapServer
{
    [ServerTask(typeof(Task_L2B_StartBattle))]
    public class Task_L2B_StartBattleHandler :TaskHandler<Task_L2B_StartBattle>
    {
        public Task_L2B_StartBattleHandler()
        {
            
        }

        public override void DoTask(Task_L2B_StartBattle task)
        {
            //G2L_EndBattle
            foreach (var i in task.Users)
            {
                MonitorPool.Singleton.GetMointor<MapSimulaterManager>()
                           .AddUser(i, task.MapID);
                Appliaction.Current.TryConnectUserServer(i);
            }
        }
    }
}

