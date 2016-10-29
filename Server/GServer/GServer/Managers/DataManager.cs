using System;
using System.Collections.Generic;
using DataBaseContext;
using org.vxwo.csharp.json;
using Proto;
using ServerUtility;

namespace GServer.Managers
{
    public class DataManager : XSingleton<DataManager>
    {
        public DataManager()
        {
        }

        public static DHero GetDHeroFromTBhero(TBPLayerHero i)
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

            return hero;
        }

        public static PlayerPackage GetPackageFromTbPlayer(TBGAmePlayer player, TBPLayerEquip equip)
        {


            var package = new PlayerPackage { MaxSize = player.PackageSize };
            //玩家道具列表

            if (!string.IsNullOrEmpty(player.UserPackage))
            {
                package.Items = JsonTool.Deserialize<List<PlayerItem>>(player.UserPackage);
            }


            if (equip != null)
            {
                if (!string.IsNullOrEmpty(equip.UserEquipValues))
                {
                    package.Equips = JsonTool.Deserialize<List<Equip>>(equip.UserEquipValues);
                }
            }
            return package;
        }
    }
}

