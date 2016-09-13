using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using LoginServer.Managers;

namespace LoginServer
{
    [HandleType(typeof(B2L_EndBattle))]
    public class B2L_EndBattleResponser : Responser<B2L_EndBattle, L2B_EndBattle>
    {
        public B2L_EndBattleResponser()
        {
            NeedAccess = true;
        }

        public override L2B_EndBattle DoResponse(B2L_EndBattle request, Client client)
        {
            var serverID = (int)client.UserState;
            BattleManager.Singleton.ExitBattle(request.UserID,serverID);
            return new L2B_EndBattle { Code = ErrorCode.OK };
        }
    }
}

