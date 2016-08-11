using System;
using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;
using ServerUtility;
using MySql.Data.MySqlClient;
using System.Linq;

namespace GServer
{
    public class Appliaction
    {
        
        int port;
        int ServicePort;
        string LoginHost;
        int LoginPort;
        string Key;
        string configRoot;
        public int ServerID { set; get; }

        private string ConnectionString;

        internal Client GetClientByUserID(long userID)
        {
            Client res =null;
            this.ListenServer.CurrentConnectionManager.Each((obj) => {
                if ((long)obj.UserState == userID)
                {
                    res = obj;
                    return true;
                }
                return false;
            });
            return res;
        }

        public MySqlConnection Connection
        {
            get
            {
                return new MySqlConnection(this.ConnectionString);
            }
        }
        public static Appliaction Current { private set; get; }

        //玩家访问端口
        public SocketServer ListenServer { private set; get; }

        //游戏战斗服务器访问端口
        public SocketServer ServiceServer { private set; get; }

        public RequestClient Client { private set; get; }

        public volatile bool IsRunning;

        public Appliaction(int port, 
                           int servicePort,
                           string loginHost, 
                           int loginPort, 
                           string datasources, 
                           string db, 
                           string username,
                           string pwd, 
                           int serverID,
                           string key,
                           string configRoot)
        {
            RequestHandle.RegAssembly(this.GetType().Assembly);
            this.configRoot = configRoot;
            this.Key = key;
            this.port = port;
            this.ServicePort = servicePort; 
            this.LoginPort = loginPort;
            this.LoignHost = loginHost;
            Current = this;
            this.ConnectionString = string.Format(
               "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                datasources, db, username, pwd);
            ServerID = serverID;
           
        }

        public Client GetClientById(int index)
        {
            return ListenServer.CurrentConnectionManager.GetClientByID(index);
        }

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
           
            ResourcesLoader.Singleton.LoadAllConfig(configRoot);


            var cm = new ConnectionManager();
            ListenServer = new SocketServer(cm, port);
            ListenServer.HandlerManager = new RequestHandle();
            ListenServer.Start();



            Client = new RequestClient(LoginHost, LoginPort);
            Client.RegAssembly(this.GetType().Assembly);
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
                    request.RequestMessage.Host = "127.0.0.1";
                    request.RequestMessage.Key = this.Key;
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
            IsRunning = false;
            ListenServer.Stop();
            Client.Disconnect();
        }

        public void Tick()
        {
            
        }

       

    }
}

