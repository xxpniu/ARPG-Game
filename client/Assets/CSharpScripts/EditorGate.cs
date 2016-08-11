using System;
using GameLogic;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLogic.Game.Perceptions;
using GameLogic.Game.Elements;
using Layout;
using Layout.LayoutEffects;
using ExcelConfig;
using org.vxwo.csharp.json;
#if UNITY_EDITOR
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
			var releaserData = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData> (1);
			var targetData = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData> (2);

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
			Gate.releaser = releaser;
			Gate.target = target;
		}
		#endregion
		
	}

	public const string EDITOR_LEVEL_NAME ="EditorReleaseMagic";

	#region implemented abstract members of UGate


	public override GTime GetTime ()
	{
		return new GTime (){ DetalTime = Time.deltaTime, Time = Time.time };
	}
		


	private  AsyncOperation operation;

	public override void JoinGate ()
	{
        curState = new GameLogic.Game.States.BattleState(UView.Singleton, new StateLoader(this),this);
		curState.Init ();
		curState.Start (Now);
		UPerceptionView.Singleton.UseCache = false;
	}

	private GState curState;

	public override void ExitGate ()
	{
		curState.Stop (Now);
	}

	public override void Tick ()
    {
        if (curState != null)
        {
            GState.Tick(curState, Now);
            var per = curState.Perception as BattlePerception;
            var notitys = per.GetNotifyMessageAndClear();
            if (notitys.Length > 0)
                Debug.Log(JsonTool.Serialize(notitys));
        }
    }

	#endregion

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
		per.CreateReleaser (magic, new GameLogic.Game.LayoutLogics.ReleaseAtTarget (this.releaser, this.target));
		
	}


	public void ReplaceRelease(ExcelConfig.CharacterData data,bool stay, bool ai)
	{
		if (!stay)
			this.releaser.SubHP (this.releaser.HP);
		var per = curState.Perception as BattlePerception;
		var scene = UPerceptionView.Singleton.UScene;
        var releaser = per.CreateCharacter(data, 1,
                     new EngineCore.GVector3(scene.startPoint.position.x,
                         scene.startPoint.position.y, scene.startPoint.position.z),
                     new EngineCore.GVector3(0, 90, 0));
		if(ai)
		per.ChangeCharacterAI (data.AIResourcePath, releaser);
		this.releaser = releaser;
	}

	public void ReplaceTarget(ExcelConfig.CharacterData data,bool stay, bool ai)
	{
		if (!stay)
			this.target.SubHP (this.target.HP);
		var per = curState.Perception as BattlePerception;
		var scene = UPerceptionView.Singleton.UScene;
		var target =per.CreateCharacter(data,2,
			new EngineCore.GVector3(scene.enemyStartPoint.position.x,
				scene.enemyStartPoint.position.y,scene.enemyStartPoint.position.z),
			new EngineCore.GVector3(0,-90,0));;
		if(ai)
			per.ChangeCharacterAI (data.AIResourcePath, target);
		this.target = target;
	}
}
#endif
