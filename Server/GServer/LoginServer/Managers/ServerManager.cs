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

        public bool AddGateServer(int clientID, GameServerInfo info, string serviceHost, int servicePort)
        {
            return Servers.Add(info.ServerID, new ServerMapping
            {
                ClientID = clientID,
                ServerInfo = info ,
                ServiceHost = serviceHost,
                ServicePort = servicePort
            });
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

        public ServerMapping GetGateServerMappingByServerID(int serverID)
        {
            ServerMapping mapp;
            Servers.TryToGetValue(serverID, out mapp);
            return mapp;
        }

        public bool RemoveGateServer(int serverID)
        {
            return Servers.Remove(serverID);
        }
    }
}

