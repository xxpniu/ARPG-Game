using System;
using System.Collections.Generic;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;
using System.Linq;

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

        public bool ModifyGold(int gold)
        {
            if (Gold - (DifGold + gold) < 0) return false;
            DifGold += gold;
            return true;
        }

        //not completed
        public Proto.Notify_PlayerJoinState GetNotifyPackage()
        {
            lock (syncRoot)
            {
                var notify = new Proto.Notify_PlayerJoinState()
                {
                    UserID = User.UserID,
                    Gold = Gold + DifGold
                };
                return notify;
            }
        }

        public void AddDrop(int item, int num)
        {
            lock (syncRoot)
            {
                if (dropItems.ContainsKey(item))
                {
                    dropItems[item] += num;
                }
                else {
                    dropItems.Add(item, num);
                }
            }
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
                    return dropItems.Select(t => new PlayerItem { ItemID = t.Key, Num =t.Value }).ToList();
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
    }

    [Monitor]
    public class MapSimulaterManager:IMonitor
    {
        private SyncDictionary<long, BattlePlayer> UserSimulaterMapping = new SyncDictionary<long, BattlePlayer>();

        //缓存需要处理的玩家 进入世界后清除
        private SyncDictionary<long, BattlePlayer> _battlePlayers = new SyncDictionary<long, BattlePlayer>();

        public static MapSimulaterManager Singleton { private set; get; }

        public MapSimulaterManager()
        {
            Singleton = this;
        }

        public void AddUser(PlayerServerInfo user, int mapID)
        {
            var userInfo = new BattlePlayer
            {
                ClientID = -1,
                Hero = null,
                Package = null,
                User = user,
                MapID = mapID,
                StartTime = DateTime.UtcNow,
                SimulaterIndex = -1
            };
            _battlePlayers.Add(user.UserID,userInfo);
            UserSimulaterMapping.Add(user.UserID, userInfo);
        }

        public bool BindUser(long userID, int clientID)
        {
            BattlePlayer player;
            if (_battlePlayers.TryToGetValue(userID, out player))
            {
                player.ClientID = clientID;
                return true;
            }
            return false;
        }

        public void OnShowState()
        {
            
        }

        private void LoginFailure(long userID)
        {
            DeleteUser(userID);
        }

        public void KickUser(long userID)
        {
            //battle
            BattlePlayer battlePlayer;
            if (UserSimulaterMapping.TryToGetValue(userID, out battlePlayer))
            {
                if (battlePlayer.SimulaterIndex > 0)
                {
                    //do nothing
                }
            }
        }

        public void DeleteUser(long userID, bool update = false)
        {
            BattlePlayer battlePlayer;
            if (UserSimulaterMapping.TryToGetValue(userID, out battlePlayer))
            {
                if (battlePlayer.ClientID > 0)
                {
                    var m = new Task_B2C_ExitBattle();
                    var message = NetProtoTool.ToNetMessage(MessageClass.Task, m);
                    var client = Appliaction.Current.GetClientByID(battlePlayer.ClientID);
                    if (client != null)
                        client.SendMessage(message);
                }

                if (update && battlePlayer.Hero!=null)
                {
                    var client = Appliaction.Current.GetGateServer(battlePlayer.User.ServerID);
                    if (client != null)
                    {
                        var rewardRequest = client.CreateRequest<B2G_BattleReward, G2B_BattleReward>();
                        rewardRequest.RequestMessage.DropItems = battlePlayer.DropItems;
                        rewardRequest.RequestMessage.ConsumeItems = battlePlayer.ConsumeItems;
                        rewardRequest.RequestMessage.Gold = battlePlayer.Gold;
                        rewardRequest.RequestMessage.MapID = battlePlayer.MapID;
                        rewardRequest.RequestMessage.UserID = battlePlayer.User.UserID;
                        rewardRequest.SendRequestSync();
                    }
                }
            }

            //notify login server
            var request = Appliaction.Current.Client.CreateRequest<B2L_EndBattle, L2B_EndBattle>();
            request.RequestMessage.UserID = userID;
            request.SendRequestSync();

            UserSimulaterMapping.Remove(userID);
            _battlePlayers.Remove(userID);
        }

        private void BeginSimulater(BattlePlayer user)
        {
            _battlePlayers.Remove(user.User.UserID);
            SimulaterManager.Singleton.BeginSimulater(user);
        }

        public void OnTick()
        {
            if (_battlePlayers.Count > 0)
            {
                foreach (var i in _battlePlayers.Values)
                {
                    if ((DateTime.UtcNow - i.StartTime).TotalSeconds > 10)
                    {
                        LoginFailure(i.User.UserID);  
                        continue;
                    }
                    if (i.IsOK)
                    {
                        BeginSimulater(i);
                    }
                    else if (i.ClientID < 0)
                    {
                        continue;
                    }
                    else if (i.Hero == null)
                    {
                        RequestClient serverConnect = Appliaction.Current.GetGateServer(i.User.ServerID);
                        if (serverConnect == null || !serverConnect.IsConnect) continue;
                        var request = serverConnect.CreateRequest<B2G_GetPlayerInfo, G2B_GetPlayerInfo>();
                        request.RequestMessage.UserID = i.User.UserID;
                        request.RequestMessage.ServiceServerID = Appliaction.Current.ServerID;
                        request.OnCompleted = (s, r) => 
                        {
                            if (r.Code == ErrorCode.OK)
                            {
                                i.Hero = r.Hero;
                                i.Package = r.Package;
                            }
                            else {
                                LoginFailure(i.User.UserID);
                            }
                        };
                        request.SendRequest();
                    }
                }
            }
        }

        public void OnExit()
        {
            _battlePlayers.Clear();
            UserSimulaterMapping.Clear();
        }

        public void OnStart()
        {
           
        }

   }
}

