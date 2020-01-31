using System;
using EConfig;
using ExcelConfig;
using Proto;
using Proto.MongoDB;

namespace GateServer
{
    public static class ProtoExtends
    {
        public static DHero ToDhero(this GameHeroEntity entity, ItemPackageEntity package)
        {
            var h = new DHero
            {
                Exprices = entity.Exp,
                HeroID = entity.HeroId,
                Level = entity.Level
            };

            foreach (var i in entity.Magics)
            {
                h.Magics.Add(new Proto.HeroMagic { MagicKey = i.Key });
            }



            foreach (var i in entity.Equips)
            {
                if (package.Items.TryGetValue(i.Value, out ItemNum item))
                {
                    h.Equips.Add(new WearEquip
                    {
                        Part = (EquipmentType)i.Key,
                        GUID = i.Value,
                        ItemID = item.Id
                    });
                }
            }

            return h;
        }

        public static PlayerPackage ToPackage(this ItemPackageEntity entity)
        {
            var p = new PlayerPackage { MaxSize = entity.PackageSize };
            foreach (var i in entity.Items)
            {
                p.Items.Add(new PlayerItem
                {
                    GUID = i.Key,
                    ItemID = i.Value.Id,
                    Locked = i.Value.IsLock,
                    Num = i.Value.Num,
                    Level = i.Value.Level
                });
            }

            return p;
        }
    }
}
