using System;
using ServerUtility;
using Proto;
using XNet.Libs.Utility;
using System.Collections.Generic;

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


        private bool isStoped = false;

        public void OnExit()
        {
            isStoped = true;
            foreach (var i in simluater.Values)
            {
                i.Exit();// = true;
                i.Runner.Join(100);
            }
            simluater.Clear();
        }

        public void OnShowState()
        {
            
        }

        public void OnStart()
        {

            isStoped = false;
        }

        public void OnTick()
        {
            
        }

        private volatile int Index = 0;

        public void BeginSimulater(BattlePlayer player)
        {
            if (isStoped) return;
            var worldSimulater = new ServerWorldSimluater(player.MapID, 
                                                          Index++, 
                                                          new List<BattlePlayer> { player});
            var client = Appliaction.Current.GetClientByID(player.ClientID);
            worldSimulater.AddClient(client);
            simluater.Add(worldSimulater.Index, worldSimulater);
            worldSimulater.Runner.Start();
            player.SimulaterIndex = worldSimulater.Index;
        }

        public void ExitUser(long userid, int simulaterIndex)
        {
            ServerWorldSimluater sim;
            if (simluater.TryToGetValue(simulaterIndex, out sim))
            {
                //sim.ExitUser(userid);
            }
        }
    }
}

