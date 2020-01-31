using XNet.Libs.Utility;
using XNet.Libs.Net;
using ServerUtility;
using org.vxwo.csharp.json;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Proto.MongoDB;

namespace LoginServer
{
    public class Appliaction
    {
        static Appliaction()
        {
            BsonClassMap.RegisterClassMap<PlayInfoEntity>(
            (cm) =>
            {
                cm.AutoMap();
                _ = cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }

        public static Appliaction Current
        {
            private set; get;
        }

        private JsonValue config;

        public Appliaction(JsonValue config)
        {
            this.config = config;
            this.port = config["ListenPort"].AsInt();
            this.servicePort = config["ServicePort"].AsInt();
            Current = this;
            this.ConnectionString = string.Format(config["DBHost"].AsString(), config["DBPwd"].AsString());
            //MonitorPool = MonitorPool.Singleton;
            MonitorPool.Singleton.Init(this.GetType().Assembly);
            NetProtoTool.EnableLog = config["Log"].AsBoolean();
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

        public  string ConnectionString { private set; get; }


        private readonly SyncDictionary<string, string> _sessions = new SyncDictionary<string, string>();

        public string GetSession(string userID)
        {
            _sessions.TryToGetValue(userID, out string session);
            return session;
        }


        public void SetSession(string userID, string session)
        {
            _sessions.Remove(userID);
            _sessions.Add(userID, session);
        }

    }
}

