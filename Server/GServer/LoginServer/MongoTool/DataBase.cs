using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Proto;
using Proto.MongoDB;
using ServerUtility;

namespace MongoTool
{
    public class DataBase : XSingleton<DataBase>
    {
        public const string BATTLE_SEVERR = "BattleServer";
        public const string GATE_SERVER = "GateServer";
        public const string BATTLE_SERVER = "BattleServer";
        public const string ACCOUNT = "Account";
        public const string SESSION = "Session";


        public MongoClient Client { get; set; }

        public IMongoDatabase Data { set; get; }

        public IMongoCollection<PlayInfoEntity> Account { private set; get; }
        public IMongoCollection<GateServerInfoEntity> GateServer { private set; get; }
        public IMongoCollection<PlayerBattleServerEntity> BattleServer { private set; get; }
        public IMongoCollection<UserSessionInfoEntity> Session { private set; get; }

        static DataBase()
        {
            BsonClassMap.RegisterClassMap<PlayInfoEntity>(
            (cm) =>
            {
                cm.AutoMap();
                _ = cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<GateServerInfoEntity>(
            cm =>
            {
                cm.AutoMap();
                _ = cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
            BsonClassMap.RegisterClassMap<UserSessionInfoEntity>(
            cm =>
            {
                cm.AutoMap();
                _ = cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
            BsonClassMap.RegisterClassMap<PlayerBattleServerEntity>(
           cm =>
           {
               cm.AutoMap();
               _ = cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
           });
        }

        public void Init(string connectString, string db)
        {
            Client = new MongoClient(connectString);
            Data = Client.GetDatabase(db);
            Account = Data.GetCollection<PlayInfoEntity>(ACCOUNT);
            GateServer = Data.GetCollection<GateServerInfoEntity>(GATE_SERVER);
            Session = Data.GetCollection<UserSessionInfoEntity>(SESSION);
            BattleServer = Data.GetCollection<PlayerBattleServerEntity>(BATTLE_SERVER);
            //clear temp data
            Session.DeleteMany(Builders<UserSessionInfoEntity>.Filter.Exists(t => t.AccountUuid, true));
            GateServer.DeleteMany(Builders<GateServerInfoEntity>.Filter.Exists(t => t.Host, true));
            BattleServer.DeleteMany(Builders<PlayerBattleServerEntity>.Filter.Exists(t => t.Host, true));
        }


        public GameServerInfo ToServerInfo(GateServerInfoEntity entity)
        {
            return new GameServerInfo
            {
                CurrentPlayerCount = entity.CurrentPlayerCount,
                Host = entity.Host,
                MaxPlayerCount = entity.MaxPlayerCount,
                Port = entity.Port,
                ServerId = entity.ServerId, ServicesHost= entity.ServiceHost,
                ServicesPort = entity.ServicePort
            };
        }

        public bool GetSessionInfo(string userID, out UserSessionInfoEntity serverInfo)
        {
            var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, userID);
            serverInfo = DataBase.S.Session.Find(filter).SingleOrDefault();
            return serverInfo != null;
        }

        /// <summary>
        /// get battle server id 
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public PlayerBattleServerEntity GetBattleServerByServerID(int serverID)
        {
            var filter = Builders<PlayerBattleServerEntity>.Filter.Eq(t => t.ServerId, serverID);
            return BattleServer.Find(filter).SingleOrDefault();
        }
    }
}
