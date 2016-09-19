using System;
using UnityEngine;
using Tips;
using System.Collections.Generic;

public class UUITipDrawer:XSingleton<UUITipDrawer>
{

    private class NotifyMessage
    {
        public float time;
        public string message;
        public int ID = -1;
    }

    public int DrawHPNumber(int instanceID,int hp, Vector2 offset)
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

    public  int DrawUUITipHpBar(int instanceId, int hp, int hpMax, Vector2 offset)
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

    #region Notify
    private  int DrawUUINotify(int instanceId, string notify)
    {
        UUINotify tip;
        if (!UUIManager.Singleton.TryToGetTip<UUINotify>(instanceId,out tip))
        {
            tip = UUIManager.Singleton.CreateTip<UUINotify>();
            tip.SetNotify(notify);
        }
        UUITip.Update(tip);
        return tip.InstanceID;
    }
        
    private  List<NotifyMessage> notifys= new List<NotifyMessage>();
    private Queue<NotifyMessage> _dels = new Queue<NotifyMessage>();

    public void ShowNotify(string notify)
    {
        notifys.Add(new NotifyMessage{ message = notify, time = Time.time +4.5f });
        Debug.Log(notify);
    }
    #endregion

    public void Update()
    {
        foreach (var i in notifys)
        {
            i.ID = DrawUUINotify(i.ID, i.message);
            if (i.time < Time.time)
            {
                _dels.Enqueue(i);
            }
        }

        while (_dels.Count > 0)
        {
            notifys.Remove(_dels.Dequeue());
        }

    }
}


