using System;
using GameLogic;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLogic.Game.Perceptions;
using GameLogic.Game.Elements;
using Layout;
using Layout.LayoutEffects;

public class EditorGate:UGate
{
	public  class StateLoader :IStateLoader
	{

		public StateLoader(EditorGate gate)
		{
			Gate = gate;
		}

		private EditorGate Gate{ set; get; }
		#region IStateLoader implementation
		public void Load (EngineCore.Simulater.GState state)
		{
			var releaserData = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData> (1);
			var targetData = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData> (2);
			//throw new NotImplementedException ();
			var per = state.Perception as BattlePerception;
			var scene = UPerceptionView.Singleton.UScene;
			var releaser = per.CreateCharacter( releaserData,1,
				new EngineCore.GVector3(scene.startPoint.position.x,
					scene.startPoint.position.y,scene.startPoint.position.z),
				new EngineCore.GVector3(0,90,0));
			var target =  per.CreateCharacter(targetData,2,
				new EngineCore.GVector3(scene.enemyStartPoint.position.x,
					scene.enemyStartPoint.position.y,scene.enemyStartPoint.position.z),
				new EngineCore.GVector3(0,-90,0));
			per.State.AddElement (releaser);
			per.State.AddElement (target);
			Gate.releaser = releaser;
			Gate.target = target;
		}
		#endregion
		
	}

	public const string EDITOR_LEVEL_NAME ="EditorReleaseMagic";

	#region implemented abstract members of UGate


	private  AsyncOperation operation;

	public override void JoinGate ()
	{
		curState = new GameLogic.Game.States.BattleState(UView.Singleton, new StateLoader(this));
		curState.Start (CTime);
		//operation = null;
		UPerceptionView.Singleton.UseCache = false;
	}

	private GState curState;

	public override void ExitGate ()
	{
		curState.Stop (CTime);
	}

	public override void Tick ()
	{
		if (curState != null) 
		{
			GState.Tick (curState, CTime );
		}

		//Debug.Log ("Del:"+CTime.DetalTime);
	}

	#endregion

	public GTime CTime{ get { return new GTime (){ DetalTime = Time.deltaTime, Time = Time.time }; } }

	public MagicReleaser currentReleaser;

	public BattleCharacter releaser;
	public BattleCharacter target;

	public void ReleaseMagic(MagicData magic)
	{
		Resources.UnloadUnusedAssets();

		if (currentReleaser != null)
		{
			GObject.Destory (currentReleaser);
		}

		var per = curState.Perception as BattlePerception;
		currentReleaser = per.CreateReleaser (magic, new GameLogic.Game.LayoutLogics.ReleaseAtTarget (this.releaser, this.target));
		per.State.AddElement (currentReleaser);
	}


	public void ReplaceRelease(ExcelConfig.CharacterData data)
	{
		GObject.Destory (this.releaser);

		var per = curState.Perception as BattlePerception;
		var scene = UPerceptionView.Singleton.UScene;
		var releaser = per.CreateCharacter( data,1,
			new EngineCore.GVector3(scene.startPoint.position.x,
				scene.startPoint.position.y,scene.startPoint.position.z),
			new EngineCore.GVector3(0,90,0));
		
		per.State.AddElement (releaser);
		this.releaser = releaser;
	}

	public void ReplaceTarget(ExcelConfig.CharacterData data)
	{
		GObject.Destory (this.target);

		var per = curState.Perception as BattlePerception;
		var scene = UPerceptionView.Singleton.UScene;
		var target =per.CreateCharacter(data,2,
			new EngineCore.GVector3(scene.enemyStartPoint.position.x,
				scene.enemyStartPoint.position.y,scene.enemyStartPoint.position.z),
			new EngineCore.GVector3(0,-90,0));;

		per.State.AddElement (target);
		this.target = target;
	}
}
