using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginGate:UGate
{
    public LoginGate()
    {
    }



  
    #region implemented abstract members of UGate

    public override void JoinGate()
    {
        SceneManager.LoadScene("null");
        UUIManager.Singleton.HideAll();
        var ui = UUIManager.Singleton.CreateWindow<Windows.UUILogin>();
        ui.ShowWindow();

        var ServerHost = UApplication.Singleton.ServerHost;
        var ServerPort = UApplication.Singleton.ServerPort;
        Client = new RequestClient<EmptyTaskHandle>(ServerHost,ServerPort);
        //Client.UseSendThreadUpdate = false;
        UApplication.Singleton.ConnectTime = Time.time;
        Client.Connect();

    }

    public override void ExitGate()
    {
        Debug.Log("Exit gate");
        if (Client.IsConnect)
        {
            Client.Disconnect();
            lastTime = Time.time;
        }
    }

    public RequestClient<EmptyTaskHandle> Client;

    public override void Tick()
    {
        if (Client != null)
        {
            Client.Update();
            UApplication.Singleton.PingDelay = (float)Client.Delay / (float)TimeSpan.TicksPerMillisecond;

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

    #endregion
}