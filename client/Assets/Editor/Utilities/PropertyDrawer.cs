using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Layout.EditorAttributes;
using EngineCore;
using Object = UnityEngine.Object;
using EventType = UnityEngine.EventType;
using Layout.LayoutElements;
using System.IO;
using Layout;
using Layout.LayoutEffects;
using System.Linq;

public class DrawerHandlerAttribute:Attribute
{
	public DrawerHandlerAttribute(Type p)
	{
		HandleType = p;
	}

	public Type HandleType{ set; get; }
}

public class PropertyDrawer
{

	static PropertyDrawer()
	{
		
	}

	private static void Init()
	{
		_handlers = new Dictionary<Type, MethodInfo> ();
		var type = typeof(PropertyDrawer);
		var methods = type.GetMethods ();
		foreach (var i in methods) {
			var atts = i.GetCustomAttributes (typeof(DrawerHandlerAttribute), false) as DrawerHandlerAttribute[];
			if (atts == null || atts.Length == 0)
				continue;
			_handlers.Add (atts [0].HandleType, i);
		}

		var att = typeof(UCharacterView).GetCustomAttributes(typeof(BoneNameAttribute),false) as BoneNameAttribute[];
		List<string> tnames = new List<string> ();
		foreach (var i in att) 
		{
			tnames.Add (i.Name);
			//tbones.Add (i.BoneName);
		}
		//bones = tbones.ToArray();
		names = tnames.ToArray ();
	}


	//private static string[] bones;
	private static string[] names;

	private static Dictionary<Type,MethodInfo> _handlers;


	public static void DrawObject(object obj)
	{
		if (_handlers == null) {
			Init (); 
		}
		var fields= obj.GetType ().GetFields ();

		foreach (var i in fields) {
			DrawProperty (i, obj);
		}
	}


	private static void DrawProperty(FieldInfo field,object obj)
	{
		 
		var hide = field.GetCustomAttributes (typeof(HideInEditorAttribute), false) as  HideInEditorAttribute[];
		if (hide.Length > 0)
			return;
		
		var label = field.GetCustomAttributes (typeof(LabelAttribute), false) as  LabelAttribute[];
		var name = field.Name;
		if (label.Length > 0) {
			name = label [0].DisplayName;
		}

		foreach (var i in _handlers) {
			var attrs = field.GetCustomAttributes (i.Key, false);
			if (attrs.Length > 0) {
				i.Value.Invoke (null, new object[]{ obj,field,name , attrs[0] });
				return;
			}
		}
	
		//GUILayout.BeginVertical ();

		if (field.FieldType == typeof(int)) {
			GUILayout.Label (name);
			var value = EditorGUILayout.IntField ((int)field.GetValue (obj));
			field.SetValue (obj, value);
        } else if (field.FieldType == typeof(bool)) {
            GUILayout.Label (name);
            var value = EditorGUILayout.Toggle ((bool)field.GetValue (obj));
            field.SetValue (obj, value);
        }else if (field.FieldType == typeof(string)) {
			GUILayout.Label (name);
			var value = EditorGUILayout.TextField ((string)field.GetValue (obj));
			field.SetValue (obj, value);
		} else if (field.FieldType == typeof(long)) {
			GUILayout.Label (name);
			var value = EditorGUILayout.LongField ((long)field.GetValue (obj));
			field.SetValue (obj, value);
		} else if (field.FieldType == typeof(float)) {
			GUILayout.Label (name);
			var value = EditorGUILayout.FloatField ((float)field.GetValue (obj));
			field.SetValue (obj, value);
		} else if (field.FieldType == typeof(Layout.Vector3)) {
			//GUILayout.Label (name);
			var v = (Layout.Vector3)field.GetValue (obj);
			var value = EditorGUILayout.Vector3Field(name,new UnityEngine.Vector3 (v.x, v.y, v.z));
			field.SetValue (obj, new Layout.Vector3 { x= value.x, y=value.y, z=value.z});
		} else if (field.FieldType.IsEnum) {
			GUILayout.Label (name);
			var value = EditorGUILayout.EnumPopup ((Enum)field.GetValue (obj));
			field.SetValue (obj, value);
		}


		//GUILayout.EndVertical ();
	}

	[DrawerHandlerAttribute(typeof(EditorResourcePathAttribute))]
	public static void ResourcesSelect(object obj,FieldInfo field, string label, object attr)
	{
		//var eAtt = attr as EditorResourcePathAttribute;
		var resources = "Assets/Resources/";
		var path = (string)field.GetValue (obj);

		var res = AssetDatabase.LoadAssetAtPath<UnityEngine.Object> (resources + path);
		GUILayout.Label (label);
		res= EditorGUILayout.ObjectField (res,typeof(Object), false);
		path = AssetDatabase.GetAssetPath (res);
		path = path.Replace (resources, "");
		field.SetValue (obj, path);

	}
	[DrawerHandlerAttribute(typeof(LayoutPathAttribute))]
	public static void LayoutPathSelect(object obj, FieldInfo field,string label, object attr)
	{
		var resources = "Assets/Resources/";
		var path = (string)field.GetValue (obj);
		var res = AssetDatabase.LoadAssetAtPath<UnityEngine.TextAsset> (resources + path);
		//GUILayout.BeginVertical ();
		GUILayout.Label (label);

		GUILayout.BeginHorizontal ();
		res= EditorGUILayout.ObjectField (res,typeof(TextAsset), false,GUILayout.Width(100)) as TextAsset;
		path = AssetDatabase.GetAssetPath (res);
		path = path.Replace (resources, "");
		field.SetValue (obj, path);

		if (GUILayout.Button ("New")) {
			var fPath = EditorUtility.SaveFilePanel ("新建Layout",Application.dataPath+"/Resources/", "layout", "xml");
			if (!string.IsNullOrEmpty (fPath)) {
				path = fPath.Replace (Application.dataPath+"/Resources/", "");
				var layout = new TimeLine ();
				var xml = XmlParser.Serialize (layout);
				File.WriteAllText (fPath, xml, XmlParser.UTF8);
				AssetDatabase.Refresh ();
				field.SetValue (obj, path);
			}
		}

		if (!string.IsNullOrEmpty (path)) {
			if (GUILayout.Button ("Open")) {
				//Open layout window
				LayoutEditorWindow.OpenLayout (path);
			}
		}
		GUILayout.EndHorizontal ();

		//GUILayout.EndVertical ();
	}

	[DrawerHandlerAttribute(typeof(EditorEffectsAttribute))]
	public static void EffectGroupSelect(object obj, FieldInfo field,string label, object attr)
	{
		//GUILayout.BeginVertical ();
		if (GUILayout.Button ("Open Effect Group",GUILayout.MinWidth(150))) {
			var effectGroup = field.GetValue(obj) as List<EffectBase>; 
			EffectGroupEditorWindow.ShowEffectGroup (effectGroup);
		}
		//GUILayout.EndVertical ();
	}

	private static int indexOfBone=-1;
	//EditorBoneAttribute
	[DrawerHandlerAttribute(typeof(EditorBoneAttribute))]
	public static void BoneSelected(object obj, FieldInfo field,string label, object attr)
	{
		var boneName = (string)field.GetValue (obj);
		indexOfBone = -1;
		for (var i = 0; i < names.Length; i++) {
			if (names [i] == boneName) {
				indexOfBone = i;
				break;
			}
		}

		GUILayout.Label (label);
		indexOfBone = EditorGUILayout.Popup (indexOfBone, names);
		if(indexOfBone>=0 && indexOfBone<names.Length)
		{
			field.SetValue (obj, names [indexOfBone]);
	    }
	}
}


