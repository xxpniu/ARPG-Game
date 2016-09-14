using System;
using ServerUtility;
using Proto;
using XNet.Libs.Net;
using MapServer.Managers;

namespace MapServer.Responsers
{
    [HandleType(typeof(C2B_JoinBattle),HandleResponserType.CLIENT_SERVER)]
    public class C2B_JoinBattleResponser:Responser<C2B_JoinBattle,B2C_JoinBattle>
    {
        public C2B_JoinBattleResponser()
        {
            NeedAccess = false;
        }

        public override B2C_JoinBattle DoResponse(C2B_JoinBattle request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new B2C_JoinBattle { Code = ErrorCode.VersionError };
            }

            var result = ErrorCode.Error;
            var req = Appliaction.Current.Client.CreateRequest<B2L_CheckSession, L2B_CheckSession>();
            req.RequestMessage.SessionKey = request.Session;
            req.RequestMessage.UserID = request.UserID;
            req.OnCompleted = (s, r) => { result = r.Code; };
            req.SendRequest();

            if (result == ErrorCode.OK)
            {
                client.UserState = request.UserID;

                if (!MapSimulaterManager.Singleton.BindUser(request.UserID, client.ID))
                {
                    result = ErrorCode.NOFoundUserOnBattleServer;
                }
            }
            return new B2C_JoinBattle { Code = result };
        }
    }
}

