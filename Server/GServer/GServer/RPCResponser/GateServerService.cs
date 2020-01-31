#define USEGM

using System;
using GServer;
using GServer.Managers;
using Proto;
using Proto.GateServerService;
using Proto.LoginBattleGameServerService;
using ServerUtility;
using XNet.Libs.Net;

namespace GateServer
{
    [Handle(typeof(IGateServerService))]
    public class GateServerService: Responser, IGateServerService
    {
        public GateServerService(Client c) : base(c) { }

        public string PlayerUuid { get { return (string)Client.UserState; } }

        public G2C_BeginGame BeginGame(C2G_BeginGame request)
        {
            var userID = (string)Client.UserState;
            var req = new G2L_BeginBattle
            {
                MapID = request.MapID,
                UserID = userID
            };
            var r = BeginBattle.CreateQuery()
                .SendRequestAsync(Appliaction.Current.Client, req).GetAwaiter().GetResult();

            return new G2C_BeginGame
            {
                Code = r.Code, //  ErrorCode.Error
                ServerInfo = r.BattleServer
            };
        }

        public G2C_CreateHero CreateHero(C2G_CreateHero request)
        {
            var manager = MonitorPool.S.Get<UserDataManager>();
            var r = manager.TryToCreateUser(PlayerUuid, request.HeroID, request.HeroName)
                .GetAwaiter().GetResult();
            return new G2C_CreateHero { Code = r ? ErrorCode.Ok : ErrorCode.Error };
        }

        public G2C_EquipmentLevelUp EquipmentLevelUp(C2G_EquipmentLevelUp request)
        {
            return MonitorPool.S.Get<UserDataManager>()
                .EquipLevel(PlayerUuid, request.Guid, request.Level)
                .GetAwaiter()
                .GetResult();
        }

        public G2C_GetLastBattle GetLastBattle(C2G_GetLastBattle request)
        {
            var response = new G2C_GetLastBattle { Code = ErrorCode.Error };
            var req = Proto.LoginBattleGameServerService.GetLastBattle.CreateQuery()
                .SendRequestAsync(Appliaction.Current.Client, new G2L_GetLastBattle { UserID = request.AccountUuid })
                .GetAwaiter().GetResult();

            if (req.Code == ErrorCode.Ok)
            {
                response.BattleServer = req.BattleServer;
                response.MapID = req.MapID;
            }

            return response;
        }

        public G2C_GMTool GMTool(C2G_GMTool request)
        {
#if USEGM
            if (!Appliaction.Current.EnableGM) return new G2C_GMTool() { Code = ErrorCode.Error };

            var args = request.GMCommand.Split(' ');
            if (args.Length == 0) return new G2C_GMTool { Code = ErrorCode.Error };
            
            switch (args[0].ToLower())
            {
                case "level":
                    {
                        if (int.TryParse(args[1], out int level))
                        {
                           // userData.HeroLevelTo(level);
                        }
                    }
                    break;
                case "make":
                    {
                        int id = int.Parse(args[1]);
                        var num = 1;
                        if (args.Length > 2) num = int.Parse(args[2]);
                    }
                    break;
                case "addgold":
                    {
                        //userData.AddGold(int.Parse(args[1]));
                    }
                    break;
                case "addcoin":
                    {
                        //userData.AddCoin(int.Parse(args[1]));
                    }
                    break;
                case "addexp":
                    {
                        int level = 0;
                        //userData.AddExp(int.Parse(args[1]), out level);
                    }
                    break;
            }

            //sync
            //var syncHero = new Task_G2C_SyncHero { Hero = userData.GetHero() };
            //var syncPackage = new Task_G2C_SyncPackage { };
            //NetProtoTool.SendTask(client, syncHero);
            //NetProtoTool.SendTask(client, syncPackage);

            return new G2C_GMTool
            {
                Code = ErrorCode.Ok
            };
#else
            return new G2C_GMTool { Code = ErrorCode.Error };
#endif
        }

        [IgnoreAdmission]
        public G2C_Login Login(C2G_Login request)
        { 
            if (string.IsNullOrWhiteSpace(request.Session)) return new G2C_Login { Code = ErrorCode.Error };

            var req = CheckUserSession.CreateQuery()
                .SendRequestAsync(Appliaction.Current.Client,
                new G2L_CheckUserSession { Session= request.Session, UserID= request.UserID })
                .GetAwaiter().GetResult()
                ;

            if (req.Code == ErrorCode.Ok)
            {
                var clients = Appliaction.Current.ListenServer.CurrentConnectionManager.AllConnections;
                foreach (var i in clients)
                {
                    if (i.UserState != null
                        && (string)i.UserState == request.UserID)
                    {
                        i.Close();
                    }
                }
                Client.HaveAdmission = true;
                Client.UserState = request.UserID;
            }

            if (Client.HaveAdmission)
            {
                var manager = MonitorPool.S.Get<UserDataManager>();
                var have = manager.HavePlayer(request.UserID).GetAwaiter().GetResult();
                if (have) manager.SyncToClient(Client).GetAwaiter().GetResult();
                return new G2C_Login { Code = ErrorCode.Ok, HavePlayer = have };
            }
            else
            {
                return new G2C_Login { Code = req.Code };
            }
        }

        public G2C_OperatorEquip OperatorEquip(C2G_OperatorEquip request)
        {
            var manager = MonitorPool.S.Get<UserDataManager>();
            if (!manager.HavePlayer(PlayerUuid).GetAwaiter().GetResult())
                return new G2C_OperatorEquip { Code = ErrorCode.NoGamePlayerData };

            bool result = manager.OperatorEquip(PlayerUuid, request.Guid, request.Part, request.IsWear)
                .GetAwaiter().GetResult();
            if (result)
                manager.SyncToClient(Client).GetAwaiter().GetResult();
            return new G2C_OperatorEquip
            {
                Code = !result ? ErrorCode.Error : ErrorCode.Ok,
                // Hero = 
            };
        }

        public G2C_SaleItem SaleItem(C2G_SaleItem req)
        {
            return MonitorPool.S.Get<UserDataManager>().SaleItem(PlayerUuid, req.Items)
                .GetAwaiter().GetResult();
        }
    }
}
