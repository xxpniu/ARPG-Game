using System;
using Proto;
using Proto.GateServerTask;
using ServerUtility;

namespace GateServer
{
    public class GateServerTask : XSingleton<GateServerTask>, IGateServerTask
    {

        public Task_G2C_JoinBattle JoinBattle(Proto.Void req) { return default; }
      
        public Task_G2C_SyncHero SyncHero(Proto.Void req) { return default; }
   
        public Task_G2C_SyncPackage SyncPackage(Proto.Void req) { return default; }
       
    }
}
