using System;
using System.Collections.Generic;
using GServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_SaleItem),HandleResponserType.CLIENT_SERVER)]
    public class C2G_SaleItemResponser:Responser<C2G_SaleItem,G2C_SaleItem>
    {
        public C2G_SaleItemResponser()
        {
            NeedAccess = true;
        }

        public override G2C_SaleItem DoResponse(C2G_SaleItem request, Client client)
        {
            UserData userData;
            var userID = (long)client.UserState;
            if (!MonitorPool.S.Get<UserDataManager>().TryToGetUserData(userID, out userData))
            {
                return new G2C_SaleItem { Code = ErrorCode.NoGamePlayerData };
            }

            ErrorCode result = ErrorCode.OK;
            userData.RecordPackage();
            //diff
            var diff = new List<PlayerItem>();
            foreach (var i in request.Items)
            {
                result= userData.SaleItem(i.Guid, i.Num, diff);
                if (result != ErrorCode.OK) break;
            }

            if (result != ErrorCode.OK)
            {
                userData.RevertPackage();
            }
            else 
            {
                userData.ClearRecord();
            }

            return new G2C_SaleItem
            {
                Code = result,
                Coin = userData.Coin,
                Gold = userData.Gold,
                Diff = diff 
            };
        }
    }
}

