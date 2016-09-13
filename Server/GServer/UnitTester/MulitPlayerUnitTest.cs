using System;
using System.Threading;
using Proto;
using ServerUtility;
using XNet.Libs.Utility;

namespace UnitTester
{
    public class MulitPlayerUnitTest
    {
        public MulitPlayerUnitTest(string userName, string pwd, string host, int port)
        {
            this.userName = userName;
            this.pwd = pwd;
            this.host = host;
            this.port = port;
        }
        private string host;
        private int port;
        private string userName;
        public string pwd;

        private GameServerInfo GateServer;
        private long userID;
        private string sessionKey;
        private GameServerInfo BattleServer;

        public void Begin()
        {
            Thread r = new Thread(Work);
            r.Start();
        }

        private ManualResetEvent MEvent = new ManualResetEvent(false);

        private void Work()
        {
            MEvent.Reset();
            Login();
            MEvent.WaitOne();
            MEvent.Reset();
            Gate();
            MEvent.WaitOne();
            BeginGame();
        }

        private RequestClient client;

        public void BeginGame()
        {
            client = new ServerUtility.RequestClient(BattleServer.Host, BattleServer.Port);
            client.UseSendThreadUpdate = true;

            client.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    var login = client.CreateRequest<C2B_JoinBattle, B2C_JoinBattle>();
                    login.RequestMessage.MapID = 1;
                    login.RequestMessage.Session = sessionKey;
                    login.RequestMessage.UserID = userID;
                    login.RequestMessage.Version = ProtoTool.GetVersion();
                    login.SendRequestSync();
                }
            };
            client.Connect();
        }
    
        private RequestClient gclient;
        private void Gate()
        {
            gclient = new ServerUtility.RequestClient(GateServer.Host, GateServer.Port);
            gclient.UseSendThreadUpdate = true;
           
            gclient.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    var login = gclient.CreateRequest<C2G_Login, G2C_Login>();
                    login.RequestMessage.Session = sessionKey;
                    login.RequestMessage.UserID = userID;
                    login.RequestMessage.Version = ProtoTool.GetVersion();
                    login.OnCompleted = (t1, t2) =>
                    {
                        Debuger.Log(t2);

                    };
                    login.SendRequestSync();

                    var beginBattle = gclient.CreateRequest<C2G_BeginGame, G2C_BeginGame>();
                    beginBattle.RequestMessage.MapID = 1;
                    beginBattle.OnCompleted = (se, res) =>
                    {
                        if (res.Code == ErrorCode.OK)
                        {
                            BattleServer = res.ServerInfo;
                            gclient.Disconnect();
                            MEvent.Set();
                        }
                    };
                    beginBattle.SendRequestSync();
                }
            };

            gclient.Connect();
        }

        private RequestClient lclient;

        private void Login()
        {
            lclient = new ServerUtility.RequestClient(host, port);
            lclient.UseSendThreadUpdate = true;

            lclient.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    
                    var login = lclient.CreateRequest<C2L_Login, L2C_Login>();
                    login.RequestMessage.Password = pwd;
                    login.RequestMessage.UserName = userName;
                    login.RequestMessage.Version = ProtoTool.GetVersion();
                    login.OnCompleted = (su, res) =>
                    {
                        if (res.Code == ErrorCode.OK)
                        {
                            GateServer = res.Server;
                            userID = res.UserID;
                            sessionKey = res.Session;
                            lclient.Disconnect();
                            MEvent.Set();
                        }
                    };
                    login.SendRequestSync();
                }
            };

            lclient.Connect();
        }
    }
}

