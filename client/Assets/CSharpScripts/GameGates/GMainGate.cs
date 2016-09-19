using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proto;
using ExcelConfig;
//using UApp = UAppliaction;
using System.Collections.Generic;

public class GMainGate:UGate
{
    public GMainGate(GameServerInfo gateServer)
    {
        ServerInfo = gateServer;
    }

    public int Gold;
    public int Coin;
    public PlayerPackage package;
    public DHero hero;
    private UCharacterView characterView;
    private GameServerInfo ServerInfo;
    public RequestClient Client{ private set; get; }

    public void UpdateItem(List<PlayerItem> diff)
    {

        var list = new List<string>();
        foreach (var i in diff)
        {
            foreach (var p in package.Items)
            {
                if (p.GUID == i.GUID)
                {
                    p.Num += i.Num;
                    if (p.Num <= 0)
                    {
                        list.Add(p.GUID);
                    }
                }
            }
        }
        foreach (var i in list)
        {
            package.Items.RemoveAll(t => t.GUID == i);
            package.Equips.RemoveAll(t => t.GUID == i);
        }
       

        UUIManager.S.UpdateUIData();
    }

    public void ReCreateHero()
    {

        if (characterView)
        {
            characterView.DestorySelf();
        }

        var character = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(this.hero.HeroID);
        characterView = UPerceptionView.Singleton.CreateBattleCharacterView(
            character.ResourcesPath, GTransform.Convent(data.pos[3].position),
            new EngineCore.GVector3(0, 0, 0)) as UCharacterView;
        var thridCamear = GameObject.FindObjectOfType<ThridPersionCameraContollor>();
        thridCamear.lookAt = characterView.GetBoneByName("Bottom");
         
        //thridCamear.forwardTrans = hero.GetBoneByName("Top");
        //GameObject.Instantiate(res, data.pos[0].position, Quaternion.identity);
    }

    #region implemented abstract members of UGate


    public override void OnTap(TapGesture gesutre)
    {
        //UApp.S.G<GMainGate>()
        //throw new NotImplementedException();
    }

   
    public override void JoinGate()
    {
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);
        //var address = System.Net.Dns.GetHostAddresses(ServerInfo.Host);
        Client = new RequestClient(ServerInfo.Host, ServerInfo.Port);
        Client.RegAssembly(this.GetType().Assembly);
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
        //select hero page
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
        //Result = result;
        Coin = result.Coin;
        Gold = result.Gold;
        hero = result.Hero;
        package = result.Package;
        operat = SceneManager.LoadSceneAsync("Main");
        //data
        //UUIManager.Singleton.ShowMask(false);
    }

    //private G2C_Login Result;

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
                ReCreateHero();
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
                    UAppliaction.S.GotoBattleGate(r.BattleServer, r.MapID);
            }
            else
            {
                    UAppliaction.S.ShowError(r.Code);    
            }
        };
        request.SendRequest();
    }
    #endregion

   
}