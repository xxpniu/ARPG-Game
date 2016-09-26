using System;
using System.Collections.Generic;
using System.Linq;
using ExcelConfig;
using Proto;

namespace MapServer.Managers
{
    public class BattlePlayer
    {

        #region Property
        public bool IsOK { get { return Hero != null && ClientID > 0; } }

        public int MapID;
        private DHero Hero;
        private PlayerPackage Package;
        public int ClientID;
        public PlayerServerInfo User;
        public DateTime StartTime;
        public int SimulaterIndex;

        private object syncRoot = new object();

        private Dictionary<int, int> dropItems = new Dictionary<int, int>();
        private Dictionary<int, int> consumeItems = new Dictionary<int, int>();

        public int Gold { get; set; }

        private int DifGold = 0;

        #endregion


        public DHero GetHero() { return Hero; }

        public void SetHero(DHero hero) { Hero = hero; }

        public void SetPackage(PlayerPackage package) 
        {
            Package = package;
            CurrentSize = package.Items.Count;
        }



        public bool SubGold(int gold)
        {
            if (Gold - (DifGold + gold) < 0) return false;
            DifGold += gold;
            return true;
        }

        public bool AddGold(int gold)
        {
            if (gold <= 0) return false;
            DifGold -= gold;
            return true;
        }

        //not completed
        public Notify_PlayerJoinState GetNotifyPackage()
        {
            lock (syncRoot)
            {
                var notify = new Proto.Notify_PlayerJoinState()
                {
                    UserID = User.UserID,
                    Gold = Gold + DifGold,
                    Package = GetCompletedPackage()
                };
                return notify;
            }
        }

        private PlayerPackage GetCompletedPackage()
        {
            var result = new PlayerPackage();
            lock(syncRoot)
            {
                foreach (var i in Package.Items)
                { 
                    result.Items.Add(i);
                    result.MaxSize = Package.MaxSize;
                }
            }
            return result;
        }

        public int CurrentSize { private set; get; }

        public bool AddDrop(int item, int num)
        {
            if (CurrentSize >= Package.MaxSize) return false;

            var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item);
            if (config == null) return false;
            lock (syncRoot)
            {
                if (dropItems.ContainsKey(item))
                {
                    dropItems[item] += num;
                }
                else
                {
                    if (config.Unique == 0) 
                    {
                        CurrentSize += 1;
                    }
                    else
                    {
                        bool have = false;
                        foreach (var i in Package.Items)
                        {
                            if (i.ItemID == item) {
                                have = true;
                            }
                        }

                        if (!have)
                        {
                            CurrentSize+=1;
                        }
                    }
                    dropItems.Add(item, num);
                }
            }
            return true;
        }

        public bool ConsumeItem(int item, int num)
        {
            lock (syncRoot)
            {
                int consumeNum = 0;
                consumeItems.TryGetValue(item, out consumeNum);
                //是否足够
                {
                    bool enough = false;
                    foreach (var i in this.Package.Items)
                    {
                        if (i.ItemID == item)
                        {
                            if (i.Num < consumeNum + num) return false;
                            else {
                                enough = true;
                                break;
                            }
                        }
                    }
                    if (!enough) return false;
                }
                if (consumeItems.ContainsKey(item))
                {
                    consumeItems[item] += num;
                }
                else {
                    consumeItems.Add(item, num);
                }
            }
            return true;
        }

        public List<PlayerItem> DropItems
        {
            get
            {
                lock (syncRoot)
                {
                    return dropItems.Select(t => new PlayerItem { ItemID = t.Key, Num = t.Value }).ToList();
                }
            }
        }

        public List<PlayerItem> ConsumeItems
        {
            get
            {
                lock (syncRoot)
                {
                    return this.consumeItems.Select(t => new PlayerItem { ItemID = t.Key, Num = t.Value }).ToList();
                }
            }
        }

        internal Equip GetEquipByGuid(string gUID)
        {

            foreach (var i in Package.Equips)
            {
                if (i.GUID == gUID) return i;
            }
            return null;
        }
    }
}
