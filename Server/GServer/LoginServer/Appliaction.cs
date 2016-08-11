using System.Data;
using XNet.Libs.Utility;
using XNet.Libs.Net;
using ServerUtility;
using MySql.Data.MySqlClient;
using org.vxwo.csharp.json;

namespace LoginServer
{
    public class Appliaction
    {

        public static Appliaction Current
        { 
            private set; get; 
        }

        public Appliaction(JsonValue config )
        {
            RequestHandle.RegAssembly(this.GetType().Assembly);
            this.port = config["ListenPort"].AsInt();
            this.servicePort = config["ServicePort"].AsInt();
            Current = this;
            this.ConnectionString =string.Format(
               "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                config["DBHost"].AsString(),
                config["DBName"].AsString(), 
                config["DBUser"].AsString(),
                config["DBPwd"].AsString()
            );
            Servers = new SyncDictionary<int, Proto.GameServerInfo>();
        }

        private int servicePort;
        private int port = 0;

        public volatile bool IsRunning = false;
        public void Start()
        {
            if (IsRunning) return;


            IsRunning = true;
            var manager = new ConnectionManager();
            Server = new SocketServer(manager, port);
            var handeler = new RequestHandle();
            Server.HandlerManager = handeler;
            Server.Start();

            var serviceManager = new ConnectionManager();
            var serviceHandler = new RequestHandle();

            ServiceServer = new SocketServer(serviceManager, servicePort);
            ServiceServer.HandlerManager = serviceHandler;
            ServiceServer.Start();
        }

        private SocketServer Server;

        private SocketServer ServiceServer;

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            ServiceServer.Stop();
            Server.Stop();
        }

        public void Tick()
        {
           
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

        public SyncDictionary<int, Proto.GameServerInfo> Servers { private set; get; }

    }
}

