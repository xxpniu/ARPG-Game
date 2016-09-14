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


public class BattleGate:UGate
{
    
    public BattleGate(GameServerInfo serverInfo,int mapID)
    {
        ServerInfo = serverInfo;
        MapID = mapID;
        MapConfig = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MapData>(MapID);
        player.OnCreateUser = (view) =>
        {
            if (UAppliaction.Singleton.UserID == view.UserID)
            {
                ThridPersionCameraContollor.Singleton.lookAt = view.GetBoneByName("Bottom");
                //ThridPersionCameraContollor.Singleton.forwardTrans = view.Character.transform;
                UUIManager.Singleton.ShowMask(false);
                var ui = UUIManager.Singleton.GetUIWindow<Windows.UUIBattle>();
                ui.InitCharacter(view);
            }
        };
        player.OnDeath = (view) =>
        {
            if (UAppliaction.Singleton.UserID == view.UserID)
            {
                //Go to Main
                //dead
            }
        };

        player.OnJoined = (initPack) =>
        {
            if (UAppliaction.Singleton.UserID == initPack.UserID)
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

    private ExcelConfig.MapData MapConfig;

    public void ProcessNotify(ISerializerable notify)
    {
        player.Process(notify);
    }

    private NotifyPlayer player = new NotifyPlayer();
   
    private GameServerInfo ServerInfo;
    private int MapID;
    public RequestClient Client{ set; get; }

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
        UAppliaction.Singleton.GoBackToMainGate();  
    }

    public override void Tick()
    {
        if (!IsInit)
        {
            if (Time.time - start < 0.1f)
                return;
            IsInit = true;
            Operation = SceneManager.LoadSceneAsync(MapConfig.LevelName);
        }
        if (Operation != null)
        {
            if (Operation.isDone)
            {
                Operation = null;
                Client = new RequestClient(ServerInfo.Host, ServerInfo.Port);
                Client.RegAssembly(this.GetType().Assembly);
                Client.RegisterHandler(MessageClass.Notify, new BattleNotifyHandler(this));
                Client.OnConnectCompleted += (s, e) =>
                {
                    UAppliaction.Singleton.ConnectTime = Time.time;
                    if (e.Success)
                    {
                        var request = Client.CreateRequest<C2B_JoinBattle,B2C_JoinBattle>();
                        request.RequestMessage.Session = UAppliaction.Singleton.SesssionKey;
                        request.RequestMessage.UserID = UAppliaction.Singleton.UserID;
                        request.RequestMessage.Version = ProtoTool.GetVersion();
                        request.RequestMessage.MapID = MapID;
                        request.OnCompleted = (success, response) =>
                        {
                            // UUITipDrawer.Singleton.ShowNotify("BattleServer:"+response.Code);
                            if (response.Code != ErrorCode.OK)
                            {
                                UUITipDrawer.Singleton.ShowNotify("BattleServer:" + response.Code);
                                UAppliaction.Singleton.GoBackToMainGate();
                            }
                        };
                        request.SendRequest();
                    }
                    else
                    {
                        UUITipDrawer.Singleton.ShowNotify("Can't login BattleServer!");
                        UAppliaction.Singleton.GoBackToMainGate();
                    }
                };
                Client.OnDisconnect += OnDisconnect; 
                Client.Connect();
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

   

    public override void OnTap(TapGesture gesutre)
    {
        var ray = Camera.main.ScreenPointToRay(gesutre.Position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.tag == AstarGridBase.GROUND)
            {
                var message = new Proto.Action_ClickMapGround
                { 
                        TargetPosition = hit.point.ToPVer3()
                };
                SendAction(message);
            }
        }
    }


    public void SendAction(Proto.ISerializerable action)
    {
        Client.SendMessage(RequestClient.ToMessage(MessageClass.Action, action));
    }

    #endregion
}