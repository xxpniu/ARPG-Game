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
using System.Linq;
using GameLogic.Game.AIBehaviorTree;
using UGameTools;
using GameLogic.Game.States;

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

            var releaserMagics = ExcelToJSONConfigManager
                .Current.GetConfigs<CharacterMagicData>(t => t.CharacterID == releaserData.ID).ToList();

            var targetMagics = ExcelToJSONConfigManager
                .Current.GetConfigs<CharacterMagicData>(t => t.CharacterID == targetData.ID).ToList();
			//throw new NotImplementedException ();
			var per = state.Perception as BattlePerception;
			var scene = UPerceptionView.Singleton.UScene;
            var releaser = per.CreateCharacter( 1,releaserData, releaserMagics, 1,
				new EngineCore.GVector3(scene.startPoint.position.x,
					scene.startPoint.position.y,scene.startPoint.position.z),
				new EngineCore.GVector3(0,90,0),-1);
            var target =  per.CreateCharacter(1,targetData,targetMagics, 2,
				new EngineCore.GVector3(scene.enemyStartPoint.position.x,
					scene.enemyStartPoint.position.y,scene.enemyStartPoint.position.z),
				new EngineCore.GVector3(0,-90,0),-1);
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
        curState = new BattleState(UView.Singleton, new StateLoader(this),this);
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
        }
    }

	#endregion

	public MagicReleaser currentReleaser;

	public BattleCharacter releaser;
	public BattleCharacter target;

    public bool EnableTap = false;

	public void ReleaseMagic(MagicData magic)
	{
		Resources.UnloadUnusedAssets();

		if (currentReleaser != null)
		{
			GObject.Destory (currentReleaser);
		}

		var per = curState.Perception as BattlePerception;
        this.currentReleaser =per.CreateReleaser (magic,
            new GameLogic.Game.LayoutLogics.ReleaseAtTarget (this.releaser, this.target),
            ReleaserType.Magic,0,0);
		
	}
        
	public void ReplaceRelease(ExcelConfig.CharacterData data,bool stay, bool ai)
	{
        var magics = ExcelToJSONConfigManager
            .Current.GetConfigs<CharacterMagicData>(t => t.CharacterID == data.ID).ToList();
		if (!stay)
			this.releaser.SubHP (this.releaser.HP);
		var per = curState.Perception as BattlePerception;
		var scene = UPerceptionView.Singleton.UScene;
        var releaser = per.CreateCharacter(1,data, magics, 1,
                     new EngineCore.GVector3(scene.startPoint.position.x,
                         scene.startPoint.position.y, scene.startPoint.position.z),
                     new EngineCore.GVector3(0, 90, 0),-1);
		if(ai)
		per.ChangeCharacterAI (data.AIResourcePath, releaser);
		this.releaser = releaser;
	}

	public void ReplaceTarget(ExcelConfig.CharacterData data,bool stay, bool ai)
	{
        var magics = ExcelToJSONConfigManager
            .Current.GetConfigs<CharacterMagicData>(t => t.CharacterID == data.ID).ToList();
		if (!stay)
			this.target.SubHP (this.target.HP);
		var per = curState.Perception as BattlePerception;
		var scene = UPerceptionView.Singleton.UScene;
        var target =per.CreateCharacter(1,data,magics, 2,
			new EngineCore.GVector3(scene.enemyStartPoint.position.x,
				scene.enemyStartPoint.position.y,scene.enemyStartPoint.position.z),
			new EngineCore.GVector3(0,-90,0),-1);;
		if(ai)
			per.ChangeCharacterAI (data.AIResourcePath, target);
		this.target = target;
	}
        
    public override void OnTap(TapGesture gesutre)
    {
        if (!EnableTap)
            return;
        //Debug.Log(gesutre.Position);

        Ray ray = Camera.main.ScreenPointToRay(gesutre.Position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.tag != AstarGridBase.GROUND)
                return;

            //Debug.Log(hit.point);
            var message = new Proto.Action_ClickMapGround
                { 
                    TargetPosition =  hit.point.ToPVer3()
                };
            DoAction(message);
                   
        }
    }

    public void DoAction(Proto.ISerializerable action)
    {
        if (this.releaser != null && this.releaser.AIRoot != null)
        {            
            this.releaser.AIRoot[AITreeRoot.ACTION_MESSAGE] = action;
            this.releaser.AIRoot.BreakTree();
        } 
    }
}
#endif
