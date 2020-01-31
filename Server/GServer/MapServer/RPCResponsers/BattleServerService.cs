using System;
using MapServer;
using MapServer.Managers;
using Proto;
using Proto.BattleServerService;
using Proto.LoginBattleGameServerService;
using ServerUtility;
using XNet.Libs.Net;

namespace RPCResponsers
{
    public class BattleServerService:Responser, IBattleServerService
    {
        public BattleServerService(Client c):base(c)
        {
        }

        public B2C_ExitBattle ExitBattle(C2B_ExitBattle req)
        {
            var account_uuid = (string)Client.UserState;
            MonitorPool.Singleton.GetMointor<MapSimulaterManager>().KickUser(account_uuid);
            return new B2C_ExitBattle { Code = ErrorCode.Ok };
        }

        public B2C_ExitGame ExitGame(C2B_ExitGame req)
        {
            throw new NotImplementedException();
        }

        public B2C_JoinBattle JoinBattle(C2B_JoinBattle request)
        {

            var result = ErrorCode.Error;
            var req = CheckSession.CreateQuery()
                .SendRequestAsync( Appliaction.Current.Client,new B2L_CheckSession { UserID = request.AccountUuid, SessionKey = request.Session })
                .GetAwaiter().GetResult();

            if (result == ErrorCode.Ok)
            {
                Client.UserState = request.AccountUuid;

                if (!MonitorPool.S.Get<MapSimulaterManager>().BindUser(request.AccountUuid, Client.ID))
                {
                    result = ErrorCode.NofoundUserOnBattleServer;
                }
            }
            return new B2C_JoinBattle { Code = result };
        }
    }
}
