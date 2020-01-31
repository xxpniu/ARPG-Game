using System;
using Proto;
using ServerUtility;
using XNet.Libs.Utility;

namespace LoginServer.Managers
{

    public class ServerMapping
    {
        public int ClientID;
        public GameServerInfo ServerInfo;
        public int ServicePort;
        public string ServiceHost;
        public int ServiceCount;
    }

    public class ServerManager:XSingleton<ServerManager>
    {
        public ServerManager()
        {
            Servers = new SyncDictionary<int, ServerMapping>();
            BattleServers = new SyncDictionary<int, ServerMapping>();
        }

        #region Battle Servers 
        private volatile int TempIndexBattleServer = 1;

        private SyncDictionary<int, ServerMapping> Servers {  set; get; }

        internal ServerMapping GetFreeBattleServerID()
        {
            var values = BattleServers.Values;
            foreach (var i in values)
            {
                if (i.ServiceCount < i.ServerInfo.MaxServiceCount)
                    return i;
            }
            return null;
        }


        private SyncDictionary<int, ServerMapping> BattleServers {  set; get; }

        public int AddBattleServer(int clientID, GameServerInfo serverInfo)
        {
            var index = TempIndexBattleServer++;
            BattleServers.Add(index, new ServerMapping
            {
                ClientID = clientID,
                ServerInfo = serverInfo
            });
            return index;
        }

        public ServerMapping GetBattleServerMappingByServerID(int serverID)
        {
            ServerMapping mapp;
            BattleServers.TryToGetValue(serverID, out mapp);
            return mapp;
        }

        internal bool RemoveBattleServer(int serverID)
        {
            return BattleServers.Remove(serverID);
        }
        #endregion

        #region GateServer

        public ServerMapping GetFreeGateServer()
        {
            foreach (var i in Servers)
            {
                if (i.Value.ServiceCount < i.Value.ServerInfo.MaxServiceCount) return i.Value;
            }
            return null;
        }

        public bool AddGateServer(int clientID,int currentCount, GameServerInfo info, string serviceHost, int servicePort)
        {
            return Servers.Add(info.ServerID, new ServerMapping
            {
                ClientID = clientID,
                ServerInfo = info,
                ServiceHost = serviceHost,
                ServicePort = servicePort,
                ServiceCount = currentCount
            });
        }

        public ServerMapping GetGateServerMappingByServerID(int serverID)
        {
            Servers.TryToGetValue(serverID, out ServerMapping mapp);
            return mapp;
        }

        public bool RemoveGateServer(int serverID)
        {
            return Servers.Remove(serverID);
        }

        #endregion
    }
}

