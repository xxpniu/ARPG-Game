using System;
using LoginServer;
using LoginServer.Managers;
using MongoDB.Driver;
using MongoTool;
using Proto;
using Proto.LoginBattleGameServerService;
using Proto.MongoDB;
using ServerUtility;
using XNet.Libs.Net;

namespace RPCResponsers
{
    /// <summary>
    /// Login server Handle Server API
    /// </summary>
    [Handle(typeof(ILoginBattleGameServerService))]
    public class LoginBattleGameServerService : Responser, ILoginBattleGameServerService
    {
        public LoginBattleGameServerService(Client c) : base(c) { }

        public L2G_BeginBattle BeginBattle(G2L_BeginBattle req)
        {
            //已经在战场中
            var res = BattleManager.Singleton.BeginBattle(req.UserID, (int)Client.UserState, out GameServerInfo battleServer);
            if (res == ErrorCode.Ok)
            {
                return new L2G_BeginBattle { Code = ErrorCode.Ok, BattleServer = battleServer };
            }
            else
            {
                return new L2G_BeginBattle { Code = res };
            }
        } 

        public L2G_GetLastBattle GetLastBattle(G2L_GetLastBattle req)
        {
            if (BattleManager.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity userSInfo))
            {
                var battleServer = ServerManager.S
                    .GetServerMappingByServerIDWithType(userSInfo.BattleServerId, ServerType.StBattle);
                if (battleServer == null)
                {
                    return new L2G_GetLastBattle { Code = ErrorCode.BattleServerHasDisconnect };
                }
                else
                {
                    return new L2G_GetLastBattle
                    {
                        BattleServer = battleServer.ServerInfo,
                        MapID = userSInfo.MapId,
                        Code = ErrorCode.Ok
                    };
                }
            }
            else
            {
                return new L2G_GetLastBattle { Code = ErrorCode.NofoundUserBattleServer };
            }
        }

        [IgnoreAdmission]
        public L2G_GateServerReg RegGateServer(G2L_GateServerReg req)
        {
            var client = this.Client;
            client.HaveAdmission = true;
            client.UserState = req.ServerID;
            var server = new GameServerInfo
            {
                ServerId = req.ServerID,
                Host = req.Host,
                Port = req.Port,
                CurrentPlayerCount = req.CurrentPlayer,
                MaxPlayerCount = req.MaxPlayer,
                ServicesHost = req.ServiceHost,
                ServicesPort = req.ServicesProt
            };


            var success = ServerManager.S.AddServerByType(client.ID, server, ServerType.StGate);
            if (success == null)
            {
                return new L2G_GateServerReg { Code = ErrorCode.Error };
            }

            client.OnDisconnect += OnDisconnect;
            return new L2G_GateServerReg { Code = ErrorCode.Ok };
        }

        private static void OnDisconnect(Client client)
        {
            ServerManager.S.RemoveServerByType((int)client.UserState, ServerType.StGate);
        }

        public L2B_CheckSession CheckSession(B2L_CheckSession req)
        {
            if (BattleManager.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity session))
            {
                if (req.SessionKey != session.Token) return new L2B_CheckSession { Code = ErrorCode.Error };

                var filter = Builders<GameServerInfoEntity>.Filter.Eq(t => t.ServerId, session.GateServerId);

                var gate = DataBase.S.Servers.Find(filter).SingleOrDefault();

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
            BattleManager.S.ExitBattle(req.UserID);
            return new L2B_EndBattle { Code = ErrorCode.Ok };
        }

        [IgnoreAdmission]
        public L2B_RegBattleServer RegBattleServer(B2L_RegBattleServer req)
        {

            var server = new GameServerInfo
            {
                Host = req.ServiceHost,
                Port = req.ServicePort,
                ServerId = -1,
                MaxPlayerCount = req.MaxBattleCount,
                ServicesHost = req.ServiceHost,
                ServicesPort = req.ServicePort,
                CurrentPlayerCount = 0
            };

            var serverIndex = ServerManager.S.AddServerByType(Client.ID, server, ServerType.StBattle);
            Client.UserState = serverIndex;
            Client.HaveAdmission = true;
            Client.OnDisconnect += UnRegWhenDisconnect;
            return new L2B_RegBattleServer { Code = ErrorCode.Ok };
        }

        private static void UnRegWhenDisconnect(Client client)
        {
            var serverID = (int)client.UserState;
            BattleManager.Singleton.ServerClose(serverID);
            ServerManager.Singleton.RemoveServerByType(serverID, ServerType.StBattle);
        }

        public L2G_GateCheckSession GateServerSession(G2L_GateCheckSession req)
        {
            BattleManager.S.GetSessionInfo(req.UserID, out UserSessionInfoEntity info);
            return new L2G_GateCheckSession
            {
                Code = info?.Token == req.Session ? ErrorCode.Ok : ErrorCode.Error
            };
        }
    }
}
