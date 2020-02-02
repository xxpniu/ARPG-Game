using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoTool;
using org.vxwo.csharp.json;
using Proto;
using Proto.MongoDB;
using RPCResponsers;
using ServerUtility;
using XNet.Libs.Utility;

namespace LoginServer.Managers
{
    [Monitor]
    public class BattleManager : XSingleton<BattleManager>, IMonitor
    {
        public BattleManager()
        {
            
            
        }
        //玩家退出战斗
        public void ExitBattle(string account_uuid,int serverID)
        {
            var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, account_uuid);
            var update = Builders<UserSessionInfoEntity>.Update.Set(t => t.BattleServerId, -1);
            DataBase.S.Session.UpdateOne(filter, update);
        }

        //server close 
        public void ServerClose(int serverID)
        {
            var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.BattleServerId, serverID);
            //var update = Builders<UserSessionInfoEntity>.Update.Set(t => t.BattleServerId, -1);
            DataBase.S.Session.DeleteMany(filter);
        }

        public bool GetSessionInfo(string userID,out UserSessionInfoEntity serverInfo)
        {
            var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, userID);
            serverInfo = DataBase.S.Session.Find(filter).SingleOrDefault();
            return serverInfo != null;
        }

        //开始进入战斗
        internal ErrorCode BeginBattle(
            string accountUuid,
            int mapID, int serverID, out GameServerInfo serverInfo)
        {
            serverInfo = null;
            var battleServer = ServerManager.Singleton.GetFreeBattleServerID();
            if (battleServer == null) return ErrorCode.NofreeBattleServer;

            if (GetSessionInfo(accountUuid, out UserSessionInfoEntity session))
            {
                if (session.BattleServerId > 0)
                {
                    var task = new Task_L2B_ExitUser { UserID = session.AccountUuid };
                    var b = ServerManager.Singleton.GetBattleServerMappingByServerID(session.BattleServerId);
                    Appliaction.Current.GetServerConnectByClientID(b.ClientID)?
                        .CreateTask<Task_L2B_ExitUser>(LoginServerTaskServices.S.ExitUser)
                        .Send(() => task);
                    return ErrorCode.PlayerIsInBattle;
                }
                else
                {
                    serverInfo = battleServer.ServerInfo;
                    var gateserver = ServerManager.Singleton.GetGateServerMappingByServerID(serverID);
                    var task = new Task_L2B_StartBattle
                    {
                        MapID = mapID
                    };
                    task.Users.Add(new PlayerServerInfo
                    {
                        AccountUuid = accountUuid,
                        ServerID = serverID,
                        ServiceHost = gateserver.ServiceHost,
                        ServicePort = gateserver.ServicePort
                    });

                    if (Appliaction.Current
                        .GetServerConnectByClientID(battleServer.ClientID)?
                        .CreateTask<Task_L2B_StartBattle>(LoginServerTaskServices.S.StartBattle)
                        .Send(() => task) ?? false)
                    {
                        return ErrorCode.BattleServerHasDisconnect;
                    }

                    var filter = Builders<UserSessionInfoEntity>.Filter.Eq(t => t.AccountUuid, session.AccountUuid);
                    var update = Builders<UserSessionInfoEntity>.Update.Set(t => t.BattleServerId, serverID);
                    DataBase.S.Session.UpdateOne(filter, update);
                    return ErrorCode.Ok;
                }
            }
            else {
                return ErrorCode.Error;
            }
        }

        public void OnTick()
        {
            
        }

        public void OnShowState()
        {

        }

        public void OnExit()
        {

        }

        public void OnStart()
        {

        }
    }
}

