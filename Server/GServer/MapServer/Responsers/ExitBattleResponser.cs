using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace MapServer.Responsers
{
    [HandleType(typeof(C2B_ExitBattle))]
    public class ExitBattleResponser : Responser<C2B_ExitBattle, B2C_ExitBattle>
    {
        public ExitBattleResponser()
        {
            NeedAccess = true;
        }

        public override B2C_ExitBattle DoResponse(C2B_ExitBattle request, Client client)
        {
            var userID = (long)client.UserState;
            Managers.MapSimulaterManager.Singleton.KickUser(userID);
            return new B2C_ExitBattle { Code = ErrorCode.OK };
        }
    }
}

