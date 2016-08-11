using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.TaskHandlers
{
    [ServerTask(typeof(Task_L2B_JoinBattle))]
    public class Task_JoinBattle : TaskHandler<Task_L2B_JoinBattle>
    {
        public Task_JoinBattle()
        {
        }

        public override void DoTask(Task_L2B_JoinBattle task)
        {
            var client = Appliaction.Current.GetClientByUserID(task.UserID);
            if (client == null) return;
            var message = new Task_G2C_JoinBattle { Server = task.BattleServer };
            //发送给客户端
            client.SendMessage(NetProtoTool.ToNetMessage(MessageClass.Task, message));
        }
    }
}

