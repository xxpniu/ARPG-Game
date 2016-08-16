using System;
using EngineCore.Simulater;
using UnityEngine;

public class LoginGate:UGate
{
    public LoginGate()
    {
    }



    #region implemented abstract members of UGate

    public override void JoinGate()
    {
        
        UUIManager.Singleton.HideAll();
        var ui = UUIManager.Singleton.CreateWindow<Windows.UUILogin>();
        ui.ShowWindow();

        var ServerHost = UAppliaction.Singleton.ServerHost;
        var ServerPort = UAppliaction.Singleton.ServerPort;
        Client = new RequestClient(ServerHost,ServerPort);
        //Client.UseSendThreadUpdate = false;
        UAppliaction.Singleton.ConnectTime = Time.time;
        Client.Connect();
    }

    public override void ExitGate()
    {
        Debug.Log("Exit gate");
        if (Client.IsConnect)
        {
            Client.Disconnect();
        }
    }

    public RequestClient Client;

    public override void Tick()
    {
        if (Client != null)
        {
            Client.Update();
            UAppliaction.Singleton.PingDelay = (float)Client.Delay / (float)TimeSpan.TicksPerMillisecond;

            if (!Client.IsConnect)
            {
                if (lastTime + 5 < Time.time)
                {
                    lastTime = Time.time;
                    Client.Connect();
                }
            }
        }

       
    }

    private float lastTime = 0;

    public override GTime GetTime()
    {
        return  new GTime(Time.time, Time.deltaTime);
    }

    #endregion
}