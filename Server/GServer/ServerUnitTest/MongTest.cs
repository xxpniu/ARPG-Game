using System;
using System.Threading;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Proto.LoginBattleGameServerService;
using Proto.MongoDB;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;
using Xunit;

namespace ServerUnitTest
{
    public class MongTest
    {

        static MongTest()
        {
            BsonClassMap.RegisterClassMap<PlayInfoEntity>(
           (cm) =>
           {
               cm.AutoMap();
               cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);

           });
        }

        private readonly string url = @"mongodb+srv://dbuser:54249636@cluster0-us8pa.gcp.mongodb.net/test?retryWrites=true&w=majority";

        [Fact]
        public  void TestAsync()
        {
            
        }


        [Theory]
        [InlineData("admin","123456")]
        [InlineData("test", "123456")]
        public void InsertEntity(string user, string passw)
        {
            var client = new MongoClient(url);
            var db = client.GetDatabase("Game");
            var users = db.GetCollection<PlayInfoEntity>("userinfo");
            var entity = new PlayInfoEntity
            {
                CreateDateTime = DateTime.Now.Ticks,
                LastLoginDateTime = DateTime.Now.Ticks,
                LoginCount = 0,
                Password = Md5Tool.GetMd5Hash(passw),
                ServerId = 1,
                Username = user
            };
            users.InsertOne(entity);
            Assert.NotEmpty(entity.Uuid);
        }

        [Fact]
        public void RequestLoginServer()
        {
            var client = new RequestClient<TaskHandler>("127.0.0.1", 1800);

            client.ConnectAsync().Wait();

            Assert.True(client.IsConnect);
            var req = RegGateServer .CreateQuery().GetResult(client,
                new  Proto.G2L_GateServerReg
                {
                    Port = 0,
                    CurrentPlayer = 100,
                    Host = "127.0.0.1",
                    MaxPlayer = 10000,
                    ServerID = 999,
                    ServiceHost = "127.0.0.1",
                    ServicesProt = 1000,
                    Version = 1
                });

            Assert.True(req.Code == Proto.ErrorCode.Ok);

            Console.WriteLine(req);

            client.Disconnect();
        }
    }
}
