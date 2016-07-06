using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml;
using Layout;
using System.Collections.Generic;
using System.IO;

public class MagicEditorWindow : EditorWindow 
{
	public class DeleteEffectGroup
	{
		public EventContainer Container;
		public EffectGroup egroup;
	}
    
	private MagicData data;    

	[MenuItem("Window/魔法编辑器")]
	public static void Init()
	{
		
		MagicEditorWindow window = (MagicEditorWindow)GetWindow(typeof(MagicEditorWindow),false, "魔法编辑器");
		window.position = new Rect(window.position.xMin, window.position.yMin, 700, 400);
		window.minSize = new Vector2 (400, 300);
		window.Show ();
		//window.TestData();
		//window.Show();
	}

	private Vector2 _scroll =Vector2.zero;
	private Rect view = new Rect(0,0,1000,1000);
	//Dictionary<int, Rect> _rects = new Dictionary<int, Rect> ();


	void OnGUI()
	{
		var group = new Rect (5, position.height - 50, 300, 20);
		GUI.Box (new Rect(3,position.height-70 ,276,50),"编辑操作");
		GUI.BeginGroup (group);


		GUILayout.BeginHorizontal (GUILayout.Width(300));

		if (GUILayout.Button ("测试",GUILayout.Width(50))) {
			//release
		}

		if (GUILayout.Button ("新建",GUILayout.Width(50))) {
			New ();
		}
		if (GUILayout.Button ("打开",GUILayout.Width(50))) {
			Open ();
		}

		if (GUILayout.Button ("保存",GUILayout.Width(50))) {
			Save (data);
		}

		if (GUILayout.Button ("另存",GUILayout.Width(50))) {
			SaveAs (data);
		}
		GUILayout.EndHorizontal ();
		GUI.EndGroup ();

		float lS = 200;
		if (data == null)
			return;
		
		_scroll = GUI.BeginScrollView(new Rect(0, 0, position.width - lS, position.height), _scroll,view);

		var currentView = new Rect (_scroll.x, _scroll.y, position.width - lS, position.height);
		BeginWindows ();

		Color color = Color.black;
		var offsetX = 50;
		var offsetY = 10;
		var sizeX = 200;
		var sizeY = 60;
		var sizeYBase = 100;

		Vector2 cOffset = new Vector2 (offsetX+sizeX+offsetX, offsetY);

		Vector2 eOffset = new Vector2 (cOffset.x + sizeX + offsetX, offsetY);
		float maxY = 0;
		int indexC = 100;
		List<Vector2> cEndPoint = new List<Vector2> ();
		foreach (var i in data.Containers) 
		{
			int indexE = indexC * 100;;
			var oldOffset = eOffset;
			var listEndPoint = new List<Vector2> ();
			foreach (var e in i.effectGroup) 
			{
				var eRect = new Rect (eOffset, new Vector2 (sizeX, sizeY));
				if (Event.current.type == UnityEngine.EventType.ContextClick) 
				{
					if (eRect.Contains (Event.current.mousePosition)) {
						GenericMenu m = new GenericMenu ();
						m.AddItem (new GUIContent ("删除"), false, DeleteEffectGroupDe, 
							new DeleteEffectGroup{ egroup= e,Container = i} );
						m.AddSeparator ("");
						m.AddItem (new GUIContent ("查看效果"), false, ShowEffectGroup, e);
						m.ShowAsContext ();
						Event.current.Use ();
					}
				}
				if (DrawWindow (indexE, eRect, EffectWindow, e.key)) {
					ShowObject (e);
				}
				listEndPoint.Add ( new Vector2(eOffset.x,eOffset.y+5));
				eOffset = new Vector2 (eOffset.x, eOffset.y + offsetY + sizeY);
				indexE++;
			}

			var center = oldOffset.y+ ((eOffset.y - oldOffset.y) / 2)  ;
			if (maxY < center) {
				maxY = center;
			}
			var cRect = new Rect (new Vector2 (cOffset.x, maxY), new Vector2 (sizeX, sizeY));
			cEndPoint.Add (new Vector2 (cOffset.x, maxY+5));
			if (DrawWindow (indexC, cRect,
				   EventWindow, i.type.ToString ())) {
				ShowObject (i);
			}

			if (Event.current.type == UnityEngine.EventType.ContextClick) 
			{
				if (cRect.Contains (Event.current.mousePosition)) 
				{
					GenericMenu m = new GenericMenu ();
					m.AddItem (new GUIContent ("删除"),false,DeleteEvent, i);
					m.AddSeparator ("");
					m.AddItem (new GUIContent ("添加效果组"),false,AddEffectGroup, i);
					if(!string.IsNullOrEmpty(i.layoutPath))
					m.AddItem (new GUIContent ("打开Layout"), false, OpenLayout, i);
					m.ShowAsContext ();
					Event.current.Use ();
				}
			}
			//ContectMenu

			var start = new Vector2 (cOffset.x+sizeX, maxY +sizeY/2);
			foreach(var p in listEndPoint)
			{
				if(currentView.Contains(start)&&currentView.Contains(p))
				GLDraw.DrawConnectingCurve (start, p, color, 1);
			}
			maxY += offsetY + sizeY;
			indexC++;
		}

	
		var rectBase =new  Rect (new Vector2 (offsetX, offsetY+maxY / 2), new Vector2 (sizeX, sizeYBase));
		if (Event.current.type == UnityEngine.EventType.ContextClick) 
		{
			if (rectBase.Contains (Event.current.mousePosition)) 
			{
				GenericMenu m = new GenericMenu ();
				m.AddItem (new GUIContent ("添加事件"),false,AddEvent, data);
				m.AddSeparator ("");
				m.AddItem (new GUIContent ("保存"),false,Save, data);
				m.AddItem (new GUIContent ("另存为"),false,SaveAs, data);
				m.ShowAsContext ();
				Event.current.Use ();
			}
		}

		if (DrawWindow (0, 
			   rectBase,
			   MagicWindow, data.name)) {
			ShowObject (data);
		}
		// content




		var startBase = new Vector2 (offsetX + sizeX, offsetY+ maxY / 2+sizeYBase / 2);
		foreach (var p in cEndPoint) {
			if(currentView.Contains(startBase)&&currentView.Contains(p))
			GLDraw.DrawConnectingCurve (startBase, p, color, 1);
		}

		EndWindows ();
		GUI.EndScrollView ();



		view = new Rect (0, 0, eOffset.x + sizeX+offsetX, Mathf.Max(maxY, eOffset.y));

		GLDraw.DrawLine (new Vector2 (position.width - lS, 0), new Vector2 (position.width - lS, position.height), color, 1);
		GUI.BeginGroup (new Rect(position.width - lS,0,lS,position.height));
		GUILayout.BeginVertical(GUILayout.Width(lS-2));
		GUILayout.Label ("属性详情");

		if (currentObj != null)
			PropertyDrawer.DrawObject (currentObj);

		GUILayout.EndVertical ();
		GUI.EndGroup ();



	}


