using System;
using ServerUtility;
using Proto;
using XNet.Libs.Net;
using ExcelConfig;

namespace GServer.Managers
{
    [HandleType(typeof(C2G_EquipmentLevelUp),HandleResponserType.CLIENT_SERVER)]
    public class C2G_EquipmentLevelUpResponser:Responser<C2G_EquipmentLevelUp,G2C_EquipmentLevelUp>
    {
        public C2G_EquipmentLevelUpResponser()
        {
            NeedAccess = true;
        }

        public override G2C_EquipmentLevelUp DoResponse(C2G_EquipmentLevelUp request, Client client)
        {
            var userID = (long)client.UserState;
            UserData userData;
            if (!MonitorPool.S.Get<UserDataManager>().TryToGetUserData(userID, out userData))
            {
                return new G2C_EquipmentLevelUp { Code = ErrorCode.NoGamePlayerData };
            }

            var item = userData.GetItemByGuid(request.Guid);
            if (item == null) return new G2C_EquipmentLevelUp { Code = ErrorCode.NOFoundItem };
            var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.ItemID);
            if ((ItemType)itemconfig.ItemType != ItemType.Equip)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };
            //装备获取失败
            var equipconfig = ExcelToJSONConfigManager
                .Current.GetConfigByID<EquipmentData>(int.Parse(itemconfig.Params1));
            
            if (equipconfig == null)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };

            var equip = userData.GetEquipByGuid(request.Guid);
            if (equip == null)
            {
                if (!userData.MakeEquipInit(request.Guid))
                {
                    return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };//nofound item?
                }
                else
                {
                    equip = userData.GetEquipByGuid(request.Guid);
                }
            }

            if (equip == null)
            {
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };
            }

            //等级不一样
            if (equip.Level != request.Level)
            {
                return new G2C_EquipmentLevelUp
                { Code = ErrorCode.Error };
            }

            var levelconfig = ExcelToJSONConfigManager
                .Current
                .FirstConfig<EquipmentLevelUpData>(t =>
                {
                    return t.Level == request.Level + 1 && t.Quility == equipconfig.Quility;
                });

            if (levelconfig == null)
            {
                //max level
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };
            }

            if (levelconfig.CostGold > userData.Gold || levelconfig.CostCoin > userData.Coin)
            {
                return new G2C_EquipmentLevelUp { Code = ErrorCode.NoEnoughtGold };
            }


            if (levelconfig.CostGold > 0)
                userData.SubGold(levelconfig.CostGold);
            if (levelconfig.CostCoin > 0)
                userData.SubCoin(levelconfig.CostCoin);

            var levelUp = GRandomer.Probability10000(levelconfig.Pro);
            if (levelUp)
            {
                userData.LevelUpGuild(request.Guid);
            }

            return new G2C_EquipmentLevelUp
            {
                Code = ErrorCode.OK,
                LevelUp = levelUp,
                Coin = userData.Coin,
                Gold = userData.Gold,
                ResultEquip = equip
            };
        }
    }
}

