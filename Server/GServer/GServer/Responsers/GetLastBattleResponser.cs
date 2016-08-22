using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_GetLastBattle))]
    public class GetLastBattleResponser:Responser<C2G_GetLastBattle,G2C_GetLastBattle>
    {
        public GetLastBattleResponser()
        {
            NeedAccess = true;
        }

        public override G2C_GetLastBattle DoResponse(C2G_GetLastBattle request, Client client)
        {
            var response = new G2C_GetLastBattle { Code = ErrorCode.Error };
            var req = Appliaction.Current.Client.CreateRequest<G2L_GetLastBattle, L2G_GetLastBattle>();
            req.RequestMessage.UserID = request.UserID;
            req.OnCompleted = (s, r) =>
            {
                response.Code = r.Code;
                if (r.Code == ErrorCode.OK)
                {
                    response.BattleServer = r.BattleServer;
                    response.MapID = r.MapID;
                }
            };
            req.SendRequest();
            return response;

        }
    }
}

