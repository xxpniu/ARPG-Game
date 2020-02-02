using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Proto.MongoDB;
using ServerUtility;

namespace MongoTool
{
    public class DataBase : XSingleton<DataBase>
    {
        public const string BATTLE_SEVERR = "BattleServer";
        public const string GATE_SERVER = "GateServer";
        public const string ACCOUNT = "Account";
        public const string SESSION = "Session";


        public MongoClient Client { get; set; }

        public IMongoDatabase Data { set; get; }

        public IMongoCollection<PlayInfoEntity> Account { private set; get; }
        public IMongoCollection<GameServerInfoEntity> GateServer { private set; get; }
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
            GateServer = Data.GetCollection<GameServerInfoEntity>(GATE_SERVER);
            Session = Data.GetCollection<UserSessionInfoEntity>(SESSION);
            //clear temp data
            Session.DeleteMany(Builders<UserSessionInfoEntity>.Filter.Exists(t => t.AccountUuid, true));
            GateServer.DeleteMany(Builders<GameServerInfoEntity>.Filter.Exists(t => t.Host, true));
        }

    }
}
