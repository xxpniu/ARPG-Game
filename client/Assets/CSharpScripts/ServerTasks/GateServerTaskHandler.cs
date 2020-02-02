using System;
using Proto;
using Proto.GateServerTask;

[TaskHandler(typeof(IGateServerTask))]
public  class GateServerTaskHandler: XSingleton<GateServerTaskHandler>, IGateServerTask
{

    public Task_G2C_JoinBattle JoinBattle(Task_G2C_JoinBattle req)
    {
        return req;
    }

    public Task_G2C_SyncHero SyncHero(Task_G2C_SyncHero req)
    {
        var gata = UApplication.S.G<GMainGate>();
        gata.hero = req.Hero;
        UUIManager.S.UpdateUIData();
        gata.ReCreateHero();
        return req;
    }

    public Task_G2C_SyncPackage SyncPackage(Task_G2C_SyncPackage req)
    {
        var gata = UApplication.S.G<GMainGate>();
        gata.Coin = req.Coin;
        gata.Gold = req.Gold;
        gata.package = req.Package;
        UUIManager.S.UpdateUIData();
        return req;
    }
}
