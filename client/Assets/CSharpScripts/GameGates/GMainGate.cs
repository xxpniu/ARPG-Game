using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proto;
using ExcelConfig;

public class GMainGate:UGate
{
    public GMainGate(GameServerInfo gateServer)
    {
        ServerInfo = gateServer;
    }

    private GameServerInfo ServerInfo;

    public RequestClient Client{ private set; get; }

    #region implemented abstract members of UGate

    public override void JoinGate()
    {
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);
        //var address = System.Net.Dns.GetHostAddresses(ServerInfo.Host);
        operat = SceneManager.LoadSceneAsync("Main");
    }

    private void RequestPlayer()
    {
        var request = Client.CreateRequest<C2G_Login,G2C_Login>();
        request.RequestMessage.Session = UAppliaction.Singleton.SesssionKey;
        request.RequestMessage.UserID = UAppliaction.Singleton.UserID;
        request.RequestMessage.Version = ProtoTool.GetVersion();
        request.OnCompleted = (su, r) =>
        {
            if (r.Code == ErrorCode.OK)
            {
                GetPlayerData(r);
            }
            else if (r.Code == ErrorCode.NoGamePlayerData)
            {
                ShowCreateHero(); 
            }
            else
            {
                UUITipDrawer.Singleton.ShowNotify("GateServer Response:" + r.Code);
                UAppliaction.Singleton.GotoLoginGate();
            }
        };
        request.SendRequest();
    }

    private void ShowCreateHero()
    {
        var request = Client.CreateRequest<C2G_CreateHero,G2C_CreateHero>();
        request.RequestMessage.HeroID = 4;
        request.OnCompleted = (s, r) =>
        {
            if (r.Code == ErrorCode.OK)
            {
                RequestPlayer();
            }
            else
            {
                UUITipDrawer.Singleton.ShowNotify("CreatHero Response:" + r.Code);
            }
        };
        request.SendRequest();
    }

    private void GetPlayerData(G2C_Login result)
    {
        //data
      
       
    }

    private AsyncOperation operat;

    public override void ExitGate()
    {
        if (Client != null && Client.IsConnect)
            Client.Disconnect();
        
        UUIManager.Singleton.ShowMask(false);
        UUIManager.Singleton.HideAll();
    }

    public override void Tick()
    {
        if (operat != null)
        {
            if (operat.isDone)
            {
                UUIManager.Singleton.ShowMask(false);
                operat = null;
                var ui = UUIManager.Singleton.CreateWindow<Windows.UUIMain>();
                ui.ShowWindow();
                data = GameObject.FindObjectOfType<MainData>();
                var character = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(4);
                var res = ResourcesManager.Singleton.LoadResourcesWithExName<GameObject>(character.ResourcesPath);
                GameObject.Instantiate(res, data.pos[0].position, Quaternion.identity);
                Client = new RequestClient(ServerInfo.Host, ServerInfo.Port);
                Client.Connect();
                Client.OnConnectCompleted = (s, e) =>
                    {
                        UAppliaction.Singleton.ConnectTime = Time.time;
                        if (e.Success)
                        {
                            RequestPlayer();
                        }
                        else
                        {
                            UAppliaction.Singleton.GotoLoginGate();
                        }
                    };
            }
        }

        if (Client != null)
        {
            Client.Update();
            UAppliaction.Singleton.ReceiveTotal = Client.ReceiveSize;
            UAppliaction.Singleton.SendTotal = Client.SendSize;
            UAppliaction.Singleton.PingDelay = (float)Client.Delay / (float)TimeSpan.TicksPerMillisecond;
        }
    }


    private MainData data;

    public override GTime GetTime()
    {
        return new GTime(Time.time, Time.deltaTime);
    }

    #endregion

   
}