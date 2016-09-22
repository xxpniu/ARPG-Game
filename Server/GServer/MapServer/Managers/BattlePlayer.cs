using System;
using System.Collections.Generic;
using System.Linq;
using ExcelConfig;
using Proto;

namespace MapServer.Managers
{
    public class BattlePlayer
    {
        public bool IsOK { get { return Hero != null && ClientID > 0; } }

        public int MapID;
        public DHero Hero;
        public PlayerPackage Package;
        public int ClientID;
        public PlayerServerInfo User;
        public DateTime StartTime;
        public int SimulaterIndex;

        private object syncRoot = new object();

        private Dictionary<int, int> dropItems = new Dictionary<int, int>();
        private Dictionary<int, int> consumeItems = new Dictionary<int, int>();

        public int Gold { get; set; }

        private int DifGold = 0;

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
            }
            return new PlayerPackage();
        }

        private int CurrentSize = 0;

        public bool AddDrop(int item, int num)
        {
            var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item);

            lock (syncRoot)
            {
                if (dropItems.ContainsKey(item))
                {
                    dropItems[item] += num;
                }
                else 
                {
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
