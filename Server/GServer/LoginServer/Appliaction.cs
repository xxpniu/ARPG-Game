using System.Data;
using XNet.Libs.Utility;
using XNet.Libs.Net;
using ServerUtility;
using MySql.Data.MySqlClient;
using org.vxwo.csharp.json;
using Proto;
using System;
using LoginServer.Responsers;

namespace LoginServer
{
    public class Appliaction
    {

        public static Appliaction Current
        {
            private set; get;
        }

        public Appliaction(JsonValue config)
        {
            this.port = config["ListenPort"].AsInt();
            this.servicePort = config["ServicePort"].AsInt();
            Current = this;
            this.ConnectionString = string.Format(
               "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                config["DBHost"].AsString(),
                config["DBName"].AsString(),
                config["DBUser"].AsString(),
                config["DBPwd"].AsString()
            );
            //MonitorPool = MonitorPool.Singleton;
            MonitorPool.Singleton.Init(this.GetType().Assembly);
            NetProtoTool.EnableLog = config["Log"].AsBoolean();
        }
        //private MonitorPool MonitorPool;

        private int servicePort;
        private int port = 0;

        public volatile bool IsRunning = false;
        public void Start()
        {
            if (IsRunning) return;

            MonitorPool.Singleton.Start();
            IsRunning = true;
            //对外端口不能全部注册
            var handeler = new RequestHandle();
            handeler.RegAssembly(this.GetType().Assembly, HandleResponserType.CLIENT_SERVER);


            var manager = new ConnectionManager();
            Server = new SocketServer(manager, port);
            Server.HandlerManager = handeler;
            Server.Start();

            //对内就全部注册
            var serviceManager = new ConnectionManager();
            var serviceHandler = new RequestHandle();
            serviceHandler.RegAssembly(this.GetType().Assembly,HandleResponserType.SERVER_SERVER);
            ServiceServer = new SocketServer(serviceManager, servicePort);
            ServiceServer.HandlerManager = serviceHandler;
            ServiceServer.Start();
        }

        private SocketServer Server;

        private SocketServer ServiceServer;

        internal Client GetServerConnectByClientID(int clientID)
        {
            return ServiceServer.CurrentConnectionManager.GetClientByID(clientID);
        }

        public void Stop()
        {
            if (!IsRunning) return;

            //close All client

            MonitorPool.Singleton.Exit();
            IsRunning = false;
            ServiceServer.Stop();
            Server.Stop();
        }

        public void Tick()
        {
            MonitorPool.Singleton.Tick();
        }

        private string ConnectionString;

        public MySqlConnection Connection
        {
            get
            {
                return new MySqlConnection(this.ConnectionString);
            }
        }


        private SyncDictionary<long, string> _sessions = new SyncDictionary<long, string>();

        public string GetSession(long userID)
        {
            var session = string.Empty;
            _sessions.TryToGetValue(userID, out session);
            return session;
        }


        public void SetSession(long userID, string session)
        {
            _sessions.Remove(userID);
            _sessions.Add(userID, session);
        }


    }
}

