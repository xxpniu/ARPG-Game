using UnityEngine;
using System.Collections;
using System.Linq;
using ExcelConfig;


public class EditorStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		tcamera = GameObject.FindObjectOfType<ThridPersionCameraContollor> ();
		UAppliaction.Singleton.ChangeGate(new EditorGate ());
		//style.normal.background
	}


	private ThridPersionCameraContollor tcamera;

	// Update is called once per frame
	void Update () {
		tcamera.forward.y = -1.1f + slider;
		tcamera.Distance = 20 - distance;
		tcamera.rotationY = ry;
	}

	void Awake()
	{
		UAppliaction.Singleton.ChangeGate (null);
	}


	private void ReleaceReleaser()
	{
		var data=	ExcelConfig.ExcelToJSONConfigManager
			.Current.GetConfigByID<CharacterData>(int.Parse(index));
		if (data == null)
			return;
		(UAppliaction.Singleton.GetGate () as EditorGate).ReplaceRelease (data);

	}

	private void ReplaceTarget()
	{
		var data=	ExcelConfig.ExcelToJSONConfigManager
			.Current.GetConfigByID<CharacterData>(int.Parse(index));
		if (data == null)
			return;
		(UAppliaction.Singleton.GetGate () as EditorGate).ReplaceTarget (data);

	}
	private string[] names ;
	private string index =string.Empty;

	private float slider = 1f;
	private float distance = 0f;
	private float ry =0;

	public  GameObject target;

	void OnGUI()
	{
		slider = GUI.VerticalSlider (new Rect (10, 10, 30, 100), slider, 0, 1);
		distance = GUI.HorizontalSlider (new Rect (50, 10, 100, 30), distance, -10, 10 );


		ry = GUI.HorizontalSlider (new Rect (50, 50, 100, 30), ry, 0, 360 );
		#if UNITY_EDITOR
		int h = 30;
		int w = 400;
		GUI.BeginGroup(new Rect(10,Screen.height-h,w,h));
		GUILayout.BeginVertical(GUILayout.Width(w));

		GUILayout.BeginHorizontal();
		index =GUILayout.TextField(index);
		if(GUILayout.Button("替换释放者"))
		{
			ReleaceReleaser();
		}
		if(GUILayout.Button("替换目标"))
		{
			ReplaceTarget();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUI.EndGroup();
		#endif
	}
}
