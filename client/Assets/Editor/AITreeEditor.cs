using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Layout.AITree;
using Layout.EditorAttributes;
using System.IO;
using GameLogic.Game.Perceptions;
using GameLogic.Game.AIBehaviorTree;
using BehaviorTree;

/// <summary>
/// AI tree editor.
/// </summary>
public class AITreeEditor:EditorWindow 
{
    #region Inner types

	public class LineData
	{
		public Vector2 point;
		public bool IsRunning;

	}

	public class StateOfEditor
	{
		public bool Expanded;
		public bool OnEdited;
		public Vector2 scroll;
		//public static StateOfEditor Empty = new StateOfEditor ();
	}

	public class MenuState
	{
		public Type type;
		public TreeNode node;
	}

	public enum HoverType
	{
		Top,
		Middle,
		Bottom
	}

    public class ShortNameColors
    {
        public int order;
        public Color color;

        public ShortNameColors(int order,Color c)
        {
            this.color = c;
            this.order = order;
        }
    }

    #endregion

    #region static 
    [MenuItem("GAME/Editor/AITreeEditor &3")]
	public static void Init()
	{
		var window = (AITreeEditor)GetWindow(typeof(AITreeEditor), false, "AI辑器");
		window.minSize = new Vector2 (200, 100);
	}
        
	static AITreeEditor()
	{
		_nodeTypes.Clear ();
		var types = typeof(TreeNode).Assembly.GetTypes ();
		foreach (var i in types) {
			if (i.IsSubclassOf (typeof(TreeNode))) {
				var attrs = i.GetCustomAttributes (typeof(EditorAITreeNodeAttribute), false) as EditorAITreeNodeAttribute[];
				if (attrs.Length > 0) {
					_nodeTypes.Add (i, attrs [0]);
				}
			}
		}

		_colors.Clear ();
        _colors.Add ("Seq", new ShortNameColors(1,new Color32(0xCA,0xE1,0xFF,0xFF)));
        _colors.Add ("PSeq",new ShortNameColors(1,new Color32(0xCA,0xE1,0xFF,0xFF)));
        _colors.Add ("Sel",new ShortNameColors(1, new Color32 (0xEE,0xD8,0xAE,0xFF)));
        _colors.Add ("PSel", new ShortNameColors(1,new Color32 (0xEE,0xD8,0xAE,0xFF)));
        _colors.Add ("Dec",new ShortNameColors(2, new Color32(0xFF,0xF6,0x8F,0xFF)));
        _colors.Add ("PRSel", new ShortNameColors(1,new Color32(0xDD,0xA0,0xDD,0xff)));
        _colors.Add ("PRNode", new ShortNameColors(1,new Color32(0xDD,0xA0,0xDD,0xff)));
        _colors.Add ("Act", new ShortNameColors(5,new Color32(0xFF,0xff,0xff,0xff)));
        _colors.Add ("Event", new ShortNameColors(3,Color.blue));
        _colors.Add ("Cond", new ShortNameColors(4,Color.yellow));

	}
    #endregion

    #region fields
    private static Dictionary<string,ShortNameColors> _colors = new Dictionary<string, ShortNameColors>();
	private static Dictionary<Type,EditorAITreeNodeAttribute> _nodeTypes = new Dictionary<Type, EditorAITreeNodeAttribute> ();

	private Color GetColorByShortName(string name)
	{ 
        ShortNameColors c;
		if (_colors.TryGetValue (name, out c))
            return c.color;
		return Color.white;
	}

	private Dictionary<string, StateOfEditor> _expand = new Dictionary<string, StateOfEditor>();

	private StateOfEditor this[string key]
	{
		get{
			StateOfEditor e;
			if (_expand.TryGetValue (key, out e))
				return e;
			else {
				e = new StateOfEditor ();
				_expand.Add (key, e);
			}
			return e;
		}

		set{ 
			if (_expand.ContainsKey (key))
				_expand [key] = value;
			else
			_expand.Add (key, value);
		}
	}

    private const string AI_ROOT="/Resources/AI";
	private const int height = 40;
	private const int width =200;
	private const int editHeight = 200;
	private const int offsetx = 40;
	private const int offsety = 20;

	private TreeNode root;

	//private Vector2 scroll = Vector2.zero;
	//private Vector2 lastoffset = Vector2.zero;
    private float scale =1;
    private Vector2 offsetPos = Vector2.zero;
    #endregion

