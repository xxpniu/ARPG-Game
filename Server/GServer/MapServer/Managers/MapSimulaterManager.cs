using System;
using System.Collections.Generic;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;
using System.Linq;
using Proto.GateBattleServerService;
using Proto.LoginBattleGameServerService;

namespace MapServer.Managers
{


    [Monitor]
    public class MapSimulaterManager:IMonitor
    {
        private SyncDictionary<string, BattlePlayer> UserSimulaterMapping = new SyncDictionary<string, BattlePlayer>();

        //缓存需要处理的玩家 进入世界后清除
        private SyncDictionary<string, BattlePlayer> _battlePlayers = new SyncDictionary<string, BattlePlayer>();

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

            if (_battlePlayers.Add(user.AccountUuid, userInfo))
            {
                if (!UserSimulaterMapping.Add(user.AccountUuid, userInfo))
                {
                    DeleteUser(user.AccountUuid, false);
                    Debuger.LogError($"user {user.AccountUuid } is in battle!");
                }
            }
            else 
            {
                Debuger.LogError($"user { user.AccountUuid } is in battle!");
            }
        }

        public bool BindUser(string account_uuid, int clientID)
        {
            if (_battlePlayers.TryToGetValue(account_uuid, out BattlePlayer player))
            {
                player.ClientID = clientID;
                return true;
            }
            return false;
        }

        public void OnShowState()
        {
            
        }

        private void LoginFailure(string account_uuid)
        {
            DeleteUser(account_uuid);
        }

        public void KickUser(string account_uuid)
        {
            if (UserSimulaterMapping.TryToGetValue(account_uuid, out BattlePlayer battlePlayer))
            {
                if (battlePlayer.SimulaterIndex > 0)
                {
                    MonitorPool.Singleton.GetMointor<SimulaterManager>()
                        .ExitUser(account_uuid, battlePlayer.SimulaterIndex);
                }
            }
            DeleteUser(account_uuid, true);
        }

        public void DeleteUser(string account_uuid, bool update = false)
        {
            if (UserSimulaterMapping.TryToGetValue(account_uuid, out BattlePlayer battlePlayer))
            {
                if (battlePlayer.ClientID > 0)
                {
                    var client = Appliaction.Current.GetClientByID(battlePlayer.ClientID);
                    if (client != null)
                    {
                        client.Close();
                    }
                }

                if (update && battlePlayer.GetHero() != null)
                {
                    var client = Appliaction.Current.GetGateServer(battlePlayer.User.ServerID);
                    if (client != null)
                    {
                        var request = new B2G_BattleReward
                        {
                            //DropItems = battlePlayer.DropItems,
                            Gold = battlePlayer.Gold,
                            MapID = battlePlayer.MapID,
                            AccountUuid = battlePlayer.User.AccountUuid
                        };

                        foreach (var i in battlePlayer.DropItems)
                        {
                            request.DropItems.Add(i);
                        }

                        foreach (var i in battlePlayer.ConsumeItems)
                        {
                            request.ConsumeItems.Add(i);
                        }

                        BattleReward.CreateQuery()
                            .SendRequestAsync(client, request)
                            .GetAwaiter().GetResult();
                    }
                }
            }
            EndBattle.CreateQuery()
                   .SendRequestAsync(Appliaction.Current.Client, new B2L_EndBattle { UserID = account_uuid })
                   .GetAwaiter().GetResult();

            UserSimulaterMapping.Remove(account_uuid);
            _battlePlayers.Remove(account_uuid);
        }

        private void BeginSimulater(BattlePlayer user)
        {
            _battlePlayers.Remove(user.User.AccountUuid);
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
                        LoginFailure(i.User.AccountUuid);  
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
                        var serverConnect = Appliaction.Current.GetGateServer(i.User.ServerID);
                        if (serverConnect == null || !serverConnect.IsConnect) continue;
                        var r= GetPlayerInfo.CreateQuery()
                            .SendRequestAsync(serverConnect,
                            new B2G_GetPlayerInfo
                            {
                                ServiceServerID = Appliaction.Current.ServerID,
                                AccountUuid = i.User.AccountUuid
                            })
                            .GetAwaiter().GetResult();
                        if (r.Code == ErrorCode.Ok)
                        {
                            i.SetHero(r.Hero);
                            i.SetPackage(r.Package);
                        }
                        else
                        {
                            LoginFailure(i.User.AccountUuid);
                        }
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

