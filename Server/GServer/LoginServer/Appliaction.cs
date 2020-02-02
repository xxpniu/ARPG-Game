using XNet.Libs.Utility;
using XNet.Libs.Net;
using ServerUtility;
using org.vxwo.csharp.json;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Proto.MongoDB;
using MongoTool;

namespace LoginServer
{
    public class Appliaction
    {
        

        public static Appliaction Current
        {
            private set; get;
        }

        private readonly JsonValue config;

        public Appliaction(JsonValue config)
        {
            this.config = config;
            this.port = config["ListenPort"].AsInt();
            this.servicePort = config["ServicePort"].AsInt();
            Current = this;
            MonitorPool.Singleton.Init(this.GetType().Assembly);
            NetProtoTool.EnableLog = config["Log"].AsBoolean();
            DataBase.S.Init(config["DBHost"].AsString(), config["DBName"].AsString());
        }

        public JsonValue this[string key]
        {
            get { return config[key]; } 

        }

        private readonly int servicePort;
        private readonly int port = 0;

        public volatile bool IsRunning = false;
        public void Start()
        {
            if (IsRunning) return;
            MonitorPool.Singleton.Start();
            IsRunning = true;
            //对外端口不能全部注册
            var handeler = new RequestHandle<RPCResponsers.LoginServerService>();
            var manager = new ConnectionManager();
            Server = new SocketServer(manager, port)
            {
                HandlerManager = handeler
            };
            Server.Start();
            //对内就全部注册
            var serviceManager = new ConnectionManager();
            var serviceHandler = new RequestHandle<RPCResponsers.LoginBattleGameServerService>();
            ServiceServer = new SocketServer(serviceManager, servicePort)
            {
                HandlerManager = serviceHandler
            };
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
            MonitorPool.Singleton.Exit();
            IsRunning = false;
            ServiceServer.Stop();
            Server.Stop();
        }

        public void Tick()
        {
            MonitorPool.Singleton.Tick();
        }


    }
}

