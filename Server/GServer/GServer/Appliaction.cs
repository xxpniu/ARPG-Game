using System;
using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;
using ServerUtility;
using System.Linq;
using org.vxwo.csharp.json;
using GateServer;
using Proto.LoginServerService;
using Proto.LoginBattleGameServerService;
using System.Threading.Tasks;

namespace GServer
{
    public class Appliaction
    {
        public Appliaction(JsonValue config)
        {

            this.configRoot = config["ConfigPath"].AsString();
            this.port = config["ListenPort"].AsInt();
            this.ServicePort = config["ServicePort"].AsInt();
            this.ServiceHost = config["ServiceHost"].AsString();
            this.LoginPort = config["LoginServerPort"].AsInt();
            this.LoginHost = config["LoginServerHost"].AsString();
            this.EnableGM = config["EnableGM"].AsBoolean();
            serverHostName = config["Host"].AsString();
            Current = this;
            this.ConnectionString = config["DBHost"].AsString();
            this.DbName = config["DBName"].AsString();
            NetProtoTool.EnableLog = config["Log"].AsBoolean();
            ServerID = config["ServerID"].AsInt();
            MonitorPool.Singleton.Init(this.GetType().Assembly);
        }

        #region 属性
        readonly int port;
        readonly int ServicePort;
        readonly string ServiceHost;
        readonly string LoginHost;
        readonly int LoginPort;
        readonly string configRoot;
        readonly string serverHostName;
        public bool EnableGM { get; set; }
        public int ServerID { set; get; }

        public static Appliaction Current { private set; get; }

        //玩家访问端口
        public SocketServer ListenServer { private set; get; }

        //游戏战斗服务器访问端口
        public SocketServer ServiceServer { private set; get; }

        public RequestClient<TaskHandler> Client { private set; get; }

        public volatile bool IsRunning;
        #endregion

        #region 数据库操作
        public readonly string ConnectionString;

        public readonly string DbName;
        #endregion

        #region 管理客户端连接
        public Client GetClientById(int index)
        {
            return ListenServer.CurrentConnectionManager.GetClientByID(index);
        }

        internal Client GetClientByUserID(string userID)
        {
            Client res = null;
            this.ListenServer.CurrentConnectionManager.Each((obj) =>
            {
                if ((string)obj.UserState == userID)
                {
                    res = obj;
                    return true;
                }
                return false;
            });
            return res;
        }
        #endregion

        #region Appliaction 
        public void Start()
        {
            if (IsRunning) return;
            MonitorPool.Singleton.Start();
            ResourcesLoader.Singleton.LoadAllConfig(this.configRoot);
            IsRunning = true;
            //同时对外对内服务器不能使用全部注册
            var listenHandler = new RequestHandle<GateBattleServerService>();
            //2 对外
            ListenServer = new SocketServer(new ConnectionManager(), port)
            {
                HandlerManager = listenHandler
            };
            ListenServer.Start();


            var serviceHandler = new RequestHandle<GateBattleServerService>();
            ServiceServer = new SocketServer(new ConnectionManager(), ServicePort)
            {
                HandlerManager = serviceHandler
            };
            ServiceServer.Start();


            Client = new RequestClient<TaskHandler>(LoginHost, LoginPort)
            {
                UseSendThreadUpdate = true
            };
            Client.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    int currentPlayer = 0;
                    var result = RegServer.CreateQuery()
                    .SendRequest(Client, new G2L_Reg
                    {
                        CurrentPlayer = currentPlayer,
                        MaxPlayer = 10000,
                        Host = serverHostName,
                        Port = ServicePort,
                        Version = 1,
                        ServiceHost = ServiceHost
                    });

                    if (result.Code == ErrorCode.Ok)
                    {
                        Debuger.Log("Server Reg Success!");
                    }
                }
                else
                {
                    Debuger.Log("Can't connect LoginServer!");
                    Stop();
                }
            };
            Client.OnDisconnect = (s, e) =>
            {
                Debuger.Log("disconnet from LoginServer!");
                Stop();
            };

            Client.Connect();
            
        }

        public void Stop()
        {
            if (!IsRunning)  return;
            MonitorPool.Singleton.Exit();
            IsRunning = false;
            ListenServer.Stop();
            ServiceServer.Stop();
            Client.Disconnect();
        }

        public void Tick()
        {
            MonitorPool.Singleton.Tick();
        }
        #endregion
 
    }
}

