using System;
using UnityEngine;
using org.vxwo.csharp.json;
using System.Collections.Generic;
using ExcelConfig;
using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;

public class UAppliaction:XSingleton<UAppliaction>,ExcelConfig.IConfigLoader
{
    public class UnityLoger : XNet.Libs.Utility.Loger
    {
        #region implemented abstract members of Loger
        public override void WriteLog(DebugerLog log)
        {
            switch (log.Type)
            {
                case  LogerType.Error:
                    Debug.LogError(log);
                    break;
                case LogerType.Log:
                    Debug.Log(log);
                    break;
                case LogerType.Waring:
                case LogerType.Debug:
                    Debug.LogWarning(log);
                    Debug.LogWarning(log);
                    break;
            }
           
        }
        #endregion
        
    }

    public string ServerHost;
    public int ServerPort;

    public string GateServerHost;
    public int GateServerPort;

    public GameServerInfo GameServer;
    public long UserID;
    public string SesssionKey;

    public float PingDelay = 0f;
    public static bool IsEditorMode = false;

    #region IConfigLoader implementation

    public List<T> Deserialize<T>() where T : ExcelConfig.JSONConfigBase
    {		
        var name = ExcelConfig.ExcelToJSONConfigManager.GetFileName<T>();
        var json = ResourcesManager.Singleton.LoadText("Json/" + name);
        if (json == null)
            return null;
        return JsonTool.Deserialize<List<T>>(json);	
    }

    #endregion

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        new ExcelConfig.ExcelToJSONConfigManager(this);
        var serverInfo = ResourcesManager.Singleton.LoadText("ServerInfo.json");
        var data = JsonReader.Read(serverInfo);
        ServerHost = data["Host"].AsString();
        ServerPort = data["Port"].AsInt();
        Debuger.Loger = new UnityLoger();
    }

    void Update()
    {
        if (next != null)
        {
            gate = next;
            gate.JoinGate();
            next = null;
           
        }
        if (gate == null)
            return;
        
        gate.Tick();
        #if UNITY_EDITOR
        if(gate!=null)
            GateName = gate.GetType().Name;
        #endif
    }

    void Start()
    {
        if (!IsEditorMode)
        {
            bool auto = false;
            #if !UNITY_EDITOR
            auto = true; 
            #endif
            if (auto && PlayerPrefs.HasKey("_PlayerSession"))
            {
                var session = PlayerPrefs.GetString("_PlayerSession");
                var str = PlayerPrefs.GetString("_UserID");
                var userID = -1L;
                if (!string.IsNullOrEmpty(str))
                {
                    userID = long.Parse(str);
                }
                int port = PlayerPrefs.GetInt("_GateServerPort");
                var host = PlayerPrefs.GetString("_GateServerHost");
                var serverID = PlayerPrefs.GetInt("_GateServerID");
                SesssionKey = session;
                UserID = userID;
                GameServer = new GameServerInfo{ ServerID = serverID, Host = host, Port =port };
                GoToMainGate(GameServer);
           
            }
            else
            {
                GotoLoginGate();
            }

        }
    }

    public void GoBackToMainGate()
    {
        //GameServer = new GameServerInfo{ ServerID =  , Host = host, Port =port };
        GoToMainGate(GameServer);
    }

    public void GoToMainGate(GameServerInfo info)
    {
        var mainGate = new GMainGate(info);
        ChangeGate(mainGate);
        GateServerHost = info.Host;
        GateServerPort = info.Port;
    }

    public void GoServerMainGate(GameServerInfo server, long userID, string session)
    {
        GameServer = server;
        UserID = userID;
        SesssionKey = session;
        GoToMainGate(server);
        PlayerPrefs.SetString("_PlayerSession",session);
        PlayerPrefs.SetString("_UserID", UserID.ToString());
        PlayerPrefs.SetInt("_GateServerPort", server.Port);
        PlayerPrefs.SetString("_GateServerHost", server.Host);
        PlayerPrefs.SetInt("_GateServerID", server.ServerID);
       

    }

    //public RequestClient GServer;

    public void GotoLoginGate()
    {
        ChangeGate(new LoginGate());
    }

    public void GotoBattleGate(GameServerInfo serverInfo,int mapID)
    { 
        var gate = new BattleGate(serverInfo, mapID);
        ChangeGate(gate);
    }

    public void ChangeGate(UGate g)
    {
		
        if (gate != null)
        {
            gate.ExitGate();
        }

        gate = null;
        next = g;
    }

    private UGate next;

    private UGate gate;

    public UGate GetGate()
    {
        return gate;
    }

    public void OnApplicationQuit()
    {
        if (gate != null)
        {
            gate.ExitGate();
        }

        gate = null;
    }

    public string GateName;
}


