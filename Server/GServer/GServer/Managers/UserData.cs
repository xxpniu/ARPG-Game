using System;
using System.Collections.Generic;
using System.IO;
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

        //User table
        public bool IsChanged { private set; get; }

        //equiptable
        public bool IsEquipChanged { private set; get; }

        //hero table
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

        internal void HeroLevelTo(int level)
        {
            Hero.Level = level;
            IsHeroChanaged = true;
        }

        internal bool AddExp(int exp, out int level)
        {
            level = Hero.Level;
            return true;
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

        internal bool AddGold(int gold)
        {
            if (gold > 0)
            {
                Gold += gold;
                IsChanged = true;
            }
            else
            {
                return false;
            }
            return true;
        }

        internal bool AddCoin(int coin)
        {
            if (coin > 0)
            {
                Coin += coin;
                IsChanged = true;
            }
            else
            {
                return false;
            }
            return true;
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
            if (subGold <= 0) return false;
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
            if (subCoin <= 0) return false;
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
            var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(value.ItemID);
            if (config == null) return false;
            IsChanged = true;
            max = config.MaxStackNum;
            if (config.Unique==0)
            {
                for (var i = 0; i < value.Num; i++)
                {
                    if (Package.Items.Count >= Package.MaxSize) return false;
                    Package.Items.Add(new PlayerItem
                    {
                        ItemID = value.ItemID,
                        Num = 1,
                        GUID = Guid.NewGuid().ToString()
                    });
                }
            }
            else
            {
                bool have = false;
                foreach (var i in Package.Items)
                {
                    //处理堆叠
                    if (i.ItemID == value.ItemID)
                    {
                        var result = Math.Min( max , i.Num+value.Num);
                        i.Num = result;
                        have = true;
                    }
                }

                if (!have)
                {
                   Package.Items.Add(new PlayerItem
                   {
                       ItemID = value.ItemID,
                       Num = Math.Min(max,value.Num),
                       GUID = Guid.NewGuid().ToString()
                   }); 
                }

            }

            return true;
        }

        internal void Accessed()
        {
            LastAccessTime = DateTime.Now;
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

        private PlayerPackage packagetemp;
        private int goldTemp = -1;
        private int coinTemp =-1;

        public void RecordPackage()
        {

            goldTemp = Gold;
            coinTemp = Coin;
            byte[] data;
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    Package.ToBinary(bw);
                }
                data = mem.ToArray();
            }

            using (var mem = new MemoryStream(data))
            {
                using (var br = new BinaryReader(mem))
                {
                    packagetemp = new PlayerPackage();
                    packagetemp.ParseFormBinary(br);
                }
            }
        }

        public void RevertPackage()
        {
            if (goldTemp < 0 || coinTemp <=0 || packagetemp ==null) return;
            Package = packagetemp;
            Gold = goldTemp;
            Coin = coinTemp;
            packagetemp = null;
            goldTemp = -1;
            coinTemp = 1;
        }
    }
}