	private void New()
	{
		if (data != null) {
			if (!EditorUtility.DisplayDialog ("放弃保存", "当前编辑不为空，新建将放弃保存!", "放弃", "取消"))
				return;
		}

		currentPath = null;
		data = new MagicData (){key="new_magic",name="新建魔法"};
		data.Containers.Add (new EventContainer (){ type = Layout.EventType.EVENT_START});
	}


	private void Open()
	{
		if (data != null) {
			if (!EditorUtility.DisplayDialog ("放弃保存", "当前编辑不为空，打开将放弃保存!", "放弃", "取消"))
				return;
		}
		var path = EditorUtility.OpenFilePanel ("打开", Application.dataPath+ "/Resources", "xml");
		if (string.IsNullOrEmpty (path))
			return;
		var xml = File.ReadAllText (path,XmlParser.UTF8);
		data = XmlParser.DeSerialize<MagicData> (xml);
		currentPath = path;
	}

	private void Save(object userstate)
	{
		var data = userstate as MagicData;
		if (data == null)
			return;
		var xml = XmlParser.Serialize (data);
		if (!string.IsNullOrEmpty (currentPath)) {
			File.WriteAllText (currentPath, xml, XmlParser.UTF8);
			EditorUtility.DisplayDialog ("成功", "保存到:" + currentPath,"关闭");
		} else {
			SaveAs (data);
		}
		
	}

