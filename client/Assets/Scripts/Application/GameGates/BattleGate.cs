using System;
using Proto;
using UnityEngine;
using XNet.Libs.Net;
using UnityEngine.SceneManagement;
using EConfig;
using Google.Protobuf;
using Proto.BattleServerService;
using XNet.Libs.Utility;
using ExcelConfig;
using System.Collections;
using GameLogic.Game.Perceptions;

public class BattleGate : UGate
{

    public class NotifyHandle : IServerMessageHandler
    {
        private readonly BattleGate battleGate;

        public NotifyHandle(BattleGate battleGate)
        {
            this.battleGate = battleGate;
        }

        public void Handle(Message message, SocketClient client)
        {
            battleGate.ProcessNotify(message.AsNotify());
        }
    }

    public void SetServer(GameServerInfo serverInfo, int mapID)
    {
        ServerInfo = serverInfo;
        MapID = mapID;
        MapConfig = ExcelToJSONConfigManager.Current.GetConfigByID<MapData>(MapID);
        player.OnCreateUser = (view) =>
        {
            var character = view as UCharacterView;
            if (UApplication.S.AccountUuid == character.AccoundUuid)
            {
                FindObjectOfType<ThridPersionCameraContollor>()?.SetLookAt(character.GetBoneByName("Bottom"));
                UUIManager.Singleton.ShowMask(false);
                var ui = UUIManager.Singleton.GetUIWindow<Windows.UUIBattle>();
                ui.InitCharacter(character);
            }
        };
        player.OnDeath = (view) =>
        {
            var character = view as UCharacterView;
            if (UApplication.S.AccountUuid == character.AccoundUuid)
            {
                //Go to Main
                //dead
            }
        };

        player.OnJoined = (initPack) =>
        {
            if (UApplication.Singleton.AccountUuid == initPack.AccountUuid)
            {
                startTime = Time.time;
                ServerStartTime = initPack.TimeNow;
            }
        };

        player.OnDrop = (drop) =>
        {

        };
    }

    public float TimeServerNow
    {
        get
        {
            if (startTime < 0)
                return 0f;
            return Time.time - startTime + ServerStartTime;
        }
    }
    private float startTime = -1f;
    private float ServerStartTime = 0;

    private MapData MapConfig;

    public void ProcessNotify(IMessage notify)
    {
        player.Process(notify);
    }

    private  NotifyPlayer player;

    private GameServerInfo ServerInfo;
    private int MapID;
    public RequestClient<TaskHandler> Client { set; get; }

    public IBattlePerception PreView { get; internal set; }

    #region implemented abstract members of UGate

    protected override void JoinGate()
    {
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);
        var ui = UUIManager.Singleton.CreateWindow<Windows.UUIBattle>();
        ui.ShowWindow();
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return SceneManager.LoadSceneAsync(MapConfig.LevelName);

        PreView = UPerceptionView.Create();
        player = new NotifyPlayer(PreView);

        Client = new RequestClient<TaskHandler>(ServerInfo.Host, ServerInfo.Port,false);
        Client.RegisterHandler(MessageClass.Notify, new NotifyHandle(this));
        Client.OnConnectCompleted += (success) =>
        {
            UApplication.Singleton.ConnectTime = Time.time;
            if (success)
            {
                _ = JoinBattle.CreateQuery()
                .SendRequest(Client, new C2B_JoinBattle
                {
                    Session = UApplication.S.SesssionKey,
                    AccountUuid = UApplication.S.AccountUuid,
                    MapID = MapID,
                    Version = 1
                },
                (r) =>
                {
                    UUITipDrawer.Singleton.ShowNotify("BattleServer:" + r.Code);
                    UApplication.Singleton.GoBackToMainGate();
                });
            }
            else
            {
                UUITipDrawer.Singleton.ShowNotify("Can't login BattleServer!");
                UApplication.Singleton.GoBackToMainGate();
            }
        };
        Client.OnDisconnect += OnDisconnect;
        Client.Connect();
    }

    protected override void ExitGate()
    {
        Client?.Disconnect();
        UUIManager.Singleton.ShowMask(false);
    }

    private void OnDisconnect()
    {
        //UUITipDrawer.Singleton.ShowNotify("Can't login BattleServer!");
        UApplication.S.GoBackToMainGate();  
    }

    protected override void Tick()
    {
        if (Client != null)
        {
            Client.Update();
            UApplication.Singleton.ReceiveTotal = Client.ReceiveSize;
            UApplication.Singleton.SendTotal = Client.SendSize;
            UApplication.Singleton.PingDelay = (float)Client.Delay / (float)TimeSpan.TicksPerMillisecond;
        }
    }

    public void SendAction(IMessage action)
    {
        Client.SendMessage(action.ToAction());
    }

    #endregion
}