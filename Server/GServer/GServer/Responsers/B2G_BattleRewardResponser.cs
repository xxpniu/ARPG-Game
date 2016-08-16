using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.Responser
{
    [HandleType(typeof(B2G_BattleReward))]
    public class B2G_BattleRewardResponser:Responser<B2G_BattleReward,G2B_BattleReward>
    {
        public B2G_BattleRewardResponser()
        {
            NeedAccess = false;
        }

        public override G2B_BattleReward DoResponse(B2G_BattleReward request, Client client)
        {
            return new G2B_BattleReward { Code = ErrorCode.OK };
        }
    }
}

