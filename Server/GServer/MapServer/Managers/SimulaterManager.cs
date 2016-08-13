using System;
using ServerUtility;
using Proto;
using XNet.Libs.Utility;

namespace MapServer.Managers
{
    [Monitor]
    public class SimulaterManager:IMonitor
    {
        public SimulaterManager()
        {
            Singleton = this;
        }

        public static SimulaterManager Singleton { private set; get; }
        private SyncDictionary<int, ServerWorldSimluater> simluater = new SyncDictionary<int, ServerWorldSimluater>(); 

        public void OnExit()
        {
            
        }

        public void OnShowState()
        {
            
        }

        public void OnStart()
        {
           
        }

        public void OnTick()
        {
           
        }

        private volatile int Index = 0;

        public void BeginSimulater(BattlePlayer player)
        {
            var worldSimulater = new ServerWorldSimluater(player.MapID, Index++);
            var client = Appliaction.Current.GetClientByID(player.ClientID);
            worldSimulater.AddClient(client);
            simluater.Add(worldSimulater.Index, worldSimulater);
            worldSimulater.Runner.Start();
        }
    }
}

