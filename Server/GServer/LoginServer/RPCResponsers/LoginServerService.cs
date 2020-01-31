using System;
using Proto;
using Proto.LoginServerService;
using Proto.MongoDB;
using ServerUtility;
using XNet.Libs.Net;
using LoginServer;
using MongoDB.Driver;
using LoginServer.Managers;

namespace RPCResponsers
{
    /// <summary>
    /// Login server handle client request
    /// </summary>
    [Handle(typeof(ILoginServerService))]
    public class LoginServerService : Responser, ILoginServerService
    {

        public LoginServerService(Client c) : base(c) { }

        private string DBName { get { return Appliaction.Current["DBName"].AsString(); } } 

        [IgnoreAdmission]
        public L2C_Login Login(C2L_Login request)
        {
            var mongoClient = new MongoClient(Appliaction.Current.ConnectionString);
            var db = mongoClient.GetDatabase(DBName);
            var users = db.GetCollection<PlayInfoEntity>("userinfo");
            var pwd = Md5Tool.GetMd5Hash(request.Password);
            var fiter = Builders<PlayInfoEntity>.Filter.Eq("UserName", request.UserName);
            var query = users.Find(fiter);

            if (!query.Any()) return new L2C_Login { Code = ErrorCode.LoginFailure };
        
            var user = query.First();
            var session = DateTime.UtcNow.Ticks.ToString();
            Appliaction.Current.SetSession(user.Uuid, session);
            user.LastLoginDateTime = DateTime.UtcNow.Ticks;
            user.LoginCount += 1;
            var update = Builders<PlayInfoEntity>
            .Update.Set(u => u.LastLoginDateTime, DateTime.UtcNow.Ticks);
            users.UpdateOne(fiter, update);

            var mapp = ServerManager.Singleton.GetGateServerMappingByServerID(user.ServerId);
            if (mapp == null)  return new L2C_Login { Code = ErrorCode.NofoundServerId };

            if (BattleManager.Singleton.GetBattleServerByUserID(user.Uuid, out UserServerInfo info))
            {
                var server = ServerManager.S.GetBattleServerMappingByServerID(info.BattleServerID);
                if (server != null)
                {
                    Appliaction.Current
                        .GetServerConnectByClientID(server.ClientID)
                        .CreateTask<Task_L2B_ExitUser>(LoginServerTaskServices.S.ExitUser)
                        .Send(() => new Task_L2B_ExitUser { UserID = user.Uuid });
                }
            }
            return new L2C_Login
            {
                Code = ErrorCode.Ok,
                Server = mapp.ServerInfo,
                Session = session,
                UserID = user.Uuid
            };
        }

        [IgnoreAdmission]
        public L2C_Reg Reg(C2L_Reg request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return new L2C_Reg { Code = ErrorCode.RegInputEmptyOrNull };
            
            var mongoClient = new MongoClient(Appliaction.Current.ConnectionString);
            var db = mongoClient.GetDatabase(DBName);
            var users = db.GetCollection<PlayInfoEntity>("userinfo");
            var filter = Builders<PlayInfoEntity>.Filter.Eq("UserName", request.UserName);
            var query = users.Find(filter);
            if (query.Any())
            {
                return new L2C_Reg
                {
                    Code = ErrorCode.RegExistUserName
                };
            }

            var free = ServerManager.S.GetFreeGateServer();
            if (free == null)
            {
                return new L2C_Reg() { Code = ErrorCode.NoFreeGateServer };
            }
            var serverID = free.ServerInfo.ServerID;

            var pwd = Md5Tool.GetMd5Hash(request.Password);
            var acc = new PlayInfoEntity
            {
                Username = request.UserName,
                Password = pwd,
                CreateDateTime = DateTime.UtcNow.Ticks,
                LoginCount = 0,
                LastLoginDateTime = DateTime.UtcNow.Ticks,
                ServerId = serverID
            };

            users.InsertOne(acc);

            var session = DateTime.UtcNow.Ticks.ToString();
            Appliaction.Current.SetSession(acc.Uuid, session);
            var mapping = ServerManager.Singleton
            .GetGateServerMappingByServerID(acc.ServerId);

            if (mapping == null) return new L2C_Reg { Code = ErrorCode.NofoundServerId };
            
            return new L2C_Reg
            {
                Code = ErrorCode.Ok,
                Session = session,
                UserID = acc.Uuid,
                Server = mapping.ServerInfo
            };


        }
    }
}
