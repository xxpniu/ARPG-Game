using System;
using System.Collections.Generic;
using ExcelConfig;
using Proto;

namespace GServer.Managers
{
    
    public class UserData
    {
        public const int MaxCacheTime = 20 * 60; //20分钟

        private DHero Hero;
        private PlayerPackage Package;

        private DateTime LastAccessTime { set; get; }

        public bool IsDead { get { return MaxCacheTime< (int)(DateTime.Now - LastAccessTime).TotalSeconds; } }

        public bool IsChanged { private set; get; }

        public bool IsEquipChanged { private set; get; }

        public bool IsHeroChanaged { private set; get; }

        public DHero GetHero()
        {
            return Hero;
        }

        public PlayerItem GetItemByGuid(string guid)
        {
            foreach (var i in Package.Items)
            {
                if (i.GUID == guid) return i;
            }
            return null;
        }

        public Equip GetEquipByGuid(string guid)
        {
            foreach (var i in Package.Equips)
            {
                if (i.GUID == guid) return i;
            }
            return null;
        }

        public bool MakeEquipInit(string guid)
        {
            if (GetEquipByGuid(guid) != null) return false;
            Package.Equips.Add(new Equip { GUID = guid, Level = 0 });
            return true;
        }

        internal ErrorCode SaleItem(string guid, int num, List<PlayerItem> diff)
        {
            var item = GetItemByGuid(guid);
            if (item == null) return ErrorCode.NOFoundItem;
            if (IsWear(item.GUID)) return ErrorCode.IsWearOnHero;
            if (item.Num < num) return ErrorCode.NOEnoughItem;
            item.Num -= num;
            var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.ItemID);
            var rewardGold = itemconfig.SalePrice * num;
            Gold += rewardGold;
            if (item.Num == 0)
            {
                Package.Items.RemoveAll(t => t.GUID == guid);
                Package.Equips.RemoveAll(t => t.GUID == guid);
            }
            diff.Add(new PlayerItem { GUID = guid, Num = -num, ItemID = item.ItemID });
            IsChanged = true;
            return ErrorCode.OK;
        }

        public void Pristed()
        {
            IsChanged = false;
            IsEquipChanged = false;
            IsHeroChanaged = false;
        }

        public PlayerPackage GetPackage() { return Package; }

        internal bool SubGold(int subGold)
        {
            if (subGold <= Gold)
            {
                Gold -= subGold;
                IsChanged = true;
                return true;
            }
            return false;
        }

        internal bool SubCoin(int subCoin)
        {
            if (subCoin <= Coin)
            {
                Coin -= subCoin;
                return true;
            }
            return false;
        }

        public UserData(DHero hero, PlayerPackage package, int gold, int coin)
        {
            Hero = hero;
            Package = package;
            Gold = gold;
            Coin = coin;
            IsChanged = false;
            IsEquipChanged = false;
            LastAccessTime = DateTime.Now;
        }

        private int GetTotalNum(int itemID)
        {
            int num = 0;
            foreach (var i in Package.Items)
            {
                if (i.ItemID == itemID)
                    num += i.Num;
            }
            return num;
        }


        public int Gold { private set; get; }
        public int Coin { get; private set; }

        internal bool ConsumeItem(PlayerItem value)
        {
            if (value.Num >= 0) return false;
            if (GetTotalNum(value.ItemID) - value.Num < 0) return false;
            IsChanged = true;
            foreach (var i in Package.Items)
            {
                if (i.ItemID == value.ItemID)
                {
                    if (i.Num + value.Num > 0)
                    {
                        i.Num += value.Num;
                    }
                    else
                    {
                        value.Num += i.Num;
                        i.Num = 0;
                    }
                }
            }
            Package.Items.RemoveAll(t => t.Num == 0);
            return true;
        }

        public bool IsWear(string guid)
        {
            foreach (var i in Hero.Equips)
            {
                if (i.GUID == guid) return true;
            }
            return false;
        }

        internal bool AddItem(PlayerItem value)
        {
            if (value.Num <= 0) return false;
            int max = 0;
            var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemData>(value.ItemID);
            if (config == null) return false;
            IsChanged = true;
            max = config.MaxStackNum;
            if (max == 1)
            {
                for (var i = 0; i < value.Num; i++)
                {
                    Package.Items.Add(new PlayerItem
                    {
                        ItemID = value.ItemID,
                        Num = 1,
                        GUID = Guid.NewGuid().ToString()
                    });
                    if (Package.Items.Count >= Package.MaxSize) return false;
                }
            }
            else
            {
                foreach (var i in Package.Items)
                {
                    //处理堆叠
                    if (i.ItemID == value.ItemID)
                    {
                        int l = max - (i.Num + value.Num);
                        if (l > 0)
                        {
                            i.Num += value.Num;
                            value.Num = 0;
                            break;
                        }
                        else
                        {
                            var diff = max - i.Num;
                            i.Num = max;
                            value.Num -= diff;
                        }
                    }
                }

                while (value.Num > 0)
                {
                    int diff = Math.Min(max, value.Num);
                    Package.Items.Add(new PlayerItem
                    {
                        ItemID = value.ItemID,
                        Num = diff,
                        GUID = Guid.NewGuid().ToString()
                    });
                    value.Num -= diff;
                    if (Package.Items.Count >= Package.MaxSize) return false;
                }
            }

            return true;
        }

        public bool WearEquip(string guid,EquipmentType type)
        {
            var item = GetItemByGuid(guid);
            if (item == null) return false;
            var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.ItemID);
            if ((ItemType)itemconfig.ItemType != ItemType.Equip) 
                return false;
            var equipconfig = ExcelToJSONConfigManager.Current.GetConfigByID<EquipmentData>(int.Parse(itemconfig.Params1));
            var equipmentType = (EquipmentType)equipconfig.PartType;
            if (equipmentType != type) return false;
            Hero.Equips.RemoveAll(t => t.Part == type);
            Hero.Equips.Add(new WearEquip { GUID = guid, EquipID = equipconfig.ID , Part = type });
            IsHeroChanaged = true;
            return true;
        }

        public bool UnWearEquip(EquipmentType type)
        {
            IsHeroChanaged = true;
            Hero.Equips.RemoveAll(t => t.Part == type);

            return true;
        }

        public bool LevelUpGuild(string guid)
        {
            var equip = GetEquipByGuid(guid);
            if (equip == null) return false;
            equip.Level += 1;
            IsEquipChanged = true;
            return true;
        }
    }
}

