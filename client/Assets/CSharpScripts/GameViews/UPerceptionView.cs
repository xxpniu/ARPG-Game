using UnityEngine;
using System.Collections;
using GameLogic.Game.Perceptions;
using Layout.LayoutElements;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;
using Vector3 = UnityEngine.Vector3;
using System.Collections.Generic;
using EngineCore;


public class UPerceptionView :XSingleton<UPerceptionView>,IBattlePerception {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public UGameScene UScene;


	void Awake()
	{
		_magicData = new Dictionary<string, Layout.MagicData> ();
		_timeLines = new Dictionary<string, TimeLine> ();
		var  magics = ResourcesManager.Singleton.LoadAll<TextAsset> ("Magics");
		foreach (var i in magics) {
			var xml = i.text;
			var m = XmlParser.DeSerialize<Layout.MagicData> (i.text);
			_magicData.Add (m.key, m);
		}
		magicCount = _magicData.Count;
		var timeLines = ResourcesManager.Singleton.LoadAll<TextAsset> ("Layouts");
		foreach (var i in timeLines) 
		{
			var line = XmlParser.DeSerialize<TimeLine> (i.text);
			_timeLines.Add ("Layouts/" + i.name+".xml",line);
		}
		timeLineCount = _timeLines.Count;

		UScene = GameObject.FindObjectOfType<UGameScene> ();
	}

	public int timeLineCount = 0;
	public int magicCount =0;

	#region IBattlePerception implementation


	public EngineCore.GVector3 GetStartPoint ()
	{
		var start = UScene.startPoint;
		return new EngineCore.GVector3 (start.position.x, start.position.y, start.position.y);
	}

	public EngineCore.GVector3 GetEnemyStartPoint ()
	{
		var start = UScene.enemyStartPoint;
		return new EngineCore.GVector3 (start.position.x, start.position.y, start.position.y);
	}

	public  TimeLine GetTimeLineByPath (string path)
	{
		TimeLine line;
		if(_timeLines.TryGetValue(path,out line))
		{
			return line;	
		}
		Debug.LogError ("No found timeline by path:"+path);
		return null;
	}

	private Dictionary<string,TimeLine> _timeLines;

	private Dictionary<string ,Layout.MagicData> _magicData;

	public Layout.MagicData GetMagicByKey (string key)
	{
		Layout.MagicData magic;
		if(_magicData.TryGetValue(key,out magic))
		{
			return magic;	
		}
		Debug.LogError ("No found magic by key:"+key);
		return null;
	}

	public IBattleCharacter CreateBattleCharacterView (string resources, GVector3 pos,GVector3 forward)
	{
		throw new System.NotImplementedException ();
	}

	public IMagicReleaser CreateReleaserView (GameLogic.Game.Elements.IBattleCharacter releaser, GameLogic.Game.Elements.IBattleCharacter targt, EngineCore.GVector3? targetPos)
	{
		throw new System.NotImplementedException ();
	}

	public IParticlePlayer CreateParticlePlayer (GameLogic.Game.Elements.IBattleCharacter from, string fromBone, GameLogic.Game.Elements.IBattleCharacter to, string toBone)
	{
		throw new System.NotImplementedException ();
	}

	public IBattleMissle CreateMissile (GameLogic.Game.Elements.IMagicReleaser releaser, Layout.LayoutElements.MissileLayout layout)
	{
		throw new System.NotImplementedException ();
	}

	public float Distance (EngineCore.GVector3 v, EngineCore.GVector3 v2)
	{
		var uV = new Vector3 (v.x, v.y, v.x);
		var uV2 = new Vector3 (v2.x, v2.y, v2.z);
		return Vector3.Distance (uV, uV2);
	}

	public float Angle (EngineCore.GVector3 v, EngineCore.GVector3 v2)
	{
		var uV = new Vector3 (v.x, v.y, v.x);
		var uV2 = new Vector3 (v2.x, v2.y, v2.z);

		return Vector3.Angle (uV, uV2);
	}

	public EngineCore.GVector3 RotateWithY (EngineCore.GVector3 v, float angle)
	{
		var uV = new Vector3 (v.x, v.y, v.x);
		var qu = Quaternion.Euler (new Vector3 (0, angle, 0));
		var r = qu*uV;
		return new EngineCore.GVector3 (r.z, r.y, r.z);
	}

	#endregion
}
