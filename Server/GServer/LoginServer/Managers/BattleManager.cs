using System;
using System.Collections.Generic;
using Proto;
using ServerUtility;
using XNet.Libs.Utility;

namespace LoginServer.Managers
{

    public class UserServerInfo
    {
        public long UserID;
        public int GServerID;
        public int MapID;
        public int BattleServerID;
        public bool SendTask;
    }

    [Monitor]
    public class BattleManager : XSingleton<BattleManager>, IMonitor
    {
        public BattleManager()
        {
            _servers = new SyncDictionary<long, UserServerInfo>();
        }

        private SyncDictionary<long, UserServerInfo> _servers;

        //玩家退出战斗
        public void ExitBattle(long userID)
        {
            _servers.Remove(userID);
        }

        //开始进入战斗
        internal Proto.ErrorCode BeginBattle(
            long userID, 
            int mapID, int serverID,out GameServerInfo serverInfo)
        {
            serverInfo = null;
            var battleServer = ServerManager.Singleton.GetFreeBattleServerID();
            if (battleServer == null) return Proto.ErrorCode.NOFreeBattleServer;
            var su = _servers.Add(
                userID,
                new UserServerInfo
                {
                    MapID = mapID,
                    BattleServerID = battleServer.ServerInfo.ServerID,
                    GServerID = serverID,
                    UserID = userID,
                    SendTask = false
            });
            if (su)
            {
                serverInfo = battleServer.ServerInfo;
                var gateserver = ServerManager.Singleton.GetGateServerMappingByServerID(serverID);
                var task = new Task_L2B_StartBattle
                {
                    MapID = mapID,
                    Users =new List<PlayerServerInfo> { 
                        new PlayerServerInfo { 
                            UserID = userID, 
                            ServerID = serverID ,
                            ServiceHost = gateserver.ServiceHost,
                            ServicePort = gateserver.ServicePort
                        } }
                };

                var message = NetProtoTool.ToNetMessage(XNet.Libs.Net.MessageClass.Task, task);
                var serverConnect = Appliaction.Current.GetServerConnectByClientID(battleServer.ClientID);
                if (serverConnect == null)
                {
                    _servers.Remove(userID);
                    return ErrorCode.BattleServerHasDisconnect;
                }
                serverConnect.SendMessage(message);

                return Proto.ErrorCode.OK;
            }
            else {
                return Proto.ErrorCode.PlayerIsInBattle;
            }
        }

        public void OnTick()
        {
            
        }

        public void OnShowState()
        {

        }

        public void OnExit()
        {

        }

        public void OnStart()
        {

        }
    }
}

