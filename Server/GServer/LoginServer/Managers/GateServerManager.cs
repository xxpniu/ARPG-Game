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
    }

    [Monitor]
    public class ServerManager:XSingleton<ServerManager>,IMonitor
    {
        public GameServerInfoEntity AddServerByType(int clientID, GameServerInfo serverInfo,ServerType type)
        {
            var entity = new GameServerInfoEntity
            {
                ClientId = clientID,
                CurrentPlayerCount = serverInfo.CurrentPlayerCount,
                Host = serverInfo.Host,
                MaxPlayerCount = serverInfo.MaxPlayerCount,
                Port = serverInfo.Port,
                ServerId = serverInfo.ServerId,
                ServiceHost = serverInfo.ServicesHost,
                ServicePort = serverInfo.ServicesPort,
                Type = type
            };
            var ty_filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.Type, type);
            var filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.ServerId, serverInfo.ServerId);
            if (DataBase.S.Servers.Find(Builders<GameServerInfoEntity>.Filter.And(ty_filter, filter)).Any())
                return null;
            DataBase.S.Servers.InsertOne(entity);
            return entity;
        }

        public ServerMapping GetFreeServerByType(ServerType type)
        {

            var b = Builders<GameServerInfoEntity>.Filter.Eq(t => t.Type, type);
            var fitler = Builders<GameServerInfoEntity>.Filter.Lt(t => t.CurrentPlayerCount, 100000);
            var gate = DataBase.S.Servers.Find(Builders<GameServerInfoEntity>.Filter.And(b, fitler))
                .FirstOrDefault();
            if(gate==null) return null;    
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

        public ServerMapping GetServerMappingByServerIDWithType(int serverID, ServerType type)
        {
            var ty_filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.Type, type);
            var filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.ServerId, serverID);
            var and_fitler = Builders<GameServerInfoEntity>.Filter.And(ty_filter, filter);
            var gate = DataBase.S.Servers.Find(and_fitler).FirstOrDefault();
            return new ServerMapping
            {
                ClientID = gate.ClientId,
                ServerInfo = DataBase.S.ToServerInfo(gate)
            };
        }

        public bool RemoveServerByType(int serverID,ServerType type)
        {
            var ty_filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.Type, type);
            var filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.ServerId, serverID);
            var and_fitler = Builders<GameServerInfoEntity>.Filter.And(ty_filter, filter);
            DataBase.S.Servers.DeleteMany(and_fitler);
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
          
        }

    }
}

