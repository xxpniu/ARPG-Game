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

   

    public override void OnTap(TapGesture gesutre)
    {
        //throw new NotImplementedException();
    }

   

    public override void JoinGate()
    {
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);
        //var address = System.Net.Dns.GetHostAddresses(ServerInfo.Host);
        Client = new RequestClient(ServerInfo.Host, ServerInfo.Port);
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
        Client.OnDisconnect += OnDisconnect;
        Client.Connect();
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
        Result = result;
        operat = SceneManager.LoadSceneAsync("Main");
        //data
        //UUIManager.Singleton.ShowMask(false);
    }

    private G2C_Login Result;

    private AsyncOperation operat;

    public override void ExitGate()
    {
        if (Client != null && Client.IsConnect)
        {
            Client.OnDisconnect -= OnDisconnect;
            Client.Disconnect();
        }
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
                var character = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(Result.Heros[0].HeroID);
                var hero = UPerceptionView.Singleton.CreateBattleCharacterView(
                               character.ResourcesPath, GTransform.Convent(data.pos[3].position),
                    new EngineCore.GVector3(0, 0, 0)) as UCharacterView;
                var thridCamear = GameObject.FindObjectOfType<ThridPersionCameraContollor>();
                thridCamear.lookAt = hero .transform;
                //thridCamear.forwardTrans = hero.GetBoneByName("Top");
                //GameObject.Instantiate(res, data.pos[0].position, Quaternion.identity);
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

    private void OnDisconnect(object s, EventArgs e)
    {
        UAppliaction.Singleton.GotoLoginGate();
    }

    private MainData data;

    public void TryToJoinLastBattle()
    {
        var request = Client.CreateRequest<C2G_GetLastBattle,G2C_GetLastBattle>();
        request.RequestMessage.UserID = UAppliaction.Singleton.UserID;
        request.OnCompleted = (s, r) =>
        {
            if (r.Code == ErrorCode.OK)
            {
                UAppliaction.Singleton.GotoBattleGate(r.BattleServer, r.MapID);
            }
            else
            {
                UAppliaction.Singleton.ShowError(r.Code);    
            }
        };
        request.SendRequest();
    }
     #endregion

   
}