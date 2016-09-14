#define USEGM 
using System;
using GServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;


namespace GServer.Responsers
{
    [HandleType(typeof(C2G_GMTool),HandleResponserType.CLIENT_SERVER)]
    public class C2G_GMToolResponser : Responser<C2G_GMTool, G2C_GMTool>
    {
        public C2G_GMToolResponser()
        {
            NeedAccess = true;
        }

        public override G2C_GMTool DoResponse(C2G_GMTool request, Client client)
        {

#if USEGM
            var args = request.GMCommand.Split(' ');
            if (args.Length == 0) return new G2C_GMTool { Code = ErrorCode.Error };
            var userid = (long)client.UserState;
            UserData userData;
            if (!MonitorPool.S.Get<Managers.UserDataManager>().TryToGetUserData(userid, out userData))
            {
                return new G2C_GMTool { Code = ErrorCode.NoGamePlayerData };
            }
            switch (args[0].ToLower())
            {
                case "level":
                    {
                        var level = 0;
                        if (int.TryParse(args[1], out level))
                        {
                            userData.HeroLevelTo(level);
                        }
                    }
                    break;
                case "make":
                    {
                        int id = int.Parse(args[1]);
                        var num = 1;
                        if (args.Length > 2)
                            num = int.Parse(args[2]);

                        var playerItem = new PlayerItem { ItemID = id, Num = num };
                        userData.AddItem(playerItem);
                    }
                    break;
                case "addgold":
                    {
                        userData.AddGold(int.Parse(args[1]));
                    }
                    break;
                case "addcoin":
                    {
                        userData.AddCoin(int.Parse(args[1]));
                    }
                    break;
            }

            //sync
            var syncHero = new Task_G2C_SyncHero { Hero = userData.GetHero() };
            var syncPackage = new Task_G2C_SyncPackage
            {
                Package = userData.GetPackage(),
                Gold = userData.Gold,
                Coin = userData.Coin
            };
            NetProtoTool.SendTask(client, syncHero);
            NetProtoTool.SendTask(client, syncPackage);

            return new G2C_GMTool
            {
                Code = ErrorCode.OK
            };
#else
            return new G2C_GMTool { Code = ErrorCode.Error };
#endif
        }
    }
}

