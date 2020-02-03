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
        public const string GATE_SERVER = "Server";
        public const string ACCOUNT = "Account";
        public const string SESSION = "Session";


        public MongoClient Client { get; set; }

        public IMongoDatabase Data { set; get; }

        public IMongoCollection<PlayInfoEntity> Account { private set; get; }
        public IMongoCollection<GameServerInfoEntity> Servers { private set; get; }
        public IMongoCollection<UserSessionInfoEntity> Session { private set; get; }

        static DataBase()
        {
            BsonClassMap.RegisterClassMap<PlayInfoEntity>(
            (cm) =>
            {
                cm.AutoMap();
                _ = cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<GameServerInfoEntity>(
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
        }

        public void Init(string connectString, string db)
        {
            Client = new MongoClient(connectString);
            Data = Client.GetDatabase(db);
            Account = Data.GetCollection<PlayInfoEntity>(ACCOUNT);
            Servers = Data.GetCollection<GameServerInfoEntity>(GATE_SERVER);
            Session = Data.GetCollection<UserSessionInfoEntity>(SESSION);
            //clear temp data
            Session.DeleteMany(Builders<UserSessionInfoEntity>.Filter.Exists(t => t.AccountUuid, true));
            Servers.DeleteMany(Builders<GameServerInfoEntity>.Filter.Exists(t => t.Host, true));
        }


        public GameServerInfo ToServerInfo(GameServerInfoEntity entity)
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
    }
}
