using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;
using ServerUtility;
using org.vxwo.csharp.json;
using RPCResponsers;
using RPCTaskHandlers;
using Proto.LoginBattleGameServerService;

namespace MapServer
{
    public class Appliaction
    {
        public const int SERVER_TICK = 33;//游戏战斗仿真刷新间隔时间


        readonly int port;
        readonly int ServicePort;
        readonly string ServiceHost;
        readonly string configRoot;
        readonly string ServerHost;
        public int ServerID { set; get; }



        public static Appliaction Current { private set; get; }

        //服务器（客户端）
        public SocketServer ListenServer { private set; get; }

        /// <summary>
        /// center server /login server
        /// </summary>
        public RequestClient<LoginServerTaskServicesTaskHandler> Client { private set; get; }

        public volatile bool IsRunning;

        private readonly int MaxBattleCount;

        public SyncDictionary<int, RequestClient<TaskHandler>> GateServerClients { private set; get; }


        public Appliaction(JsonValue config)
        {
            this.configRoot = config["ConfigRoot"].AsString();
            this.port = config["Port"].AsInt();
            this.ServicePort = config["LoginServerProt"].AsInt();
            this.ServiceHost = config["LoginServerHost"].AsString();
            ServerHost = config["ServiceHost"].AsString();
            MaxBattleCount = config["MaxBattle"].AsInt();
            NetProtoTool.EnableLog = config["Log"].AsBoolean();
            Current = this;
            MonitorPool.Singleton.Init(this.GetType().Assembly);
            GateServerClients  = new SyncDictionary<int, RequestClient<TaskHandler>>();
        }


        //获取当前连接客户端
        internal Client GetClientByID(int clientID)
        {
            return this.ListenServer.CurrentConnectionManager.GetClientByID(clientID);
        }
        //根据userid获取当前连接客户端
        internal Client GetClientByUserID(long userID)
        {
            Client res = null;
            this.ListenServer.CurrentConnectionManager.Each((obj) =>
            {
                if ((long)obj.UserState == userID)
                {
                    res = obj;
                    return true;
                }
                return false;
            });
            return res;
        }

        //尝试连接用户所在网关服务器
        public void TryConnectUserServer(PlayerServerInfo player)
        {
            if (GateServerClients.HaveKey(player.ServerID)) return;
            var client = new RequestClient<TaskHandler>(player.ServiceHost, player.ServicePort)
            {
                UseSendThreadUpdate = true
            };
            client.Connect();
            client.UserState = player.ServerID;
            client.OnDisconnect += (s, e) => 
            {
                var c = s as RequestClient<TaskHandler>;
                var serverID = (int)c.UserState;
                GateServerClients.Remove(serverID);
            };
            GateServerClients.Add(player.ServerID, client);
        }

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;

            ResourcesLoader.Singleton.LoadAllConfig(configRoot);

           
            var listenHandler = new RequestHandle<BattleServerService>();
            
            ListenServer = new SocketServer(new ConnectionManager(), port);
            ListenServer.HandlerManager = listenHandler;
            ListenServer.Start();
            Client = new RequestClient<LoginServerTaskServicesTaskHandler>(ServiceHost, ServicePort)
            {
                UseSendThreadUpdate = true
            };
            Client.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    var r= RegBattleServer.CreateQuery().SendRequestAsync(Client, new B2L_RegBattleServer
                    {
                        MaxBattleCount = MaxBattleCount,
                        ServiceHost = ServerHost,
                        ServicePort = this.port,
                        Version = 1
                    }).GetAwaiter().GetResult();

                    if (r.Code == ErrorCode.Ok)
                    {
                        ServerID = r.ServiceServerID;
                        Debuger.Log("Server Reg Success!");
                    }
                    else
                    {
                        Debuger.Log("Can't Regsiter LoginServer!");
                        Stop();
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
                Debuger.Log("disconnect from LoginServer!");
                Stop();
            };
            Client.Connect();
            MonitorPool.Singleton.Start();
        }

        public void Stop()
        {
            if (!IsRunning) 
                return;
            IsRunning = false;
            MonitorPool.Singleton.Exit();
            Client.Disconnect();
            ListenServer.Stop();
            Debuger.Log("Server appliaction Stoped!");
        }

        public void Tick()
        {
            MonitorPool.Singleton.Tick();
        }

        //获取用户网关服务器
        public RequestClient<TaskHandler> GetGateServer(int serverID)
        {
            GateServerClients.TryToGetValue(serverID, out RequestClient<TaskHandler> client);
            return client;
        }
    }
}

