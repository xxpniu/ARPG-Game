using System;
using MapServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace MapServer.Responsers
{
    [HandleType(typeof(C2B_ExitBattle),HandleResponserType.CLIENT_SERVER)]
    public class C2B_ExitBattleResponser : Responser<C2B_ExitBattle, B2C_ExitBattle>
    {
        public C2B_ExitBattleResponser()
        {
            NeedAccess = true;
        }

        public override B2C_ExitBattle DoResponse(C2B_ExitBattle request, Client client)
        {
            var userID = (long)client.UserState;
            MonitorPool.Singleton.GetMointor<MapSimulaterManager>().KickUser(userID);
            return new B2C_ExitBattle { Code = ErrorCode.OK };
        }
    }
}

