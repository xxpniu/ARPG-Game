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
        

        private readonly SyncDictionary<int, ServerWorldSimluater> simluater = new SyncDictionary<int, ServerWorldSimluater>();

        private bool isStoped = false;

        public void OnExit()
        {
            isStoped = true;
            foreach (var i in simluater.Values)
            {
                i.Exit();// = true;
                //i.Runner.Join(100);
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

        private volatile int Index = 1;

        public ServerWorldSimluater BeginSimulater(BattlePlayer player)
        {
            if (isStoped) return null;

            var worldSimulater = new ServerWorldSimluater(player.MapID, 
                                                          Index++, 
                                                          new List<BattlePlayer> { player});
            var client = Appliaction.Current.GetClientByID(player.ClientID);
            worldSimulater.AddClient(client);
            simluater.Add(worldSimulater.Index, worldSimulater);
            player.SimulaterIndex = worldSimulater.Index;
            return worldSimulater;
        }

        public void ExitUser(string account_uuid, int simulaterIndex)
        {
            ServerWorldSimluater sim;
            if (simluater.TryToGetValue(simulaterIndex, out sim))
            {
                sim.KickUser(account_uuid);
                Debuger.Log("Kick User:" + account_uuid +" In simulater "+simulaterIndex);
            }
        }

        public void EndSumlater(int sumlaterIndex)
        {
            simluater.Remove(sumlaterIndex);
        }
    }
}

