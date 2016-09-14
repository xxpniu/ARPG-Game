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
            long userID = (long)client.UserState;
            if (!UserDataManager.Current.TryToGetUserData(userID, out userData))
            {
                return new G2C_SaleItem { Code = ErrorCode.NoGamePlayerData };
            }

            var diff = new List<PlayerItem>();
            var result = userData.SaleItem(request.Guid, request.Num, diff);
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

