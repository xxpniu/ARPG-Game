using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using LoginServer.Managers;

namespace LoginServer
{
    [HandleType(typeof(B2L_EndBattle))]
    public class BattleServerEndBattle : Responser<B2L_EndBattle, L2B_EndBattle>
    {
        public BattleServerEndBattle()
        {
            NeedAccess = true;
        }

        public override L2B_EndBattle DoResponse(B2L_EndBattle request, Client client)
        {
            BattleManager.Singleton.ExitBattle(request.UserID);
            return new L2B_EndBattle { Code = ErrorCode.OK };
        }
    }
}

