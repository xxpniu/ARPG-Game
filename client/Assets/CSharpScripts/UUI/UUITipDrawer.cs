using System;
using UnityEngine;
using Tips;

public sealed class UUITipDrawer
{
    public static int DrawHPNumber(int instanceID,int hp, Vector2 offset)
    {
        UUIHpNumber tip;
        if (!UUIManager.Singleton.TryToGetTip<UUIHpNumber>(instanceID,out tip))
        {
            tip = UUIManager.Singleton.CreateTip<UUIHpNumber>();
            tip.SetHp(hp);
        }
        UUITip.Update(tip, offset);
        return tip.InstanceID;
    }

    public static int DrawUUITipHpBar(int instanceId, int hp, int hpMax, Vector2 offset)
    {
        UUITipHpBar tip;
        if (!UUIManager.Singleton.TryToGetTip<UUITipHpBar>(instanceId,out tip))
        {
            tip = UUIManager.Singleton.CreateTip<UUITipHpBar>();
        }
        tip.SetHp(hp,hpMax);
        UUITip.Update(tip, offset);
        return tip.InstanceID;
    }
}


