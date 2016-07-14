using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLogic.Game.States;
using GameLogic;
using GameLogic.Game.Perceptions;

public class UGameGate:UGate,IStateLoader
{
	public UGameGate (int levelID)
	{
		this.data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.LevelData> (levelID);
		characters = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.CharacterData> (t => t.ID <= 4 && t.ID !=2);

	}	

	private ExcelConfig.CharacterData[] characters;
	private ExcelConfig.LevelData data;

	public float pointLeft =5;
	public float pointRight =5;

	private AsyncOperation operation;
	public override void JoinGate ()
	{
		UUIManager.Singleton.ShowMask (true);
		UUIManager.Singleton.ShowLoading (0);

		operation = SceneManager.LoadSceneAsync ("Level1", LoadSceneMode.Single);
	}

	public override void ExitGate ()
	{
		if (state != null)
			state.Stop (this.GetTime());
	}

	private GState state;


	private float startTime;


	public override void Tick ()
	{
		if (operation != null) {
			if (!operation.isDone) {
				UUIManager.Singleton.ShowLoading (operation.progress);
				return;
			}
				operation = null;
			state = new BattleState (UView.Singleton, this);
			state.Init ();
			state.Start (this.GetTime());
			startTime = this.GetTime ().Time;
			UUIManager.Singleton.ShowMask (false);
			var ui =UUIManager.Singleton.CreateWindow<Windows.GUIBattle> ();
			ui.ShowWindow ();
		}

		if (state == null)
			
			return;
		GState.Tick (state, this.GetTime ());



		if (aiTime < this.GetTime ().Time) {
			var data = GRandomer.RandomArray (this.characters);
			CreateTarget (data);
			aiTime =this.GetTime ().Time+GRandomer.RandomMinAndMax (1, 4);
		}

		if (lastTime > this.GetTime().Time)
			return;
		lastTime = this.GetTime ().Time + 3f;// GRandomer.RandomMinAndMax (4, 3);
		pointRight +=  1.5f;
		pointLeft += 1.8f;


	}

	private float aiTime = 0;
	private void CreateTarget(ExcelConfig.CharacterData targetData)
	{
		if (targetData.Cost <= pointLeft) {
			pointLeft -= targetData.Cost;
		}else
			return;
		var scene = UPerceptionView.Singleton.UScene;
		var per = this.state.Perception as BattlePerception;
		var target =  per.CreateCharacter(targetData,2,
			new EngineCore.GVector3(scene.enemyStartPoint.position.x,
				scene.enemyStartPoint.position.y,scene.enemyStartPoint.position.z),
			new EngineCore.GVector3(0,-90,0));
		per.State.AddElement (target);
		per.ChangeCharacterAI (targetData.AIResourcePath, target);
	}

	public override  GTime GetTime ()
	{
		return new GTime (Time.time, Time.deltaTime);
	}

	private float lastTime = 0;

	public void Load (GState state)
	{
		 //empty
		var tower = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData>(data.TowerID);
		var scene = UPerceptionView.Singleton.UScene;
		var per = state.Perception as BattlePerception;
		var character =  per.CreateCharacter(tower,1,
			new EngineCore.GVector3(scene.tower.position.x,
				scene.tower.position.y,scene.tower.position.z),
			new EngineCore.GVector3(0,0,0));
		//per.State.AddElement (releaser);
		per.State.AddElement (character);
		per.ChangeCharacterAI (tower.AIResourcePath, character);


		var target =  per.CreateCharacter(tower,2,
			new EngineCore.GVector3(scene.towerEnemy.position.x,
				scene.towerEnemy.position.y,scene.towerEnemy.position.z),
			new EngineCore.GVector3(0,0,0));
		//per.State.AddElement (releaser);
		state.AddElement (target);
		per.ChangeCharacterAI (tower.AIResourcePath, target);
	}


	public void CreateCharacter(ExcelConfig.CharacterData data)
	{
		if (data.Cost <= pointRight) {
			pointRight -= data.Cost;
		} else
			return;
		var scene = UPerceptionView.Singleton.UScene;
		var per = this.state.Perception as BattlePerception;
		var character =  per.CreateCharacter(data,1,
			new EngineCore.GVector3(scene.startPoint.position.x,
				scene.startPoint.position.y,scene.startPoint.position.z),
			new EngineCore.GVector3(0,90,0));
		//per.State.AddElement (releaser);
		per.State.AddElement (character);
		per.ChangeCharacterAI (data.AIResourcePath, character);
	}

	public float LeftTime { get { return Mathf.Max (0, startTime + data.LimitTime - this.GetTime ().Time);} }

}