    #region OnGUI
    public void OnGUI()
    {
        Repaint();
        var guiSty = new GUIStyle();
        guiSty.normal.textColor = Color.yellow;
        guiSty.alignment = TextAnchor.MiddleRight;
        GUI.Label(new Rect(this.position.width -200, 0, 150, 20),
            string.Format("s:{0:0.0} p:x={1:0.0} y={2:0.0}",
                scale,offsetPos.x,offsetPos.y),guiSty
        );

        if (GUI.Button(new Rect(this.position.width - 50, 0, 45, 20), "Reset"))
        {
            scale = 1; 
            //offsetPos = Vector2.zero;
        }

        if (!string.IsNullOrEmpty(currenPath))
        {
            GUI.Label(new Rect(3, 0, position.xMax - 10, 20), currenPath);
        }
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(0,20,100000,100000));
        var m = GUI.matrix;
        var sm = m * 
            Matrix4x4.TRS(
                new Vector3(offsetPos.x,offsetPos.y,0),
                Quaternion.identity,Vector3.one*scale);
        GUI.matrix = sm;
        OnDrawGUI();
        GUI.matrix = m;
        DrawOp();
        if (Event.current.type == EventType.ScrollWheel)
        {
            float y = Event.current.delta.y;
            var s =  (1 + (-y) / 100);
            scale *=s;
            scale = Mathf.Clamp(scale,0.1f,1.5f) ;
            Event.current.Use();
        }

