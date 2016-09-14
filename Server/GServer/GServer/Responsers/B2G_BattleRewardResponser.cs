using System;
using System.Collections.Generic;
using GServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.Responser
{
    [HandleType(typeof(B2G_BattleReward),HandleResponserType.SERVER_SERVER)]
    public class B2G_BattleRewardResponser:Responser<B2G_BattleReward,G2B_BattleReward>
    {
        public B2G_BattleRewardResponser()
        {
            NeedAccess = false;
        }

        public override G2B_BattleReward DoResponse(B2G_BattleReward request, Client client)
        {
            //check ?

            Managers.UserData data;
            if (! MonitorPool.S.Get<UserDataManager>().TryToGetUserData(request.UserID, out data))
            {
                return new G2B_BattleReward { Code = ErrorCode.NoGamePlayerData };
            }

            var diff = new Dictionary<int,Proto.PlayerItem>();
            foreach (var i in request.DropItems)
            {
                if (diff.ContainsKey(i.ItemID))
                {
                    diff[i.ItemID].Num += i.Num;
                }
                else 
                {
                    diff.Add(i.ItemID, i);
                }
            }

            foreach (var i in request.ConsumeItems)
            {
                if (diff.ContainsKey(i.ItemID))
                {
                    diff[i.ItemID].Num -= i.Num;
                }
                else
                {
                    diff.Add(i.ItemID, i);
                }
            }

            ErrorCode code = ErrorCode.OK;

            foreach (var i in diff)
            {
                if (i.Value.Num == 0) continue;
                if (i.Value.Num > 0)
                {
                    if (!data.AddItem(i.Value))
                    {
                        code = ErrorCode.Error;
                    }
                }
                else 
                {
                    if (!data.ConsumeItem(i.Value))
                    {
                        code = ErrorCode.Error;
                    }
                }
            }

            data.AddGold(request.Gold);

            var userClient = Appliaction.Current.GetClientByUserID(request.UserID);
            if (userClient != null)
            {
                var syncHero = new Task_G2C_SyncHero { Hero = data.GetHero() };
                var syncPackage = new Task_G2C_SyncPackage
                {
                    Package = data.GetPackage(),
                    Gold = data.Gold,
                    Coin = data.Coin
                };
                NetProtoTool.SendTask(userClient, syncHero);
                NetProtoTool.SendTask(userClient, syncPackage);
            }

            return new G2B_BattleReward { Code = code};
        }
    }
}

