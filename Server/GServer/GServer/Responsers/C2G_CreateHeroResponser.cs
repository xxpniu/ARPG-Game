using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using System.Linq;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_CreateHero))]
    public class C2G_CreateHeroResponser:Responser<C2G_CreateHero,G2C_CreateHero>
    {
        public C2G_CreateHeroResponser()
        {
            NeedAccess = true;
        }

        public override G2C_CreateHero DoResponse(C2G_CreateHero request, Client client)
        {
            var userID = (long)client.UserState;

            if (Managers.UserDataManager.Current.TryToCreateUser(userID, request.HeroID))
            {
                return new G2C_CreateHero { Code = ErrorCode.OK };
            }
            return new G2C_CreateHero { Code = ErrorCode.Error };
        }
    }
}

