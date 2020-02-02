using UnityEngine;
using System.Collections;
using System.IO;
using Proto;
using System.Reflection;
using Proto.GateServerService;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameGMTools : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		var data =PlayerPrefs.GetString ("GM");
		if(!string.IsNullOrEmpty(data))
		  level = data; 
        green.alignment = TextAnchor.MiddleRight;
        green.normal.textColor = Color.green;
        red.alignment = TextAnchor.MiddleRight;
        red.normal.textColor = Color.red;
	}
	

	// Update is called once per frame
	void Update ()
	{
        #if UNITY_EDITOR


        //Send Update

        #endif
	}

	private string level = "level 1";
    public bool ShowGM = false;
    GUIStyle red = new GUIStyle();
    GUIStyle green = new GUIStyle();

	public void OnGUI ()
	{
        GUI.Label(
            new Rect(Screen.width- 220,5, 200,40),
            string.Format("FPS:{0:0}P:{1:0}\nS:{2:0.00}kb/s R:{3:0.00}kb/s(AVG)", 
                1/Time.deltaTime,
                UApplication.Singleton.PingDelay,
                (UApplication.Singleton.SendTotal/1024.0f)/Mathf.Max(1,Time.time - UApplication.Singleton.ConnectTime),
                (UApplication.Singleton.ReceiveTotal/1024.0f)/Mathf.Max(1,Time.time - UApplication.Singleton.ConnectTime)),
            1/Time.deltaTime>28?green:red);

        if (!ShowGM)
            return;
		GUI.BeginGroup (new Rect (Screen.width - 185, 55, 180, 25));
		GUILayout.BeginHorizontal ();
        int[] indexs = new int[]{ 0, 1, 2 };
        foreach (var i in indexs)
        {
            if(GUILayout.Button(string.Format("{0}",i)))
            {
                UApplication.S.SetServer(i);
                UApplication.S.GotoLoginGate();
            }
        }
        level = GUILayout.TextField (level,GUILayout.Width(100));
        if (GUILayout.Button("GM", GUILayout.Width(60)))
        {
            //StartGM (level);
            var gata = UApplication.S.G<GMainGate>();
            if (gata == null)
                return;
            GMTool.CreateQuery()
                .SetCallBack(r=> {
                    Debug.Log("GMResult:" + r.Code);
                    PlayerPrefs.SetString("GM", level);
                })
                .SendRequestAsync(gata.Client, new C2G_GMTool
            {
                GMCommand = level
            });
        }
		GUILayout.EndHorizontal ();
		GUI.EndGroup ();
	}

	private void StartGM(string gm)
	{

		var args = gm.Split (' ');
		if (args.Length >= 2) {
			switch (args [0].ToLower()) 
            {
                case "replay":
                    {
                        var data = File.ReadAllBytes(Path.Combine(Application.dataPath, "replay.data"));
                        var gate = new UReplayGate(data, 1);
                        UApplication.Singleton.ChangeGate(gate);
                    }
                    break;
                case "server":
                    {
                        var index =  int.Parse (args[1]);
                        UApplication.Singleton.index = index;
                        UApplication.Singleton.GetServer();
                        UApplication.Singleton.GotoLoginGate();
                    }
                    break;
			default:
				return;
				//break;
            }
		}

		PlayerPrefs.SetString ("GM", gm);
	}
        
}
