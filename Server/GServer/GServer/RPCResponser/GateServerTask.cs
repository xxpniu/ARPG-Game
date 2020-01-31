using System;
using Proto;
using Proto.GateServerTask;
using ServerUtility;

namespace GateServer
{
    public class GateServerTask: XSingleton<GateServerTask>, IGateServerTask
    {
        public GateServerTask()
        {
        }

        public Task_G2C_SyncHero SyncHero(Task_G2C_SyncHero req)
        {
            return req;
        }

        public Task_G2C_SyncPackage SyncPackage(Proto.Task_G2C_SyncPackage req)
        {
            return req;
        }
    }
}
