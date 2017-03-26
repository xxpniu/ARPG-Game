using System;
using System.Collections.Generic;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;
using System.Linq;

namespace MapServer.Managers
{


    [Monitor]
    public class MapSimulaterManager:IMonitor
    {
        private SyncDictionary<long, BattlePlayer> UserSimulaterMapping = new SyncDictionary<long, BattlePlayer>();

        //缓存需要处理的玩家 进入世界后清除
        private SyncDictionary<long, BattlePlayer> _battlePlayers = new SyncDictionary<long, BattlePlayer>();

        private SyncList<WorkThread<ServerWorldSimluater>> worksThread = new SyncList<WorkThread<ServerWorldSimluater>>();

        public void AddUser(PlayerServerInfo user, int mapID)
        {
            var userInfo = new BattlePlayer
            {
                ClientID = -1,
                User = user,
                MapID = mapID,
                StartTime = DateTime.UtcNow,
                SimulaterIndex = -1
            };

            if (_battlePlayers.Add(user.UserID, userInfo))
            {
                if (!UserSimulaterMapping.Add(user.UserID, userInfo))
                {
                    DeleteUser(user.UserID, false);
                    Debuger.LogError("user " + user.UserID + " is in battle!");
                }
            }
            else 
            {
                Debuger.LogError("user "+ user.UserID +" is in battle!");
            }
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
            BattlePlayer battlePlayer;
            if (UserSimulaterMapping.TryToGetValue(userID, out battlePlayer))
            {
                if (battlePlayer.SimulaterIndex > 0)
                {
                    MonitorPool.Singleton.GetMointor<SimulaterManager>().ExitUser(userID, battlePlayer.SimulaterIndex);
                }
            }
            DeleteUser(userID, true);
        }

        public void DeleteUser(long userID, bool update = false)
        {
            BattlePlayer battlePlayer;
            if (UserSimulaterMapping.TryToGetValue(userID, out battlePlayer))
            {
                if (battlePlayer.ClientID > 0)
                {
                    var client = Appliaction.Current.GetClientByID(battlePlayer.ClientID);
                    if (client != null)
                    {
                        client.Close();
                    }
                }

                if (update && battlePlayer.GetHero()!=null)
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
            var si =MonitorPool.Singleton.GetMointor<SimulaterManager>().BeginSimulater(user);
            foreach (var i in worksThread.ToList())
            {
                if (i.Count >= i.MaxUpdaterPerThread) continue;
                i.AddThread(si);
            }
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
                    else if (i.GetHero() == null)
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
                                i.SetHero(r.Hero);
                                i.SetPackage(r.Package);
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
            this.worksThread.Add(new WorkThread<ServerWorldSimluater>(15, 100));
            this.worksThread.Add(new WorkThread<ServerWorldSimluater>(15, 100));
            this.worksThread.Add(new WorkThread<ServerWorldSimluater>(15, 100));
            foreach (var i in worksThread.ToList())
                i.Start();
        }

   }
}

