using System;
using Proto;
using Proto.LoginServerService;
using Proto.MongoDB;
using ServerUtility;
using XNet.Libs.Net;
using LoginServer;
using MongoDB.Driver;
using LoginServer.Managers;
using MongoTool;

namespace RPCResponsers
{
    /// <summary>
    /// Login server handle client request
    /// </summary>
    [Handle(typeof(ILoginServerService))]
    public class LoginServerService : Responser, ILoginServerService
    {

        public LoginServerService(Client c) : base(c) { }

        [IgnoreAdmission]
        public L2C_Login Login(C2L_Login request)
        {

            var users = DataBase.S.Account;

            var pwd = Md5Tool.GetMd5Hash(request.Password);
            var fiter = Builders<PlayInfoEntity>.Filter.Eq(t=>t.Username, request.UserName);
            var query = users.Find(fiter);

            if (!query.Any()) return new L2C_Login { Code = ErrorCode.LoginFailure };
        
            var user = query.First();
            var s_filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, user.Uuid);
            DataBase.S.Session.DeleteMany(s_filter);

            if (BattleManager.Singleton.GetSessionInfo(user.Uuid, out UserSessionInfoEntity info))
            {
                //had login notify battle process exit
                var server = ServerManager.S.GetBattleServerMappingByServerID(info.BattleServerId);
                if (server != null)
                {
                    Appliaction.Current
                        .GetServerConnectByClientID(server.ClientID)
                        .CreateTask<Task_L2B_ExitUser>(LoginServerTaskServices.S.ExitUser)
                        .Send(() => new Task_L2B_ExitUser { UserID = user.Uuid });
                }
            }

           
            user.LastLoginDateTime = DateTime.UtcNow.Ticks;
            user.LoginCount += 1;
            var update = Builders<PlayInfoEntity>
            .Update.Set(u => u.LastLoginDateTime, DateTime.UtcNow.Ticks);
            users.UpdateOne(fiter, update);

            var mapp = ServerManager.Singleton.GetGateServerMappingByServerID(user.ServerId);
            if (mapp == null)  return new L2C_Login { Code = ErrorCode.NofoundServerId };


            var session = SaveSession(user.Uuid, user.ServerId);

            return new L2C_Login
            {
                Code = ErrorCode.Ok,
                Server = mapp.ServerInfo,
                Session = session,
                UserID = user.Uuid
            };
        }


        private string SaveSession(string uuid,int gateServer)
        {

            var session = Md5Tool.GetMd5Hash(DateTime.UtcNow.Ticks.ToString());

            var us = new UserSessionInfoEntity
            {
                AccountUuid = uuid,
                Token = session,
                MapId = -1,
                BattleServerId = -1,
                GateServerId = gateServer
            };

            DataBase.S.Session.InsertOne(us);
            return session;
        }

        [IgnoreAdmission]
        public L2C_Reg Reg(C2L_Reg request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return new L2C_Reg { Code = ErrorCode.RegInputEmptyOrNull };
            var users = DataBase.S.Account;
            var filter = Builders<PlayInfoEntity>.Filter.Eq(t=>t.Username, request.UserName);
            var query = users.Find(filter);
            if (query.Any())return new L2C_Reg{Code = ErrorCode.RegExistUserName };

            var free = ServerManager.S.GetFreeGateServer();
            if (free == null) return new L2C_Reg() { Code = ErrorCode.NoFreeGateServer };
    
            var serverID = free.ServerInfo.ServerId;
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

            var mapping = ServerManager.Singleton.GetGateServerMappingByServerID(acc.ServerId);
            if (mapping == null) return new L2C_Reg { Code = ErrorCode.NofoundServerId };

            var session = SaveSession(acc.Uuid, acc.ServerId);
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
