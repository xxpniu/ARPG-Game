using UnityEngine;
using org.vxwo.csharp.json;
using System.Collections.Generic;
using ExcelConfig;
using Proto;
using XNet.Libs.Utility;
using System.Collections;

/// <summary>
/// 处理 App
/// </summary>
public class UApplication : XSingleton<UApplication>, IConfigLoader
{

    List<T> IConfigLoader.Deserialize<T>()
    {
        var name = ExcelToJSONConfigManager.GetFileName<T>();
        var json = ResourcesManager.S.LoadText("Json/" + name);
        if (json == null)
            return null;
        return JsonTool.Deserialize<List<T>>(json);
    }


    public int ReceiveTotal;
    public int SendTotal;
    public float ConnectTime;

    public string ServerHost;
    public int ServerPort;
    public string ServerName;

    public string GateServerHost;
    public int GateServerPort;
    public int index = 0;
    public GameServerInfo GameServer;
    public string AccountUuid;
    public string SesssionKey;

    public float PingDelay = 0f;
    public static bool IsEditorMode = false;
    
    #region Gate

    public void GetServer()
    {
#if !UNITY_EDITOR
        index =0;
#endif
        SetServer(index);
    }

    public void SetServer(int index)
    {
        var serverInfo = ResourcesManager.Singleton.LoadText("ServerInfo.json");
        var data = JsonReader.Read(serverInfo);
        Debug.Log(serverInfo);

        var server = data["Servers"].GetAt(index);
        ServerHost = server["Host"].AsString();
        ServerPort = server["Port"].AsInt();
        ServerName = server["Name"].AsString();
        Debug.Log(string.Format("{2} {0}:{1}", ServerHost, ServerPort, ServerName));

    }

    public void GoBackToMainGate()
    {
        //GameServer = new GameServerInfo{ ServerID =  , Host = host, Port =port };
        GoToMainGate(GameServer);
    }

    public void GoToMainGate(GameServerInfo info)
    {
        ChangeGate<GMainGate>().Init(info);
        GateServerHost = info.Host;
        GateServerPort = info.Port;
    }

    public void GoServerMainGate(GameServerInfo server, string userID, string session)
    {
        GameServer = server;
        AccountUuid = userID;
        SesssionKey = session;
        GoToMainGate(server);
        PlayerPrefs.SetString("_PlayerSession", session);
        PlayerPrefs.SetString("_UserID", AccountUuid);
        PlayerPrefs.SetInt("_GateServerPort", server.Port);
        PlayerPrefs.SetString("_GateServerHost", server.Host);
        PlayerPrefs.SetInt("_GateServerID", server.ServerId);
    }

    public void GotoLoginGate()
    {
        ChangeGate<LoginGate>();
    }

    public void GotoBattleGate(GameServerInfo serverInfo, int mapID)
    {
        ChangeGate<BattleSimulater>().SetServer(serverInfo, mapID);
    }

    public T ChangeGate<T>() where T : UGate
    {
        if (gate) Destroy(gate);
        return (T)(gate = gameObject.AddComponent<T>());
    }

    private UGate gate;


    #endregion

    #region mono behavior

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _ = new ExcelToJSONConfigManager(this);
        GetServer();
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(RunReader());
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
                var userID = PlayerPrefs.GetString("_UserID");
                int port = PlayerPrefs.GetInt("_GateServerPort");
                var host = PlayerPrefs.GetString("_GateServerHost");
                var serverID = PlayerPrefs.GetInt("_GateServerID");
                SesssionKey = session;
                AccountUuid = userID;
                GameServer = new GameServerInfo { ServerId = serverID, Host = host, Port = port };
                GoToMainGate(GameServer);
            }
            else
            {
                GotoLoginGate();
            }

        }
    }

    public void OnApplicationQuit()
    {
        if (gate) Destroy(gate);
    }

    #endregion

    #region Reader

    private IEnumerator RunReader()
    {
        while (true)
        {
            yield return null;
            if (NotifyMessages.Count > 0)
            {
                UUITipDrawer.Singleton.ShowNotify(NotifyMessages.Dequeue());
                yield return new WaitForSeconds(.8f);
            }
        }
    }

    public void ShowError(ErrorCode code)
    {
        ShowNotify("ErrorCode:" + code);
    }

    public void ShowNotify(string notify)
    {
        NotifyMessages.Enqueue(notify);
    }

    private Queue<string> NotifyMessages { get; } = new Queue<string>();

    #endregion


    public static T G<T>() where T : UGate { return S.gate as T; }
}


