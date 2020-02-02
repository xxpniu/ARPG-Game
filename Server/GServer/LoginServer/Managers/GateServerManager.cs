using System;
using MongoDB.Driver;
using MongoTool;
using Proto;
using Proto.MongoDB;
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

    [Monitor]
    public class ServerManager:XSingleton<ServerManager>,IMonitor
    {

       
        #region Battle Servers 
        private volatile int TempIndexBattleServer = 1;

        public void Init()
        {
            
        }

        internal ServerMapping GetFreeBattleServerID()
        {
            throw new NotImplementedException();
        }

        public int AddBattleServer(int clientID, GameServerInfo serverInfo)
        {
            throw new NotImplementedException();
        }

        public ServerMapping GetBattleServerMappingByServerID(int serverID)
        {
            throw new NotImplementedException();
        }

        internal bool RemoveBattleServer(int serverID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GateServer

        public ServerMapping GetFreeGateServer()
        {
            
            var fitler = Builders<GameServerInfoEntity>.Filter.Gt(t => t.MaxPlayerCount - t.CurrentPlayerCount, 0);
            var gate = DataBase.S.GateServer.Find(fitler).FirstOrDefault();
            return new ServerMapping
            {
                ClientID = gate.ClientId,
                ServerInfo = new GameServerInfo
                {
                    CurrentPlayerCount = gate.CurrentPlayerCount,
                    Host = gate.Host,
                    MaxPlayerCount = gate.MaxPlayerCount,
                    ServerId = gate.ServerId, Port = gate.Port
                }
            };
        }

        public bool AddGateServer(int clientID, int currentCount, GameServerInfo info, string serviceHost, int servicePort)
        {;
            var entity = new GameServerInfoEntity
            {
                ClientId = clientID,
                ServerId = info.ServerId,
                CurrentPlayerCount = currentCount,
                Host = serviceHost,
                MaxPlayerCount = info.MaxPlayerCount,
                Port = servicePort
            };
            DataBase.S.GateServer.InsertOne(entity);
            return true;
        }

        public ServerMapping GetGateServerMappingByServerID(int serverID)
        {
            var fitler = Builders<GameServerInfoEntity>.Filter.Eq(t => t.ServerId, serverID);
            var gate = DataBase.S.GateServer.Find(fitler).FirstOrDefault();
            return new ServerMapping
            {
                ClientID = gate.ClientId,
                ServerInfo = new GameServerInfo
                {
                    CurrentPlayerCount = gate.CurrentPlayerCount,
                    Host = gate.Host,
                    MaxPlayerCount = gate.MaxPlayerCount,
                    ServerId = gate.ServerId,
                    Port = gate.Port
                }
            };
        }

        public bool RemoveGateServer(int serverID)
        {
            var fitler = Builders<GameServerInfoEntity>.Filter.Eq(t => t.ServerId, serverID);
            DataBase.S.GateServer.DeleteMany(fitler);
            return true;
        }

        public void OnShowState()
        {
            //throw new NotImplementedException();
        }

        public void OnTick()
        {
            //throw new NotImplementedException();
        }

        public void OnExit()
        {
            //throw new NotImplementedException();
        }

        public void OnStart()
        {
            Init();
            //throw new NotImplementedException();
        }

        #endregion
    }
}

