using System;
using GServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_OperatorEquip),HandleResponserType.CLIENT_SERVER)]
    public class C2G_OperatorEquipResponser:Responser<C2G_OperatorEquip,G2C_OperatorEquip>
    {
        public C2G_OperatorEquipResponser()
        {
            NeedAccess = true;
        }

        public override G2C_OperatorEquip DoResponse(C2G_OperatorEquip request, Client client)
        {
            var userID = (long)client.UserState;
            UserData userData;
            if (!UserDataManager.Current.TryToGetUserData(userID, out userData))
            {
                return new G2C_OperatorEquip { Code = ErrorCode.NoGamePlayerData };
            }
            bool result = false;
            if (request.IsWear)
            {
                result = userData.WearEquip(request.Guid, request.Part);
            }
            else
            {
                result = userData.UnWearEquip(request.Part);
            }

            return new G2C_OperatorEquip
            {
                Code = !result ? ErrorCode.Error : ErrorCode.OK,
                Hero = userData.GetHero()                          
            };

        }
    }
}

