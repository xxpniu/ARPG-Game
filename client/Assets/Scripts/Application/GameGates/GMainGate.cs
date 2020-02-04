using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proto;
using ExcelConfig;
using System.Collections.Generic;
using UMath;
using GameLogic.Game.Perceptions;
using UGameTools;
using System.Linq;
using EConfig;
using Proto.GateServerService;
using System.Threading.Tasks;
using Windows;
using System.Collections;
using XNet.Libs.Net;

public class GMainGate:UGate
{
    public void Init(GameServerInfo gateServer)
    {
        ServerInfo = gateServer;
    }

    public UPerceptionView view;
    public MainData Data;
    public int Gold;
    public int Coin;
    public PlayerPackage package;
    public DHero hero;
    private UCharacterView characterView;
    private GameServerInfo ServerInfo;
    public RequestClient<GateServerTaskHandler> Client{ private set; get; }

    public void UpdateItem(IList<PlayerItem> diff)
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
            var item = package.Items.FirstOrDefault(t => t.GUID == i);
            if (item == null) continue;
            package.Items.Remove(item);
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
        var perView = view as IBattlePerception;
        characterView = perView.CreateBattleCharacterView(
            character.ResourcesPath,Data.pos[3].position.ToGVer3(),
            new UVector3(0, 0, 0)) as UCharacterView;
        var thridCamear = GameObject.FindObjectOfType<ThridPersionCameraContollor>();
        thridCamear.lookAt = characterView.GetBoneByName("Bottom");
         
        //thridCamear.forwardTrans = hero.GetBoneByName("Top");
        //GameObject.Instantiate(res, data.pos[0].position, Quaternion.identity);
    }

    #region implemented abstract members of UGate

   

    protected override void JoinGate()
    {
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);

        StartCoroutine(StartInit());
    }

    private IEnumerator StartInit()
    {

        yield return SceneManager.LoadSceneAsync("Main");

        Data = GameObject.FindObjectOfType<MainData>();
        view = UPerceptionView.Create();
        Client = new RequestClient<GateServerTaskHandler>(ServerInfo.Host, ServerInfo.Port, false)
        {
            OnConnectCompleted = (success) =>
            {
                UApplication.S.ConnectTime = Time.time;
                if (success)
                {
                    _ = RequestPlayer();
                }
                else
                {
                    UApplication.S.GotoLoginGate();
                }
            },
            OnDisconnect = OnDisconnect
        };
        Client.Connect();
    }

    private async Task RequestPlayer()
    {

        var r = await Login.CreateQuery()
             .SendAsync(Client, new C2G_Login
             {
                 Session = UApplication.S.SesssionKey,
                 UserID = UApplication.S.AccountUuid,
                 Version = 1
             });

        if (r.Code.IsOk())
        {
            ShowPlayer(r);
        }
        else
        {
            UUITipDrawer.S.ShowNotify("GateServer Response:" + r.Code);
            UApplication.S.GotoLoginGate();
        }
    }

    private async Task ShowCreateHero(int heroId, string heroName )
    {
        var r = await CreateHero.CreateQuery()
            .SendAsync(Client, new C2G_CreateHero { HeroID = heroId, HeroName = heroName });
        if (r.Code == ErrorCode.Ok)
        {
            await RequestPlayer();
        }
        else
        {
            UUITipDrawer.Singleton.ShowNotify("CreatHero Response:" + r.Code);
        }
    }

    private void ShowPlayer(G2C_Login result)
    {
        //Result = result;
        Coin = result.Coin;
        Gold = result.Gold;
        hero = result.Hero;
        // package = result.Package;
        UUIManager.Singleton.ShowMask(false);

        if (result.HavePlayer)
        {
            ShowMain();
        }
        else
        {
            //todo
            //create  hero
            _ = ShowCreateHero(4, "Max");
        }
    }

    public void ShowMain()
    {
        var ui = UUIManager.Singleton.CreateWindow<UUIMain>();
        ui.ShowWindow();
        //ReCreateHero();
    }

    protected override void ExitGate()
    {
        Client?.Disconnect();
        UUIManager.Singleton.ShowMask(false);
        UUIManager.Singleton.HideAll();
       
    }

    protected override void Tick()
    {
        if (Client == null) return;
        Client.Update();
        UApplication.Singleton.ReceiveTotal = Client.ReceiveSize;
        UApplication.Singleton.SendTotal = Client.SendSize;
        UApplication.Singleton.PingDelay = (float)Client.Delay / (float)TimeSpan.TicksPerMillisecond;
    }

    private void OnDisconnect()
    {
        UApplication.S.GotoLoginGate();
    }

    

    public async Task TryToJoinLastBattle()
    {
        var r = await GetLastBattle.CreateQuery()
             .SendAsync(Client,
             new C2G_GetLastBattle
             {
                 AccountUuid = UApplication.S.AccountUuid
             });

        if (r.Code == ErrorCode.Ok)
        {
            UApplication.S.GotoBattleGate(r.BattleServer, r.MapID);
        }
        else
        {
            UApplication.S.ShowError(r.Code);
        }
    }
    #endregion

   
}