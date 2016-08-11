using System;
using Proto;
using XNet.Libs.Net;
using System.Linq;
using org.vxwo.csharp.json;
using System.Collections.Generic;
using ServerUtility;

namespace GServer
{
    [HandleType(typeof(C2G_Login))]
    public class LoginResponser:Responser<Proto.C2G_Login,Proto.G2C_Login>
    {
        public LoginResponser()
        {
            NeedAccess = false;
        }

        public override G2C_Login DoResponse(C2G_Login request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new G2C_Login { Code = ErrorCode.VersionError };
            }


            if (string.IsNullOrWhiteSpace(request.Session))
                return new G2C_Login { Code = ErrorCode.Error };

            var req = Appliaction.Current.Client.CreateRequest<G2L_CheckUserSession, L2G_CheckUserSession>();
            req.RequestMessage.Session = request.Session;
            req.RequestMessage.UserID = request.UserID;
            ErrorCode resultCode = ErrorCode.Error;
            req.OnCompleted = (s,res) =>
            {
                if (s&&res.Code == ErrorCode.OK)
                {
                    resultCode = ErrorCode.OK;
                    client.HaveAdmission = true;
                    client.UserState = request.UserID;
                }
            };
            req.SendRequest();

            if (client.HaveAdmission)
            {
                using (var db = new DataBaseContext.GameDb(Appliaction.Current.Connection))
                {
                    var player = db.TBGAmePlayer.Where(t => t.UserID == request.UserID).SingleOrDefault();
                    if (player == null)
                    {
                        return new G2C_Login { Code = ErrorCode.NoGamePlayerData };
                    }

                    var heros = db.TBPLayerHero.Where(t => t.UserID == request.UserID).ToList();
                    if (heros == null &&
                        heros.Count == 0)
                    {
                        return new G2C_Login { Code = ErrorCode.NoHeroInfo };
                    }

                    var package = new PlayerPackage { MaxSize = player.PackageSize };
                    //玩家道具列表
                    //var packageItems = new List<PlayerItem>();
                    if (!string.IsNullOrEmpty(player.UserPackage))
                    {
                        package.Items = JsonTool.Deserialize<List<PlayerItem>>(player.UserPackage);
                    }

                    var equip = db.TBPLayerEquip.Where(t => t.UserID == request.UserID)
                                   .SingleOrDefault();
                    if (equip != null)
                    {
                        if (!string.IsNullOrEmpty(equip.UserEquipValues))
                        {
                            package.Equips = JsonTool.Deserialize<List<Equip>>(equip.UserEquipValues);
                        }
                    }


                    var resHeros = new List<DHero>();
                    foreach (var i in heros)
                    {
                        var hero = new DHero
                        {
                            HeroID = i.HeroID,
                            Exprices = i.Exp,
                            Level = i.Level
                        };

                        if (!string.IsNullOrEmpty(i.Equips))
                        {
                            hero.Equips = JsonTool.Deserialize<List<WearEquip>>(i.Equips);
                        }
                        if (!string.IsNullOrEmpty(i.Magics))
                        {
                            hero.Magics = JsonTool.Deserialize<List<HeroMagic>>(i.Magics);
                        }

                        resHeros.Add(hero);
                    }

                    return new G2C_Login
                    {
                        Code = ErrorCode.OK,
                        Package = package,
                        Heros = resHeros
                    };
                }
            }
            else {

                return new G2C_Login { Code = resultCode };
            }
        }
    }
}

