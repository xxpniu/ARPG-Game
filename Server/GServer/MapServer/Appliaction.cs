using System;
using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;
using ServerUtility;
using System.Linq;

namespace MapServer
{
    public class Appliaction
    {
        
        int port;
        int ServicePort;
        string ServiceHost;
        string Key;
        string configRoot;
        public int ServerID { set; get; }


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

        public static Appliaction Current { private set; get; }

        public SocketServer ListenServer { private set; get; }

        private volatile int SimulaterIndex = 0;

        internal bool BeginSimulater(Client client, long userID, int mapID)
        {
            var simulater = new ServerWorldSimluater(mapID, SimulaterIndex++);
            this.Simluaters.Add(simulater.Index, simulater);
            simulater.AddClient(client);
            return true;
        }

        public RequestClient Client { private set; get; }

        public volatile bool IsRunning;

        public Appliaction(int port, 
                           string serivecHost, 
                           int servicePort, 
                           int serverID,
                           string key,
                           string configRoot)
        {
            RequestHandle.RegAssembly(this.GetType().Assembly);
            this.configRoot = configRoot;
            this.Key = key;
            this.port = port;
            this.ServicePort = servicePort;
            this.ServiceHost = serivecHost;
            Current = this;
            ServerID = serverID;
            Simluaters = new SyncDictionary<int, ServerWorldSimluater>();
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

            Client = new RequestClient(ServiceHost, ServicePort);
            Client.RegAssembly(this.GetType().Assembly);
            Client.UseSendThreadUpdate = true;
            Client.OnConnectCompleted = (s, e) =>
            {
                if (e.Success)
                {
                    
                    var request = Client.CreateRequest<B2L_RegBattleServer, L2B_RegBattleServer>();

                    request.RequestMessage.MaxBattleCount = 1000;
                    request.RequestMessage.ServiceHost = "127.0.0.1";
                    request.RequestMessage.ServicePort = this.port;
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

        public SyncDictionary<int, ServerWorldSimluater> Simluaters { private set; get; }

    }
}

