using System;
using LoginServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer
{
    [HandleType(typeof(G2L_GetLastBattle))]
    public class G2L_GetLastBattleResponser : Responser<G2L_GetLastBattle, L2G_GetLastBattle>
    {
        public G2L_GetLastBattleResponser()
        {
            NeedAccess = true;
        }

        public override L2G_GetLastBattle DoResponse(G2L_GetLastBattle request, Client client)
        {
            UserServerInfo userSInfo;
            if (BattleManager.Singleton.GetBattleServerByUserID(request.UserID, out userSInfo))
            {
                var battleServer = ServerManager.Singleton.GetBattleServerMappingByServerID(userSInfo.BattleServerID);

                if (battleServer == null)
                {
                    return new L2G_GetLastBattle { Code = ErrorCode.BattleServerHasDisconnect };
                }
                else {
                    return new L2G_GetLastBattle
                    {
                        BattleServer = battleServer.ServerInfo,
                        MapID = userSInfo.MapID,
                        Code = ErrorCode.OK
                    };
                }
            }
            else {
                return new L2G_GetLastBattle { Code = ErrorCode.NOFoundUserBattleServer };
            }
        }
    }
}

