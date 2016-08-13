using System;
using System.Collections.Generic;
using org.vxwo.csharp.json;
using Proto;

namespace GServer.Managers
{
    public class DataManager : ServerUtility.XSingleton<DataManager>
    {
        public DataManager()
        {
        }

        public static DHero GetDHeroFromTBhero(DataBaseContext.TBPLayerHero i)
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


        public static PlayerPackage GetPackageFromTbPlayer(DataBaseContext.TBGAmePlayer player, DataBaseContext.TBPLayerEquip equip)
        {


            var package = new PlayerPackage { MaxSize = player.PackageSize };
            //玩家道具列表
            //var packageItems = new List<PlayerItem>();
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

