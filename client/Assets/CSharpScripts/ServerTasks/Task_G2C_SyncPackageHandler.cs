using UnityEngine;
using System.Collections;
using Proto;

[ServerTask(typeof(Task_G2C_SyncPackage))]
public class Task_G2C_SyncPackageHandler : TaskHandler<Task_G2C_SyncPackage>
{
    #region implemented abstract members of TaskHandler
    public override void DoTask(Task_G2C_SyncPackage task)
    {
        var gata = UAppliaction.S.G<GMainGate>();
        gata.Coin = task.Coin;
        gata.Gold = task.Gold;
        gata.package = task.Package;
        UUIManager.S.UpdateUIData();
    }
    #endregion

}
