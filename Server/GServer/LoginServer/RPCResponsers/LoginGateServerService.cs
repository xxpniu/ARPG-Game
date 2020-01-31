using System;
using LoginServer;
using LoginServer.Managers;
using Proto;
using Proto.LoginBattleGameServerService;
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
            var res = BattleManager.Singleton.BeginBattle(
                req.UserID,
                req.MapID,
                (int)Client.UserState,
                out GameServerInfo battleServer);
            if (res == ErrorCode.Ok)
            {
                return new L2G_BeginBattle { Code = ErrorCode.Ok, BattleServer = battleServer };
            }
            else
            {
                return new L2G_BeginBattle { Code = res };
            }
        }

        public L2G_CheckUserSession CheckUserSession(G2L_CheckUserSession req)
        {
            return new L2G_CheckUserSession
            {
                Code = Appliaction.Current.GetSession(req.UserID) == req.Session ?
                ErrorCode.Ok : ErrorCode.Error
            };
        }

        public L2G_GetLastBattle GetLastBattle(G2L_GetLastBattle req)
        {
            if (BattleManager.Singleton.GetBattleServerByUserID(req.UserID, out UserServerInfo userSInfo))
            {
                var battleServer = ServerManager.Singleton.GetBattleServerMappingByServerID(userSInfo.BattleServerID);

                if (battleServer == null)
                {
                    return new L2G_GetLastBattle { Code = ErrorCode.BattleServerHasDisconnect };
                }
                else
                {
                    return new L2G_GetLastBattle
                    {
                        BattleServer = battleServer.ServerInfo,
                        MapID = userSInfo.MapID,
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
        public L2G_Reg RegServer(G2L_Reg req)
        {
            var client = this.Client;
            
            client.HaveAdmission = true;
            client.UserState = req.ServerID;
            var server = new GameServerInfo
            {
                ServerID = req.ServerID,
                Host = req.Host,
                Port = req.Port
            };
            var success = ServerManager
                .S.AddGateServer(
                    client.ID,
                    req.CurrentPlayer,
                    server,
                    req.ServiceHost,
                    req.ServicesProt
                );

            if (!success)
            {
                return new L2G_Reg { Code = ErrorCode.Error };
            }

            client.OnDisconnect += OnDisconnect;
            return new L2G_Reg { Code = ErrorCode.Ok };
        }

        private static void OnDisconnect(object sender, EventArgs e)
        {
            var client = sender as Client;
            ServerManager.Singleton.RemoveGateServer((int)client.UserState);
        }

        public L2B_CheckSession CheckSession(B2L_CheckSession req)
        {
            if (Appliaction.Current.GetSession(req.UserID) == req.SessionKey)
            {
                return new L2B_CheckSession { Code = ErrorCode.Ok };
            }
            else
            {
                return new L2B_CheckSession { Code = ErrorCode.Error };
            }
        }

        public L2B_EndBattle EndBattle(B2L_EndBattle req)
        {
            var serverID = (int)Client.UserState;
            BattleManager.Singleton.ExitBattle(req.UserID, serverID);
            return new L2B_EndBattle { Code = ErrorCode.Ok };
        }

        public L2B_RegBattleServer RegBattleServer(B2L_RegBattleServer req)
        {

            var server = new GameServerInfo
            {
                Host = req.ServiceHost,
                Port = req.ServicePort,
                ServerID = -1,
                MaxServiceCount = req.MaxBattleCount
            };

            var serverIndex = ServerManager.Singleton.AddBattleServer(Client.ID, server);
            server.ServerID = serverIndex;
            Client.UserState = serverIndex;
            Client.HaveAdmission = true;
            Client.OnDisconnect += UnRegWhenDisconnect;
            return new L2B_RegBattleServer { Code = ErrorCode.Ok, ServiceServerID = serverIndex };
        }

        private static void UnRegWhenDisconnect(object sender, EventArgs e)
        {
            var client = sender as Client;
            var serverID = (int)client.UserState;
            BattleManager.Singleton.ServerClose(serverID);
            ServerManager.Singleton.RemoveBattleServer(serverID);
        }
    }
}
