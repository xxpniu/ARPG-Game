using System;
using Proto;
using UnityEngine;
using XNet.Libs.Net;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UGameTools;
using EConfig;
using Google.Protobuf;
using Proto.BattleServerService;

public class BattleGate:UGate
{

    public class NotifyHandle : ServerMessageHandler
    {
        private BattleGate battleGate;

        public NotifyHandle(BattleGate battleGate)
        {
            this.battleGate = battleGate;
        }

        public override void Handle(Message message)
        {
            battleGate.ProcessNotify(message.AsNotify());
        }
    }

    public BattleGate(GameServerInfo serverInfo,int mapID)
    {
        ServerInfo = serverInfo;
        MapID = mapID;
        MapConfig = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<MapData>(MapID);
        player.OnCreateUser = (view) =>
        {
            var character = view as UCharacterView;
            if (UApplication.S.AccountUuid == character.AccoundUuid)
            {
                ThridPersionCameraContollor.Singleton.lookAt = character.GetBoneByName("Bottom");
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

    private NotifyPlayer player = new NotifyPlayer();
   
    private GameServerInfo ServerInfo;
    private int MapID;
    public RequestClient<EmptyTaskHandle> Client{ set; get; }

    private bool IsInit = false;

    #region implemented abstract members of UGate

    public override void JoinGate()
    {
        start = Time.time;
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);
        var ui = UUIManager.Singleton.CreateWindow<Windows.UUIBattle>();
        ui.ShowWindow();
    }


    private float start = 0f;
    private AsyncOperation Operation;

    public override void ExitGate()
    {
        if (Client != null)
        {
            Client.OnDisconnect -= OnDisconnect;
            Client.Disconnect();
        }
        UUIManager.Singleton.ShowMask(false);
    }

    private void OnDisconnect(object sender, EventArgs e)
    {
        //UUITipDrawer.Singleton.ShowNotify("Can't login BattleServer!");
        UApplication.S.GoBackToMainGate();  
    }

    public override void Tick()
    {
        if (!IsInit)
        {
            if (Time.time - start < 0.1f) return;
            IsInit = true;
            Operation = SceneManager.LoadSceneAsync(MapConfig.LevelName);
        }
        if (Operation != null)
        {
            if (Operation.isDone)
            {
                Operation = null;
                Client = new RequestClient<EmptyTaskHandle>(ServerInfo.Host, ServerInfo.Port);
                Client.RegisterHandler(MessageClass.Notify, new NotifyHandle(this));
                Client.OnConnectCompleted += (s, e) =>
                {
                    UApplication.Singleton.ConnectTime = Time.time;
                    if (e.Success)
                    {
                        _ = JoinBattle.CreateQuery()
                        .SetCallBack((r) => {
                            UUITipDrawer.Singleton.ShowNotify("BattleServer:" + r.Code);
                            UApplication.Singleton.GoBackToMainGate();
                        })
                        .SendRequestAsync(Client, new C2B_JoinBattle
                        {
                            Session = UApplication.S.SesssionKey,
                            AccountUuid = UApplication.S.AccountUuid,
                            MapID = MapID,
                            Version = 1
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
        }

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