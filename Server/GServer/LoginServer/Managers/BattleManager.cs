using System;
using System.Collections.Generic;
using org.vxwo.csharp.json;
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
        public void ExitBattle(long userID,int serverID)
        {
            UserServerInfo server;
            if (_servers.TryToGetValue(userID, out server))
            {
                if (server.BattleServerID == serverID)
                {
                    _servers.Remove(userID);
                }
            }
        }

        //server close 
        public void ServerClose(int serverID)
        {
            foreach (var i in _servers)
            {
                if (i.Value.BattleServerID == serverID)
                    _servers.Remove(i.Key);
            }
        }

        public bool GetBattleServerByUserID(long userID,out UserServerInfo serverInfo)
        {
            return  _servers.TryToGetValue(userID, out serverInfo);
        }

        //开始进入战斗
        internal ErrorCode BeginBattle(
            long userID,
            int mapID, int serverID, out GameServerInfo serverInfo)
        {
            serverInfo = null;
            var battleServer = ServerManager.Singleton.GetFreeBattleServerID();
            if (battleServer == null) return ErrorCode.NOFreeBattleServer;
            var su = _servers.Add(
                userID,
                new UserServerInfo
                {
                    MapID = mapID,
                    BattleServerID = battleServer.ServerInfo.ServerID,
                    GServerID = serverID,
                    UserID = userID
                });
            if (su)
            {
                serverInfo = battleServer.ServerInfo;
                var gateserver = ServerManager.Singleton.GetGateServerMappingByServerID(serverID);
                var task = new Task_L2B_StartBattle
                {
                    MapID = mapID,
                    Users = new List<PlayerServerInfo> {
                        new PlayerServerInfo {
                            UserID = userID,
                            ServerID = serverID ,
                            ServiceHost = gateserver.ServiceHost,
                            ServicePort = gateserver.ServicePort
                        } }
                };

                if (NetProtoTool.EnableLog)
                {
                    Debuger.Log(task.GetType() + "-->" + JsonTool.Serialize(task));
                }
                //task 
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
            else 
            {
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

