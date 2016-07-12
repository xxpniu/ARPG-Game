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
		tcamera.forward.y = -1.08f + slider;
		tcamera.Distance = 22 - distance;
		tcamera.rotationY = ry;

		var midd = tcamera.lookAt;

		var gate = (UAppliaction.Singleton.GetGate () as EditorGate);
		if (gate != null &&isChanged) {
			var position = midd.position;
			var left = position  + (Vector3.left * distanceCharacter/2);
			var right = position + (Vector3.right * distanceCharacter / 2);

			gate.releaser.View.SetPosition (GTransform.Convent (left));
			gate.target.View.SetPosition (GTransform.Convent (right));
			isChanged = false;

		}
	}

	void Awake()
	{
		UAppliaction.Singleton.ChangeGate (null);
	}

	private bool isChanged = false;


	private void ReleaceReleaser(bool stay)
	{
		var data=	ExcelConfig.ExcelToJSONConfigManager
			.Current.GetConfigByID<CharacterData>(int.Parse(index));
		if (data == null)
			return;
		(UAppliaction.Singleton.GetGate () as EditorGate).ReplaceRelease (data,stay);

	}

	private void ReplaceTarget(bool stay)
	{
		var data=	ExcelConfig.ExcelToJSONConfigManager
			.Current.GetConfigByID<CharacterData>(int.Parse(index));
		if (data == null)
			return;
		(UAppliaction.Singleton.GetGate () as EditorGate).ReplaceTarget (data,stay);

	}
	private string[] names ;
	private string index =string.Empty;

	private float slider = 1f;
	private float distance = -5f;
	private float ry =0;
	private float distanceCharacter = 10;

	public  GameObject target;
	public bool stay = false;

	void OnGUI()
	{
		slider = GUI.VerticalSlider (new Rect (10, 10, 30, 100), slider, 0, 1);
		distance = GUI.HorizontalSlider (new Rect (50, 10, 100, 30), distance, -10, 20 );
		ry = GUI.HorizontalSlider (new Rect (50, 35, 100, 30), ry, 0, 180 );
		float last = distanceCharacter;
		distanceCharacter= GUI.HorizontalSlider (new Rect (50, 70, 100, 30), distanceCharacter, -10, 20 );
		if (last != distanceCharacter)
			isChanged = true;
		#if UNITY_EDITOR
		int h = 30;
		int w = 230;
		GUI.BeginGroup(new Rect(10,Screen.height-h,w,h));
		GUILayout.BeginVertical(GUILayout.Width(w));

		GUILayout.BeginHorizontal();
		index =GUILayout.TextField(index);
		if(GUILayout.Button("释放者"))
		{
			ReleaceReleaser(stay);
		}
		if(GUILayout.Button("目标"))
		{
			ReplaceTarget(stay);
		}

		stay= GUILayout.Toggle(stay,"保留");
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUI.EndGroup();
		#endif
	}
}
