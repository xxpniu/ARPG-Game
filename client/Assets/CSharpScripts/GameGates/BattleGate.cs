using System;
using Proto;
using UnityEngine;
using XNet.Libs.Net;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using UnityEngine.SceneManagement;


public class BattleGate:UGate
{
    
    public BattleGate(GameServerInfo serverInfo,int mapID)
    {
        ServerInfo = serverInfo;
        MapID = mapID;
        MapConfig = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MapData>(MapID);
        player.OnCreateUser = (notify, view) =>
        {
            if (UAppliaction.Singleton.UserID == notify.UserID)
            {
                ThridPersionCameraContollor.Singleton.lookAt = view.transform;
                UUIManager.Singleton.ShowMask(false);
            }
        };
    }

    private ExcelConfig.MapData MapConfig;

    public void ProcessNotify(ISerializerable notify)
    {
        player.Process(notify);
    }

    private NotifyPlayer player = new NotifyPlayer();
   
    private GameServerInfo ServerInfo;
    private int MapID;
    public RequestClient Client{ set; get; }

    #region implemented abstract members of UGate

    public override void JoinGate()
    {
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowMask(true);
        Operation = SceneManager.LoadSceneAsync(MapConfig.LevelName);
    }

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
        UUITipDrawer.Singleton.ShowNotify("Can't login BattleServer!");
        UAppliaction.Singleton.GoBackToMainGate();  
    }

    public override void Tick()
    {

        if (Operation != null)
        {
            if (Operation.isDone)
            {
                Operation = null;
                Client = new RequestClient(ServerInfo.Host, ServerInfo.Port);
                Client.RegAssembly(this.GetType().Assembly);
                Client.RegisterHandler(MessageClass.Notify, new BattleNotifyHandler(this));
                Client.OnConnectCompleted +=(s,e)=>{
                    UAppliaction.Singleton.ConnectTime = Time.time;
                    if(e.Success)
                    {
                        var request = Client.CreateRequest<C2B_JoinBattle,B2C_JoinBattle>();
                        request.RequestMessage.Session= UAppliaction.Singleton.SesssionKey;
                        request.RequestMessage.UserID = UAppliaction.Singleton.UserID;
                        request.RequestMessage.Version = ProtoTool.GetVersion();
                        request.RequestMessage.MapID = MapID;
                        request.OnCompleted =(success,response)=>{
                            // UUITipDrawer.Singleton.ShowNotify("BattleServer:"+response.Code);
                            if(response.Code != ErrorCode.OK){
                                UUITipDrawer.Singleton.ShowNotify("BattleServer:"+response.Code);
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

    public override EngineCore.Simulater.GTime GetTime()
    {
        return new EngineCore.Simulater.GTime(Time.time, Time.deltaTime);
    }

    #endregion
}