using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UCharacterView))]
public class UCharacterViewEditor : Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginVertical ();
		if (GUILayout.Button ("同步到当前行为树编辑器")) {
			var target = this.target as UCharacterView;
			var window = EditorWindow.GetWindow<AITreeEditor> ();
			if (window == null)
				return;
			window.AttachRoot (target.GetBattleCharacter ().AIRoot);
		}
		EditorGUILayout.EndVertical ();
		base.OnInspectorGUI ();
	}
}
