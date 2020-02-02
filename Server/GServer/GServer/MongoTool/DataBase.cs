using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Proto.MongoDB;
using ServerUtility;

namespace GateServer
{
    public class DataBase:XSingleton<DataBase>
    {
        public DataBase()
        {
            BsonClassMap.RegisterClassMap<GamePlayerEntity>(
            (cm) =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<ItemPackageEntity>(
            (cm) =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<GameHeroEntity>(
            (cm) =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }

        public const string PLAYER = "Player";
        public const string HERO = "Hero";
        public const string PACKAGE = "Package";

        public IMongoCollection<GamePlayerEntity> Playes { get; private set; }
        public IMongoCollection<GameHeroEntity> Heros { get; private set; }
        public IMongoCollection<ItemPackageEntity> Packages { get; private set; }

        public void Init(string connectString, string dbName)
        {
            var mongo = new MongoClient(connectString);
            var db = mongo.GetDatabase(dbName);

            Playes = db.GetCollection<GamePlayerEntity>(PLAYER);
            Heros = db.GetCollection<GameHeroEntity>(HERO);
            Packages = db.GetCollection<ItemPackageEntity>(PACKAGE);

        }
    }
}
