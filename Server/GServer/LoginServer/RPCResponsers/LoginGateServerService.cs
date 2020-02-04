using System;
using LoginServer;
using MongoDB.Driver;
using MongoTool;
using Proto;
using Proto.LoginBattleGameServerService;
using Proto.MongoDB;
using XNet.Libs.Net;
using XNet.Libs.Utility;

namespace RPCResponsers
{
    /// <summary>
    /// Login server Handle Server API
    /// </summary>
    [Handle(typeof(ILoginBattleGameServerService))]
    public class LoginBattleGameServerService : Responser, ILoginBattleGameServerService
    { 
        private static volatile int BattleServerIndex = 1;

        public LoginBattleGameServerService(Client c) : base(c) { }

        public L2G_BeginBattle BeginBattle(G2L_BeginBattle req)
        {
            if (DataBase.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity session))
            {
                if (session.BattleServerId > 0)
                {
                    //踢当前用户下线
                    var task = new Task_L2B_ExitUser { UserID = session.AccountUuid };
                    var b = DataBase.S.GetBattleServerByServerID(session.BattleServerId);
                    if (b != null) Appliaction.Current.GetServerConnectByClientID(b.ClientId)?.CreateTask(task).Send();
                    //return new L2G_BeginBattle { Code = ErrorCode.PlayerIsInBattle };
                }

                var free_filter = Builders<PlayerBattleServerEntity>.Filter.Eq(t => t.LevelId, req.LevelId);
                var battle = DataBase.S.BattleServer.Find(free_filter).FirstOrDefault();
                if (battle == null) return new L2G_BeginBattle { Code = ErrorCode.NofoundUserBattleServer };

                var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, session.AccountUuid);
                var update = Builders<UserSessionInfoEntity>.Update.Set(t => t.BattleServerId, battle.ServerId);

                DataBase.S.Session.UpdateOne(filter, update);
                return new L2G_BeginBattle
                {
                    BattleServer = new GameServerInfo
                    {
                        CurrentPlayerCount = 0,
                        Host = battle.Host,
                        Port = battle.Port,
                        MaxPlayerCount = battle.MaxPlayerCount,
                        ServerId = battle.ServerId
                    },
                    Code = ErrorCode.Ok
                };

            }
            return new L2G_BeginBattle { Code = ErrorCode.Error };
        }

        public L2G_GetLastBattle GetLastBattle(G2L_GetLastBattle req)
        {
            if (DataBase.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity userSInfo))
            {
                if (userSInfo.BattleServerId > 0)
                {
                    var filter = Builders<PlayerBattleServerEntity>
                        .Filter.Eq(t => t.ServerId, userSInfo.BattleServerId);
                    var battleServer = DataBase.S.BattleServer.Find(filter).SingleOrDefault();
                    if (battleServer == null)
                    {
                        return new L2G_GetLastBattle { Code = ErrorCode.BattleServerHasDisconnect };
                    }
                    else
                    {
                        return new L2G_GetLastBattle
                        {
                            BattleServer = new GameServerInfo
                            {
                                CurrentPlayerCount = 0,
                                Host = battleServer.Host,
                                Port = battleServer.Port,
                                MaxPlayerCount = battleServer.MaxPlayerCount,
                                ServerId = battleServer.ServerId
                            },
                            LevelId = battleServer.LevelId,
                            Code = ErrorCode.Ok
                        };
                    }
                }
            }

            return new L2G_GetLastBattle { Code = ErrorCode.NofoundUserBattleServer };

        }

        [IgnoreAdmission]
        public L2G_GateServerReg RegGateServer(G2L_GateServerReg req)
        {
            
            Client.HaveAdmission = true;
            Client.UserState = req.ServerID;
            var server = new GateServerInfoEntity
            {
                ServerId = req.ServerID,
                Host = req.Host,
                Port = req.Port,
                CurrentPlayerCount = req.CurrentPlayer,
                MaxPlayerCount = req.MaxPlayer,
                ServiceHost = req.ServiceHost,
                ServicePort = req.ServicesProt,
                ClientId = Client.ID
            };

            DataBase.S.GateServer.InsertOne(server);
            Client.OnDisconnect += OnDisconnect;
            return new L2G_GateServerReg { Code = ErrorCode.Ok };
        }

        private static void OnDisconnect(Client client)
        {
            var filter = Builders<GateServerInfoEntity>.Filter.Eq(t => t.ClientId, client.ID);
            DataBase.S.GateServer.DeleteOne(filter);
        }

        public L2B_CheckSession CheckSession(B2L_CheckSession req)
        {
            if (DataBase.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity session))
            {
                if (req.SessionKey != session.Token) return new L2B_CheckSession { Code = ErrorCode.Error };

                var filter = Builders<GateServerInfoEntity>.Filter.Eq(t => t.ServerId, session.GateServerId);
                var gate = DataBase.S.GateServer.Find(filter).SingleOrDefault();
                if (gate == null) return new L2B_CheckSession { Code = ErrorCode.Error };
                return new L2B_CheckSession
                {
                    Code = session?.Token == req.SessionKey ? ErrorCode.Ok : ErrorCode.Error,
                    GateServer = DataBase.S.ToServerInfo(gate)
                };
            }
            return new L2B_CheckSession { Code = ErrorCode.Error };
        }

        public L2B_EndBattle EndBattle(B2L_EndBattle req)
        {
            var serverID = (int)Client.UserState;
            var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, req.UserID);
            var update = Builders<UserSessionInfoEntity>.Update.Set(t => t.BattleServerId, -1);
            DataBase.S.Session.UpdateOne(filter, update);
            return new L2B_EndBattle { Code = ErrorCode.Ok };
        }

        [IgnoreAdmission]
        public L2B_RegBattleServer RegBattleServer(B2L_RegBattleServer req)
        {
            int id = BattleServerIndex++;
            if (BattleServerIndex == int.MaxValue) BattleServerIndex = 0;

            var server = new PlayerBattleServerEntity
            {
                Host = req.Host,
                Port = req.Port,
                ServerId = id,
                MaxPlayerCount = req.Maxplayer,
                ClientId = Client.ID,
                JoinTime = DateTime.UtcNow.Ticks,
                LevelId = req.LevelId,
            };

            DataBase.S.BattleServer.InsertOne(server);
            Client.UserState = id;
            Client.HaveAdmission = true;
            Client.OnDisconnect += UnRegWhenDisconnect;
            return new L2B_RegBattleServer { Code = ErrorCode.Ok , ServiceServerID =BattleServerIndex};
        }

        private static void UnRegWhenDisconnect(Client client)
        {
            var clientId = client.ID;
            var filter = Builders<PlayerBattleServerEntity>.Filter.Eq(t => t.ClientId, clientId);
            DataBase.S.BattleServer.DeleteOne(filter);
        }

        public L2G_GateCheckSession GateServerSession(G2L_GateCheckSession req)
        {
            DataBase.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity info);
            return new L2G_GateCheckSession
            {
                Code = info?.Token == req.Session ? ErrorCode.Ok : ErrorCode.Error
            };
        }
    }
}
