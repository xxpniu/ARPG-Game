using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using System.Linq;

namespace GServer.Responsers
{
    [HandleType(typeof(B2G_GetPlayerInfo))]
    public class B2G_GetPlayerInfoResponser : Responser<B2G_GetPlayerInfo, G2B_GetPlayerInfo>
    {
        public B2G_GetPlayerInfoResponser()
        {
            NeedAccess = false;
        }

        public override G2B_GetPlayerInfo DoResponse(B2G_GetPlayerInfo request, Client client)
        {

            using (var db = new DataBaseContext.GameDb(Appliaction.Current.Connection))
            {
                var user = db.TBGAmePlayer.Where(t => t.UserID == request.UserID).SingleOrDefault();
                if (user == null)
                {
                    return new G2B_GetPlayerInfo { Code = ErrorCode.NoGamePlayerData };
                }
                var equip = db.TBPLayerEquip.Where(t => t.UserID == request.UserID).SingleOrDefault();                                   
                var hero = db.TBPLayerHero.Where(t => t.UserID == request.UserID).FirstOrDefault();
                if (hero == null)
                {
                    return new G2B_GetPlayerInfo { Code = ErrorCode.NoHeroInfo };
                }



                return new G2B_GetPlayerInfo
                {
                    Code = ErrorCode.OK,
                    Package = Managers.DataManager.GetPackageFromTbPlayer(user, equip),
                    Hero = Managers.DataManager.GetDHeroFromTBhero(hero)
                };
            }
        }
    }
}

