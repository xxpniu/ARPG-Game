using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using LoginServer.Managers;

namespace LoginServer.Responsers
{
    [HandleType(typeof(G2L_BeginBattle),HandleResponserType.SERVER_SERVER)]
    public class G2L_BeginBattleResponser:Responser<G2L_BeginBattle,L2G_BeginBattle>
    {
        public G2L_BeginBattleResponser()
        {
            NeedAccess = true;
        }

        public override L2G_BeginBattle DoResponse(G2L_BeginBattle request, Client client)
        {
            GameServerInfo battleServer;
            //已经在战场中
            var res = BattleManager.Singleton.BeginBattle(
                request.UserID, request.MapID, (int)client.UserState, out battleServer);
            if (res == ErrorCode.OK)
            {
                return new L2G_BeginBattle { Code =  ErrorCode.OK, BattleServer = battleServer };
            }
            else {
                return new L2G_BeginBattle { Code = res };
            }

        }
    }
}

