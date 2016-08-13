using System;
using Proto;

public class ExitBattleTask:TaskHandler<Task_B2C_ExitBattle>
{
    public ExitBattleTask()
    {
        
    }

    #region implemented abstract members of TaskHandler
    public override void DoTask(Task_B2C_ExitBattle task)
    {
        var gate = UAppliaction.Singleton.GetGate() as BattleGate;
        if (gate != null)
        {
            UAppliaction.Singleton.GoBackToMainGate();
        }
    }
    #endregion
}

