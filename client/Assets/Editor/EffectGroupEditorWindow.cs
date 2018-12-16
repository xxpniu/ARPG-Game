using System;
using UnityEditor;
using UnityEngine;
using Layout;
using System.Collections.Generic;
using Layout.LayoutEffects;
using Layout.EditorAttributes;

public class EffectGroupEditorWindow:EditorWindow
{
	static EffectGroupEditorWindow ()
	{
		var list = new List<string> ();
		var types = typeof(EffectBase).Assembly.GetTypes ();
		foreach (var i in types) {
			if(i.IsSubclassOf(typeof(EffectBase)))
			{
				var attrs = i.GetCustomAttributes (typeof(EditorEffectAttribute), false) as EditorEffectAttribute[];
				if (attrs.Length == 0)
					continue;
				_effects.Add(i);
				list.Add (attrs [0].Name);
			}
		}
		_effectNames = list.ToArray ();
	}


	private static List<Type> _effects =  new List<Type>();
	private static string[] _effectNames ;


	public static void ShowEffectGroup(List<EffectBase> group)
	{
		//Debug.Log (group);
        var window =(EffectGroupEditorWindow) GetWindow (typeof(EffectGroupEditorWindow),false, "编辑效果组");

		window.groupData = group;
		window.minSize = new Vector2 (450, 250);
		window.maxSize = window.minSize;
		window.position = new Rect( new Vector2 (Screen.width / 2 - 225, Screen.height / 2 -125),window.maxSize);
		//window.Show (false);
	}

	int index =-1;
	Vector2 _scroll = Vector2.zero;
	Vector2 scrollProperty = Vector2.zero;
	void OnGUI()
	{
        Repaint();
		if (groupData == null)
			return;
		var leftWidth = 200;
		var height = 50;
		var color = Color.black;
		var topHeight = 25;

		var rectLeft = new Rect (0, topHeight, position.width - leftWidth, position.height-topHeight);
		//var rectRight = new Rect (rectLeft.width , 0, leftWidth , position.height);

        GUILayout.BeginArea (new Rect (0, 0, rectLeft.width, topHeight));
		GUILayout.BeginHorizontal(GUILayout.Height(topHeight),GUILayout.Width(rectLeft.width));

		index = EditorGUILayout.Popup (index, _effectNames);
		if (GUILayout.Button ("Create", GUILayout.Width (70))) {
			if (index >= 0) {
				var t = _effects [index];
				var effect = EffectBase.CreateInstance (t);
				groupData.Add (effect);
			}
		}
		GUILayout.EndHorizontal();
        GUILayout.EndArea();

		var re = new Rect (rectLeft);
		re.width = re.width + 20;
		_scroll=GUI.BeginScrollView (re, _scroll, new Rect(0,0,rectLeft.width,groupData.Count * 50));
		float h = 0;
		foreach (var i in groupData) {
			var attrs = i.GetType ().GetCustomAttributes (typeof(EditorEffectAttribute), false) as EditorEffectAttribute[];
			var name = i.GetType ().Name;
			if (attrs.Length > 0)
				name = attrs [0].Name;

			var rect = new Rect (0, h, rectLeft.width, height);
			GUI.Label (new Rect (10, h+5, 200, 16), name);
			GUI.Label (new Rect (10, h+20, 200, 16), i.ToString());
			GLDraw.DrawBox (rect, color, 1);
			if (current == i) {
				GLDraw.DrawBox (new Rect (rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4), Color.green, 2);
			} else {
				if (Event.current.type == UnityEngine.EventType.MouseDown) {
					if (rect.Contains (Event.current.mousePosition)) {
						current = i;
						Event.current.Use ();
					}
				}
			}

			if (Event.current.type == UnityEngine.EventType.ContextClick) {
				if (rect.Contains (Event.current.mousePosition)) {
					GenericMenu m = new GenericMenu ();
					m.AddItem (new GUIContent ("Delete"), false, DeleteThis, i);
					m.ShowAsContext ();
					//Event.current.Use ();
				}
			}

			h += height;
		}

		GUI.EndScrollView ();


        GUILayout.BeginArea (new Rect (position.width - leftWidth+16, 0, leftWidth-20, position.height));
		GUILayout.BeginVertical ();
		GUILayout.Label ("Property", GUILayout.Height (20));
		scrollProperty = GUILayout.BeginScrollView (scrollProperty);
		GUILayout.BeginVertical ();
		if (current != null)
			PropertyDrawer.DrawObject (current,"EFFECT");
		GUILayout.EndVertical ();
		GUILayout.EndScrollView ();
		GUILayout.EndVertical ();
        GUILayout.EndArea ();

		GLDraw.DrawLine (new Vector2 (rectLeft.width, 0), new Vector2 (rectLeft.width, position.height), color, 1);
	}
	private List<EffectBase> groupData;
	private object current;
	private void DeleteThis(object user)
	{
		var u = user as EffectBase;
		groupData.Remove (u);
	}
}
