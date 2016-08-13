using System;
using System.Collections.Generic;
using org.vxwo.csharp.json;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;

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
    }

    [Monitor]
    public class MapSimulaterManager:IMonitor
    {

        private SyncDictionary<long, BattlePlayer> _battlePlayers = new SyncDictionary<long, BattlePlayer>();

        public static MapSimulaterManager Singleton { private set; get; }

        public MapSimulaterManager()
        {
            Singleton = this;
        }

        public void AddUser(PlayerServerInfo userID, int mapID)
        {
            _battlePlayers.Add(userID.UserID, new BattlePlayer
            {
                ClientID = -1,
                Hero = null,
                Package = null,
                User = userID,
                MapID = mapID,
                StartTime = DateTime.UtcNow
                                    
            });
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
            BattlePlayer battlePlayer;
            if (_battlePlayers.TryToGetValue(userID, out battlePlayer))
            {
                if (battlePlayer.ClientID > 0)
                {
                    var m = new Task_B2C_ExitBattle();
                    var message = NetProtoTool.ToNetMessage(MessageClass.Task, m);
                    var client = Appliaction.Current.GetClientByID(battlePlayer.ClientID);
                    if (client != null)
                        client.SendMessage(message);
                }
            }
            _battlePlayers.Remove(userID);
            var request = Appliaction.Current.Client.CreateRequest<B2L_EndBattle, L2B_EndBattle>();
            request.RequestMessage.UserID = userID;
            request.SendRequestSync();
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
                        _battlePlayers.Remove(i.User.UserID);
                        SimulaterManager.Singleton.BeginSimulater(i);
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
            //throw new NotImplementedException();
        }

        public void OnStart()
        {
           // throw new NotImplementedException();
        }
    }
}

