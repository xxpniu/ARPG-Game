using System;
using UnityEditor;
using UnityEngine;
using Layout;
using System.Collections.Generic;
using Layout.LayoutEffects;

public class EffectGroupEditorWindow:EditorWindow
{
	public EffectGroupEditorWindow ()
	{
		
	}

	public static void ShowEffectGroup(List<EffectBase> group)
	{
		Debug.Log (group);
	}
}
