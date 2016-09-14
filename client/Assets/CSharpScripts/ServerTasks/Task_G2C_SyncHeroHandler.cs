using UnityEngine;
using System.Collections;
using Proto;

[ServerTask(typeof(Task_G2C_SyncHero))]
public class Task_G2C_SyncHeroHandler : TaskHandler<Task_G2C_SyncHero> {
    #region implemented abstract members of TaskHandler
    public override void DoTask(Task_G2C_SyncHero task)
    {
        var gata = UAppliaction.S.G<GMainGate>();
        gata.hero = task.Hero;
        UUIManager.S.UpdateUIData();
        //gata.CreateHero();
    }
    #endregion
}
