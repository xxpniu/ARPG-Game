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

        Operation = SceneManager.LoadSceneAsync(MapConfig.LevelName);
    }

    private AsyncOperation Operation;

    public override void ExitGate()
    {
        if (Client != null)
        {
            Client.Disconnect();
        }
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
                            UUITipDrawer.Singleton.ShowNotify("BattleServer:"+response.Code);
                        };
                        request.SendRequest();
                    }
                };
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