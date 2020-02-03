using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using XNet.Libs.Net;

public class LoginGate:UGate
{
    
    #region implemented abstract members of UGate

    protected override void JoinGate()
    {
        SceneManager.LoadScene("null");
        UUIManager.Singleton.HideAll();
        var ui = UUIManager.Singleton.CreateWindow<Windows.UUILogin>();
        ui.ShowWindow();

        var ServerHost = UApplication.Singleton.ServerHost;
        var ServerPort = UApplication.Singleton.ServerPort;
        Client = new RequestClient<TaskHandler>(ServerHost,ServerPort,false);
        UApplication.Singleton.ConnectTime = Time.time;
        Client.Connect();

    }

    protected override void ExitGate()
    {
        Debug.Log("Exit gate");
        Client?.Disconnect();
        lastTime = Time.time;
    }

    public RequestClient<TaskHandler> Client;

    protected override void Tick()
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