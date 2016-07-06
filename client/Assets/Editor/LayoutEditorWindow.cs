using System;
using UnityEditor;
using UnityEngine;

public class LayoutEditorWindow:EditorWindow
{
	public LayoutEditorWindow ()
	{
	}

	[MenuItem("Window/Layout编辑器")]
	public static void Init()
	{
		LayoutEditorWindow window = (LayoutEditorWindow)GetWindow(typeof(LayoutEditorWindow), false, "魔法编辑器");
		//window.position = new Rect(window.position.xMin, window.position.yMin, 700, 400);
		window.minSize = new Vector2 (400, 300);
		window.Show ();
	}

	public static void OpenLayout(string layout)
	{
		Init ();

	}
}