	private string currentPath;

	private void SaveAs(object userstate)
	{
		var data = userstate as MagicData;
		if (data == null)
			return;
		var xml = XmlParser.Serialize (data);

		var path =EditorUtility.SaveFilePanel ("打开", Application.dataPath+ "/Resources",data.key, "xml");
		if (string.IsNullOrEmpty (path))
			return;
		File.WriteAllText (path, xml, XmlParser.UTF8);
		currentPath = path;

		EditorUtility.DisplayDialog ("成功", "保存到:" + path,"关闭");
	}
	private void AddEvent(object userstate)
	{
		var data = userstate as MagicData;
		if (data == null)
			return;
		data.Containers.Add (new EventContainer (){ type = Layout.EventType.EVENT_TRIGGER });
	}

	private void OpenLayout(object userstate)
	{
		var e = userstate as EventContainer;
		//LayoutWindow
		LayoutEditorWindow.OpenLayout (e.layoutPath);
	}

	private void ShowEffectGroup(object  userstate)
	{
		var e = userstate as EffectGroup;
		EffectGroupEditorWindow.ShowEffectGroup (e.effects);
	}

	private void DeleteEffectGroupDe(object userstate)
	{
		var del = userstate as DeleteEffectGroup;
		if (del == null)
			return;
		del.Container.effectGroup.Remove (del.egroup);
	}

	private void AddEffectGroup(object userstate)
	{
		var e = userstate as EventContainer;
		if (e == null)
			return;
		e.effectGroup.Add (new EffectGroup (){ key= "Action_new"  });
	}
	private void DeleteEvent(object userstate)
	{
		var e = userstate as EventContainer;
		if (e == null)
			return;
		if (EditorUtility.DisplayDialog ("确认删除?", "删除当前的事件容器?", "删除", "取消")) {
			data.Containers.Remove (e);
			currentObj = null;
		}
	}

	private object currentObj;
	private void ShowObject(object obj)
	{
		if (obj == currentObj)
			return;
		Event.current.Use ();
		currentObj = obj;
		//Debug.Log (obj.ToString ());
	}

	private bool DrawWindow(int id,Rect r ,GUI.WindowFunction fun,string name)
	{

		GUI.Window (id, r, fun, name);
		if (Event.current.type == UnityEngine.EventType.MouseDown&& r.Contains(Event.current.mousePosition) ) {
			//Debug.Log (r + "|" + Event.current.mousePosition);
			//Event.current.Use ();
			return true;
		}
		return false;
	}


	private void EffectWindow(int id)
	{
		var indexC = (int)(id / 100) -100;
		var con = data.Containers [indexC];
		var indexE = id - ((int)(id/100) * 100);
		var d = con.effectGroup [indexE];
		GUILayout.BeginVertical ();
		GUILayout.Label (string.Format("描述:{0}", d.Des));
		GUILayout.Label (string.Format("效果数:{0}", d.effects.Count));
		GUILayout.EndVertical ();
	}

	private void MagicWindow(int id)
	{
		GUILayout.BeginVertical ();
		//GUILayout.FlexibleSpace();
		GUILayout.Label (string.Format("Key:{0}", data.key));
		//GUILayout.FlexibleSpace();
		GUILayout.Label(string.Format("间隔:{0}s",data.triggerTicksTime));
		//GUILayout.FlexibleSpace();
		GUILayout.Label(string.Format("持续时间:{0}s",data.triggerDurationTime));
		//GUILayout.FlexibleSpace();
		GUILayout.Label(string.Format("事件数:{0}",data.Containers.Count));
		GUILayout.EndVertical ();
		//GUI.DragWindow ();
	}

	public void EventWindow(int id)
	{
		GUILayout.BeginVertical ();
		var ec = data.Containers [id-100];
		GUILayout.Label (string.Format("路径:{0}", ec.layoutPath));
		GUILayout.Label (string.Format("效果组:{0}个", ec.effectGroup.Count));
		GUILayout.EndVertical ();
		//GUI.DragWindow ();
	}


}
