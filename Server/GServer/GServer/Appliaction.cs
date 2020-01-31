using System;
using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;
using ServerUtility;
using System.Linq;
using org.vxwo.csharp.json;
using GServer.Responsers;

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
            this.ConnectionString = string.Format(
                config["DBHost"].AsString(),
                config["DBUser"].AsString(),
                config["DBPwd"].AsString()
            );
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

        public RequestClient Client { private set; get; }

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
            var listenHandler = new RequestHandle();
            //2 对外
            listenHandler.RegAssembly(this.GetType().Assembly, HandleResponserType.CLIENT_SERVER);
            ListenServer = new SocketServer(new ConnectionManager(), port);
            ListenServer.HandlerManager = listenHandler;
            ListenServer.Start();

            var serviceHandler = new RequestHandle();
            serviceHandler.RegAssembly(this.GetType().Assembly,HandleResponserType.SERVER_SERVER);
            ServiceServer = new SocketServer(new ConnectionManager(), ServicePort);
            ServiceServer.HandlerManager = serviceHandler;
            ServiceServer.Start();


            Client = new RequestClient(LoginHost, LoginPort);
            Client.RegTaskHandlerFromAssembly(this.GetType().Assembly);
            Client.UseSendThreadUpdate = true;
            Client.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    int currentPlayer = 0;
                    using (var db = new DataBaseContext.GameDb(Connection))
                    {
                        currentPlayer = db.TBGAmePlayer.Count();
                    }
                    var request = Client.CreateRequest<G2L_Reg, L2G_Reg>();
                    request.RequestMessage.ServerID = ServerID;
                    request.RequestMessage.Port = this.port;
                    request.RequestMessage.Host = serverHostName;
                    request.RequestMessage.ServiceHost = ServiceHost;
                    request.RequestMessage.ServicesProt = ServicePort;
                    request.RequestMessage.MaxPlayer = 100000; //最大玩家数
                    request.RequestMessage.CurrentPlayer = currentPlayer;
                    request.RequestMessage.Version = ProtoTool.GetVersion();
                    request.OnCompleted = (success, r) =>
                    {
                        if (success && r.Code == ErrorCode.OK)
                        {
                            Debuger.Log("Server Reg Success!");
                        }

                    };
                    request.SendRequestSync();
                }
                else
                {
                    Debuger.Log("Can't connect LoginServer!");
                    Stop();

                }
            };
            Client.OnDisconnect = (s, e) => 
            {
                Debuger.Log("Can't connect LoginServer!");
                Stop();
            };
            Client.Connect();

        }

        public void Stop()
        {
            if (!IsRunning) 
                return;
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

