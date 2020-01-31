using System;
using System.Collections.Generic;
using org.vxwo.csharp.json;
using Proto;
using RPCResponsers;
using ServerUtility;
using XNet.Libs.Utility;

namespace LoginServer.Managers
{

    public class UserServerInfo
    {
        public string UserID;
        public int GServerID;
        public int MapID;
        public int BattleServerID;
    }

    [Monitor]
    public class BattleManager : XSingleton<BattleManager>, IMonitor
    {
        public BattleManager()
        {
            
            _servers = new SyncDictionary<string, UserServerInfo>(1024);
        }

        private SyncDictionary<string, UserServerInfo> _servers;

        //玩家退出战斗
        public void ExitBattle(string userID,int serverID)
        {
            if (_servers.TryToGetValue(userID, out UserServerInfo server))
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
                {
                    _servers.Remove(i.Key);
                }
            }
        }

        public bool GetBattleServerByUserID(string userID,out UserServerInfo serverInfo)
        {
            return  _servers.TryToGetValue(userID, out serverInfo);
        }

        //开始进入战斗
        internal ErrorCode BeginBattle(
            string accountUuid,
            int mapID, int serverID, out GameServerInfo serverInfo)
        {
            serverInfo = null;
            var battleServer = ServerManager.Singleton.GetFreeBattleServerID();
            if (battleServer == null) return ErrorCode.NofreeBattleServer;

            if (_servers.TryToGetValue(accountUuid, out UserServerInfo user))
            {
                var task = new Task_L2B_ExitUser { UserID = user.UserID };
                var b = ServerManager.Singleton.GetBattleServerMappingByServerID(user.BattleServerID);
                Appliaction.Current.GetServerConnectByClientID(b.ClientID)?
                    .CreateTask<Task_L2B_ExitUser>( LoginServerTaskServices.S.ExitUser)
                    .Send(()=>task);
                return ErrorCode.PlayerIsInBattle;
            }

            var su = _servers.Add(
                accountUuid,
                new UserServerInfo
                {
                    MapID = mapID,
                    BattleServerID = battleServer.ServerInfo.ServerID,
                    GServerID = serverID,
                    UserID = accountUuid
                });

            if (su)
            {
                serverInfo = battleServer.ServerInfo;
                var gateserver = ServerManager.Singleton.GetGateServerMappingByServerID(serverID);
                var task = new Task_L2B_StartBattle
                {
                    MapID = mapID
                };
                task.Users.Add(new PlayerServerInfo
                {
                    AccountUuid = accountUuid,
                    ServerID = serverID,
                    ServiceHost = gateserver.ServiceHost,
                    ServicePort = gateserver.ServicePort
                });

                if (Appliaction.Current
                    .GetServerConnectByClientID(battleServer.ClientID)?
                    .CreateTask<Task_L2B_StartBattle>(LoginServerTaskServices.S.StartBattle)
                    .Send(()=>task)??false)
                {
                    _servers.Remove(accountUuid);
                    return ErrorCode.BattleServerHasDisconnect;
                }
                return ErrorCode.Ok;
            }
            else
            {
                return ErrorCode.PlayerIsInBattle;
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

