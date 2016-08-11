using UnityEngine;
using System.Collections;
using System.IO;

public class GameGMTools : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		var data =PlayerPrefs.GetString ("GM");
		if(!string.IsNullOrEmpty(data))
		  level = data; 
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private string level = "level 1";

	public void OnGUI ()
	{
		GUI.Box (new Rect (Screen.width - 200, Screen.height - 50, 195, 50), "GM Tools");
        GUI.Label(new Rect(Screen.width- 120,5, 120,20),string.Format("FPS:{0:0}P:{1:0}", 1/Time.deltaTime, UAppliaction.Singleton.PingDelay));
		GUI.BeginGroup (new Rect (Screen.width - 195, Screen.height - 30, 185, 25));

		GUILayout.BeginHorizontal ();
		level = GUILayout.TextField (level, GUILayout.Width (120));
		if (GUILayout.Button ("GM",GUILayout.Width (60))) {
			StartGM (level);
		}
		GUILayout.EndHorizontal ();
		GUI.EndGroup ();
	}

	private void StartGM(string gm)
	{

		var args = gm.Split (' ');
		if (args.Length >= 2) {
			switch (args [0].ToLower()) {
                case "level":
                    {
                        var gate = new UGameGate(int.Parse(args[1]));
                        UAppliaction.Singleton.ChangeGate(gate);
                    }
				break;
                case "replay":
                    {
                        var data = File.ReadAllBytes(Path.Combine(Application.dataPath, "replay.data"));
                        var gate = new UReplayGate(data, 1);
                        UAppliaction.Singleton.ChangeGate(gate);
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