        if (currentDrag == null 
            && Event.current.type == EventType.MouseDrag)
        {
            offsetPos += Event.current.delta;
            Event.current.Use();
        }
    }

    private void OnDrawGUI()
    {
        if (root == null)
        {
            if (Event.current.type == EventType.ContextClick)
            {
                GenericMenu m = new GenericMenu();
                m.AddItem(new GUIContent("Open Tree"), false, OpenTree);
                m.AddSeparator("");
                ProcessMenu(m, null);
                m.ShowAsContext();
            }
            if (!string.IsNullOrEmpty(currenPath))
            {
                OpenTree(currenPath);
            }
        }
        else
        {
            runstate = null;
            //runNodeName = string.Empty;
            //var rect = new Rect(0, 0, position.width, position.height); 
            //scroll = GUI.BeginScrollView(rect, scroll, new Rect(0, 0, lastoffset.x, lastoffset.y));
            Vector2 mine;
            RunStatus? runState;
            var runing = CheckRunning(root.guid, out runState);
            //GUI.BeginClip (rect);
            //lastoffset =
            DrawRoot(root, new Vector2(offsetx, offsety), runing, runState, out mine);
            //GUI.EndClip ();
            //lastoffset.x;

            if (currentDrag != null)
            {

                var p = Event.current.mousePosition;
                var prect = new Rect(p.x, p.y, width, height);
                DrawNode(prect, currentDrag, new StateOfEditor(), false, false, null);
            }



            if (Event.current.type == EventType.MouseUp)
            {
                if (currentDrag != null)
                {
                    currentDrag = null;
                    currentShowDrag = null;
                }
            }

            if (Event.current.type == EventType.MouseDrag)
            {
                currentShowDrag = null;
            }

            if (currentShowDrag.HasValue)
            {
                GLDraw.DrawFillBox(currentShowDrag.Value, Color.black, Color.green, 1);
            }
            //GUI.EndScrollView();
        }



    }

    private void DrawOp()
    {
        #region Operator
        {
            var group = new Rect(5, position.height - 30, 300, 25);
            GUI.Box(new Rect(3, position.height - 50, 276, 50), "Operator");
            GUILayout.BeginArea(group);
            GUILayout.BeginHorizontal(GUILayout.Width(300));

            if (GUILayout.Button("Run", GUILayout.Width(50)))
            {

                RunAI();
            }

            if (GUILayout.Button("New", GUILayout.Width(50)))
            {
                if (root != null && ShowSaveNotify())
                {
                    root = null;
                    currenPath = null;
                    currentDrag = null;
                    currentShowDrag = null;
                    _runRoot = null;
                }
            }
            if (GUILayout.Button("Open", GUILayout.Width(50)))
            {
                if (ShowSaveNotify())
                {
                    this.OpenTree();
                }
            }

            if (GUILayout.Button("Save", GUILayout.Width(50)))
            {
                Save();
            }

            if (GUILayout.Button("Save As", GUILayout.Width(50)))
            {
                SaveAs();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        #endregion
        if (runstate != null)
        {
            
            /*// disable show state
            var group = new Rect(position.width - 300, position.height - 100, 295, 90);
            GUI.BeginGroup(group);
            GUI.Box(new Rect(position.width - 300 - 3, position.height - 125, 300, 120), "RunState");
            _scrollviewDebug = GUILayout.BeginScrollView(_scrollviewDebug);
            GUILayout.BeginVertical(GUILayout.Width(260));
            PropertyDrawer.DrawObject(runstate);
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUI.EndGroup();
            */
        }
        
    }
    #endregion

    #region Menu Operator

    private TreeNode pasteNode = null;

    private void ProcessMenu(GenericMenu m, TreeNode node)
    {
        if (node != null)
        {
            if (pasteNode != null)
            {
                m.AddItem(new GUIContent("Paste"), false, PasteNode, new MenuState()
                    {
                        type = null,
                        node = node
                    });
            }
            if (pasteNode != node)
            {
                m.AddItem(new GUIContent("Copy"), false, CopyNode, new MenuState()
                    {
                        type = null,
                        node = node
                    });
            }
            m.AddSeparator("");

            m.AddItem(new GUIContent("Export"), false, ExportNode, new MenuState()
                {
                    type = null,
                    node = node
                });
            m.AddItem(new GUIContent("Import"), false, ImportNode, new MenuState()
                {
                    type = null,
                    node = node
                });
            m.AddSeparator("");
        }

        foreach (var i in _nodeTypes) 
        {
            if (node == null) {
                if (!CanAppend (i.Key))
                    continue;
            } else {
                if (!CanAppend (node, i.Key))
                    continue;
            }


            m.AddItem (new GUIContent (i.Value.Flag + "/" + i.Value.Name),false,CreateNode
                ,new MenuState(){
                type = i.Key,
                node = node
            });
        }
    }

    private void PasteNode(object userState)
    {
        var m = userState as MenuState;
        if (CanAppend(m.node, pasteNode))
        {
            var tempNode = XmlParser.DeSerialize<TreeNode>(XmlParser.Serialize(pasteNode));
            tempNode.NewGuid();
            m.node.childs.Add(tempNode);
            pasteNode = null;
        }
        else
        {
            ShowNotification(new GUIContent(string.Format("Can't Paste:{0}", m.node.name)));
        }
    }

    private void CopyNode(object userState)
    {
        var m = userState as MenuState;
        pasteNode = m.node;
        ShowNotification(new GUIContent(string.Format("Copy:{0}",m.node.name)));
    }

    private void ImportNode(object userState)
    {
        var m = userState as MenuState;
        string path = EditorUtility.OpenFilePanel(
            "Import Ai Tree",
            Application.dataPath + AI_ROOT, "xml");
        if(!string.IsNullOrEmpty(path))
        {
            var node = XmlParser.DeSerialize<TreeNode>(File.ReadAllText(path));
            if (CanAppend(m.node, node))
            {
                node.NewGuid();
                m.node.childs.Add(node);
            }
            else
            {
                ShowNotification(new GUIContent(string.Format("Can't Import:{0}", m.node.name)));
            }
        }
    }

    private void ExportNode(object userState)
    {
        var m = userState as MenuState;
        var path = EditorUtility.SaveFilePanel("Export Ai Tree", 
            Application.dataPath + AI_ROOT,"AITree", "xml");
        if (string.IsNullOrEmpty (path))
            return;
        
        var xml = XmlParser.Serialize (m.node);
        var exportNode = XmlParser.DeSerialize<TreeNode>(xml);
        exportNode.NewGuid();
        xml = XmlParser.Serialize(exportNode);
        File.WriteAllText (path, xml);
        ShowNotification ( new GUIContent("Export To:" + path));
        AssetDatabase.Refresh ();
    }

    private void CreateNode(object userState)
    {
        var m = userState as MenuState;
        var n = TreeNode.CreateInstance (m.type);
        if (root != null) {
            m.node.childs.Add (n);
            this [m.node.guid].Expanded = true;
        } else {
            root = n;
            currenPath = string.Empty;
        }
    }

    private void DeleteNode(object userState)
    {
        var n = userState as TreeNode;
        var r = TreeNode.FindNodeByGuid (root,n.guid);
        if (r.HasValue && r.Value.Parant != null) {
            r.Value.Parant.childs.Remove (n);
        }  else if(r.HasValue) {
            root = null;
            currenPath = string.Empty;
        }
    }

    #endregion

    #region Save

    private string currenPath;

	private void OpenTree()
	{
		var path = EditorUtility.OpenFilePanel("Open AI Tree",
            Application.dataPath+ AI_ROOT,"xml");	
        OpenTree(path);
	}

    private void OpenTree(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            var xml = File.ReadAllText(path, XmlParser.UTF8);
            root = XmlParser.DeSerialize<TreeNode>(xml);
            currenPath = path;
            currentDrag = null;
            currentShowDrag = null;
            _runRoot = null;
            this._expand.Clear();
        }
    }

	private void Save()
	{
		if (!string.IsNullOrEmpty (currenPath)) {
			var xml = XmlParser.Serialize (root);
			File.WriteAllText (currenPath, xml);
			ShowNotification (new GUIContent( "Save To:" + currenPath));
			AssetDatabase.Refresh ();
		} else {
			SaveAs ();
		}
	}

	private void SaveAs()
	{
        currenPath = EditorUtility.SaveFilePanel("Save Tree", Application.dataPath +
            AI_ROOT,"AITree_"+root.guid, "xml");
		if (string.IsNullOrEmpty (currenPath))
			return;

		var xml = XmlParser.Serialize (root);
		File.WriteAllText (currenPath, xml);

		ShowNotification ( new GUIContent("Save To:" + currenPath));
		AssetDatabase.Refresh ();
	}
   
    private bool ShowSaveNotify()
    {
        return EditorUtility.DisplayDialog ("Cancel", "Do you want over current edit？", "Yes", "Cancel");
    }

    #endregion
    #region Run And Debug
    private void RunAI()
    {
        if (root == null)
            return;
        if (!EditorApplication.isPlaying)
            return;
        var gate = UApplication.G<EditorGate>();
        if (gate == null) return;

        var per = gate.releaser.Controllor.Perception as BattlePerception;
        _runRoot = per.ChangeCharacterAI(root, gate.releaser);
    }

	private AITreeRoot _runRoot;

	public void AttachRoot(AITreeRoot root)
	{
		if (root != null) 
		{
			if (!ShowSaveNotify())
				return;
		}
		_runRoot = root;
		this.root = _runRoot.NodeRoot;
	}

    #endregion
 
    #region draw

    private void SetLastClick(TreeNode node)
    {
        lastClick = node;
    }

    private TreeNode lastClick;

    private EditorAITreeNodeAttribute GetNodeAtt(Type type)
    {
        var attrs = type.GetCustomAttributes (typeof(EditorAITreeNodeAttribute), false) as EditorAITreeNodeAttribute[];
        if (attrs.Length > 0)
            return attrs [0];
        return null;
    }
	private bool CheckRunning(string guid,out RunStatus? state)
	{
		state = null;
		if (!EditorApplication.isPlaying)
			return false;
		if (_runRoot == null)
			return false;
		var comp =_runRoot.Root.FindGuid (guid);
		if (comp == null)
			return false;
		if (comp.LastStatus == null)
			return false;
		state = comp.LastStatus;
		return comp.LastStatus.Value == BehaviorTree.RunStatus.Running;
		//return false;
	}

    private Composite GetComposite(string guid)
    {
        if (!EditorApplication.isPlaying)
            return null;
        if (_runRoot == null)
            return null;
        var comp =_runRoot.Root.FindGuid (guid);
        if (comp == null)
            return null;
        return comp;
    }

	private Vector2 DrawRoot(TreeNode node, Vector2 offset, bool isRuning,RunStatus? state, out  Vector2  current)
	{
		var tempOffset = new Vector2 (offset.x+offsetx + width, offset.y);
		float offex = tempOffset.x;
		var expand = this [node.guid];
		List<LineData> points = new List<LineData> ();
		if (expand.Expanded) {
			for (var i = 0; i < node.childs.Count; i++) {
				if (node.childs [i] == currentDrag)
					continue;
				RunStatus? cRunstate;
				var runing = CheckRunning(node.childs [i].guid,out cRunstate);
				var mine = tempOffset;
				var or = DrawRoot (node.childs [i], tempOffset, runing,cRunstate, out mine);
				var h = Mathf.Max (height + offsety, or.y);
				offex = Mathf.Max (offex, or.x);

				points.Add ( new LineData{ point = new Vector2(mine.x,mine.y+5), IsRunning = runing});
				tempOffset.y += h;
			}
		}
		var t = Mathf.Max (0, (tempOffset.y - offset.y-height));
		int currentHeight = height;
		if (this [node.guid].OnEdited) 
        {
			currentHeight = editHeight;
            runstate = GetComposite(node.guid);
            //runNodeName = node.ToString();
		}
		var rect = new Rect (offset.x, offset.y + (t/2), width, currentHeight);
		this[node.guid] = DrawNode (rect, node, expand, node.childs.Count>0,isRuning,state);

		foreach (var p in points) 
		{
			GLDraw.DrawConnectingCurve(new Vector2(rect.xMax+10,rect.center.y),p.point,
				p.IsRunning? Color.yellow: Color.black, p.IsRunning?2:1);
		}

		current = rect.position;

		return new Vector2(offex, Mathf.Max(height + offsety, t+offsety+currentHeight));
	}

    private object runstate;
 
	private StateOfEditor DrawNode(Rect rect, TreeNode node, StateOfEditor expanded, bool haveChild, bool isRuning,RunStatus? state)
	{
		Color color = isRuning ? Color.yellow : Color.black;


		Color bg = Color.white;
		var att = GetNodeAtt (node.GetType ());

		if (lastClick == node) {
			color = Color.green;
		}
		var name = node.GetType().Name;
		if (att != null) 
        {
			name = att.ShorName+":"+node.name + (state==null?"":state.Value.ToString());
			bg = GetColorByShortName (att.ShorName);
		}

		if (haveChild) {
			expanded.Expanded =	EditorGLTools.DrawExpandBox (rect, name,
				expanded.OnEdited?string.Empty:node.ToString (), 
				expanded.Expanded, color, bg, isRuning ? 2 : 1);
		} else {
			EditorGLTools.DrawTitleRect (rect, name,
				expanded.OnEdited?string.Empty:node.ToString (),
				color, bg, isRuning ? 2 : 1); 
		} 

		if (lastClick == node) {
			if (GUI.Button (new Rect (rect.xMax - 50, rect.y, 50, 20), expanded.OnEdited?"Close":"Edit")) {
				expanded.OnEdited = !expanded.OnEdited;
			}
		} else {
			expanded.OnEdited = false;
		}

		if (expanded.OnEdited) {
		
            GUILayout.BeginArea (new Rect (rect.x, rect.y + 25, rect.width-2, rect.height - 25));
			expanded.scroll = GUILayout.BeginScrollView (expanded.scroll);
			GUILayout.BeginVertical(GUILayout.Width(rect.width-25));
			PropertyDrawer.DrawObject (node,"AINODE");
			GUILayout.EndVertical ();
			GUILayout.EndScrollView ();
            GUILayout.EndArea ();
		}


		if (currentDrag == node)
			return new StateOfEditor();
		
		if (Event.current.type == EventType.ContextClick) 
        {
			if (rect.Contains (Event.current.mousePosition))
            {
				GenericMenu m = new GenericMenu ();
				m.AddItem (new GUIContent ("Delete"), false, DeleteNode, node);
				//m.AddSeparator ("");
				ProcessMenu (m, node);
				m.ShowAsContext ();
				Event.current.Use ();
			}
		}
            
		if (lastClick != node) {
			if (Event.current.type == EventType.MouseDown) {
				if (rect.Contains (Event.current.mousePosition)) {
					SetLastClick (node);
					Event.current.Use ();
				}
			}
		} 

		if (Event.current.type == EventType.MouseDrag) {
			if (rect.Contains (Event.current.mousePosition)) {
				if (currentDrag != null) {
					if (currentDrag != node) {
						
						HoverType t = HoverType.Middle;
						if (node != root)
							t = GetHoverType (rect, Event.current.mousePosition.y);
						DragHover (rect, t); 
						Event.current.Use ();

					} else {
						currentShowDrag = null;
						Event.current.Use ();
					}
				} else {
				
					BeginDrag (node);
					Event.current.Use ();
				}
			}
		}

		if (Event.current.type == EventType.MouseUp) {
			if (currentDrag != null && currentDrag != node) {
				if (rect.Contains (Event.current.mousePosition)) {
					var t = GetHoverType (rect, Event.current.mousePosition.y);
					EndDrag (node, t);
					Event.current.Use ();
				}
			}
		}

		return expanded;
	}
       
	private HoverType GetHoverType(Rect rect, float y)
	{
		HoverType hy = HoverType.Middle;
		float p =	rect.height / 3;

		if (y < p + rect.y) {
			hy = HoverType.Top;
		} else if (y > 2 * p + rect.y) {
			hy = HoverType.Bottom;
		}
		return hy;
	}
    #endregion

    #region Drag
	private Rect? currentShowDrag;
    private TreeNode currentDrag;
    private void DragHover(Rect rect,HoverType type)
    {
        switch (type) {
            case HoverType.Bottom:
                currentShowDrag = new Rect (rect.x, rect.yMax, rect.width, offsety);
                break;
            case HoverType.Middle:
                currentShowDrag = new Rect (rect.xMax, rect.center.y- offsety/2, rect.width, offsety);
                break;
            case HoverType.Top:
                currentShowDrag = new Rect (rect.x, rect.y-offsety, rect.width, offsety);
                break;
        }
    }

	private void BeginDrag(TreeNode node)
	{
		if(root == node) 
			return;
		currentDrag = node;
	}

	private void EndDrag(TreeNode hoverNode,HoverType type)
	{
		var reDrag = TreeNode.FindNodeByGuid (root,currentDrag.guid);
		var hove = TreeNode.FindNodeByGuid (root,hoverNode.guid);
		if (reDrag != null && hove != null) {
			
			switch(type)
			{
			case HoverType.Bottom:
				{
                        if (CanAppend(hove.Value.Parant, currentDrag))
                        {
                            int index = hove.Value.Parant.childs.IndexOf(hoverNode) + 1;
                            if (reDrag.Value.Parant == hove.Value.Parant)
                            {
                                int a = reDrag.Value.Parant.childs.IndexOf(currentDrag);
                                index = hove.Value.Parant.childs.IndexOf(hoverNode);
                                if (a > index)
                                {
                                    index += 1;
                                }
                                reDrag.Value.Parant.childs.Remove(currentDrag);
                                hove.Value.Parant.childs.Insert(index, currentDrag);
                            }
                            else if (index >= 0 && index < hove.Value.Parant.childs.Count)
                            {
                                reDrag.Value.Parant.childs.Remove(currentDrag);
                                hove.Value.Parant.childs.Insert(index, currentDrag);

                            }
                        }
                        else
                        {
                            EditorApplication.Beep();
                        }
				}
				break;
                case HoverType.Middle:
                    {
                        if (CanAppend(hoverNode, currentDrag))
                        {
                            reDrag.Value.Parant.childs.Remove(currentDrag);
                            hoverNode.childs.Add(currentDrag);
                        }
                        else
                        {
                            EditorApplication.Beep();
                        }
                    }
				break;
                case HoverType.Top:
                    {
                        if (CanAppend(hove.Value.Parant, currentDrag))
                        {
                            int index = hove.Value.Parant.childs.IndexOf(hoverNode);
                            if (index >= 0 && index <= hove.Value.Parant.childs.Count)
                            {
                                reDrag.Value.Parant.childs.Remove(currentDrag);
                                hove.Value.Parant.childs.Insert(index, currentDrag);
                            }
                        }
                        else
                        {
                            EditorApplication.Beep();
                        }
                    }
				break;
			}
		}
		 
		currentDrag = null;
		currentShowDrag = null;
	}
    #endregion

    #region append 
	private bool CanAppend(Type t )
	{
		if (t == typeof(TreeNodeProbabilityNode))
			return false;
		var att = GetNodeAtt (t);
		if (att == null)
			return false;
		switch (att.CanAppendChild) {
		case AllowChildType.Manay:
		case AllowChildType.One:
		case AllowChildType.Probability:
			return true;
		default:
			return false;
		}
	}

	private bool CanAppend(TreeNode node, TreeNode child)
	{
		return CanAppend (node, child.GetType ());
	}

	private bool CanAppend(TreeNode node, Type t)
	{
		if (node == null)
			return false;
		
		var att = GetNodeAtt (node.GetType ());
		switch (att.CanAppendChild) {
		case  AllowChildType.Manay:
			return true;
		case AllowChildType.One:
			{
				return node.childs.Count == 0;
			}
		case  AllowChildType.None:
			return false;
		case AllowChildType.Probability:
			{
				return t == typeof(TreeNodeProbabilityNode);
			}
		}
		return false;
	}
    #endregion

}
