using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using System.Linq;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_CreateHero))]
    public class CreateHeroResponser:Responser<C2G_CreateHero,G2C_CreateHero>
    {
        public CreateHeroResponser()
        {
            NeedAccess = true;
        }

        public override G2C_CreateHero DoResponse(C2G_CreateHero request, Client client)
        {
            var userID = (long)client.UserState;
            using (var db = new DataBaseContext.GameDb(Appliaction.Current.Connection))
            {
                var query = db.TBGAmePlayer.Where(t => t.UserID == userID);
                if (query.Count() > 0)
                {
                    return new G2C_CreateHero { Code = ErrorCode.Error };
                }

                var gamePlayer = new DataBaseContext.TBGAmePlayer 
                {
                    Coin = 100, 
                    UserID = userID, 
                    Gold = 100,
                    PackageSize = 50, 
                    UserPackage = string.Empty
                };

                db.TBGAmePlayer.InsertOnSubmit(gamePlayer);

                var hero= new DataBaseContext.TBPLayerHero
                {
                    UserID = userID,
                    HeroID = request.HeroID,
                    Exp = 0,
                    Level = 1,
                    Magics =string.Empty,
                    Equips = string.Empty
                };
                db.TBPLayerHero.InsertOnSubmit(hero);

                var equip = new DataBaseContext.TBPLayerEquip 
                { 
                    UserID = userID,
                    UserEquipValues= string.Empty
                };
                db.TBPLayerEquip.InsertOnSubmit(equip);

                db.SubmitChanges();
                return new G2C_CreateHero { Code = ErrorCode.OK };
            }
        }
    }
}

