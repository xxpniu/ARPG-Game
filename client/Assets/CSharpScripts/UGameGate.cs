using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLogic.Game.States;
using GameLogic;
using GameLogic.Game.Perceptions;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Game.Elements;
using ExcelConfig;

public class UGameGate:UGate,IStateLoader
{
	public UGameGate (int levelID)
    {
        this.data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.LevelData>(levelID);
    }

	//private ExcelConfig.CharacterData[] characters;
	private ExcelConfig.LevelData data;
    private List<BattleCharacter> towers = new List<BattleCharacter>();

    public LevelData LevelData{ get { return data; } }
	public float pointLeft = 0;
	public float pointRight = 0;

	private AsyncOperation operation;
	public override void JoinGate ()
	{
		UUIManager.Singleton.ShowMask (true);
		UUIManager.Singleton.ShowLoading (0);
        pointLeft = data.PointBegin;
        pointRight = data.MonsterPoint;
        operation = SceneManager.LoadSceneAsync (this.data.LevelResouceName, LoadSceneMode.Single);
        UUIManager.Singleton.HideAll();
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
            var ui =UUIManager.Singleton.CreateWindow<Windows.UUIBattle> ();
			ui.ShowWindow ();
		}

		if (state == null)
			return;

        if (state.IsEnable)
        {
            foreach (var i in towers)
            {
                if (i.IsDeath)
                {
                    state.Pause(true);
                    var battleUI = UUIManager.Singleton.GetUIWindow<Windows.UUIBattle>();
                    if (battleUI != null)
                        battleUI.HideWindow();
                    var ui = UUIManager.Singleton.CreateWindow<Windows.UUIBattleResult>();
                    ui.ShowWindow();
                    return;
                }
            }
        }
        else
        {
            return;
        }


		GState.Tick (state, this.GetTime ());

        if (nextCharacter == null)
        {
            var pro = data.AILogic.Split('|');
            var keyValue = new List<int[]>();
            foreach (var i in pro)
            {
                var vals = i.Split(',');
                keyValue.Add(new int[]{ int.Parse(vals[0]), int.Parse(vals[1]) });
            }

            var index = GRandomer.RandPro(keyValue.Select(t => t[1]).ToArray());
            var id = keyValue[index][0];
            nextCharacter = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData>(id);
        }
        else
        {
            if (lastTimeLimit < Time.time)
            {
                if (pointRight >= nextCharacter.Cost)
                {
                    lastTimeLimit = Time.time + data.Limit;
                    CreateTarget(nextCharacter);
                    nextCharacter = null;
                }
            }
        }

		if (lastTime > this.GetTime().Time)
			return;
        
		lastTime = this.GetTime ().Time + 1f;
        AddPoint(data.PointLeftAdd);
        pointRight=  Math.Min( pointRight + data.PointRightAdd,data.MaxPoint);
	}

    private float lastTimeLimit = 0f;


    private void AddPoint(float point)
    {
        pointLeft = Math.Min(pointLeft + point,data.MaxPoint);
    }



    private ExcelConfig.CharacterData nextCharacter;
   
	private void CreateTarget(ExcelConfig.CharacterData targetData)
	{
        if (targetData.Cost <= pointRight) {
            pointRight -= targetData.Cost;
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
        target.OnDead += (el) =>
        {
                var charcater = el as BattleCharacter;
                var data = ExcelConfig.ExcelToJSONConfigManager.Current
                    .GetConfigByID<ExcelConfig.CharacterData>(charcater.ConfigID);
                AddPoint(data.Cost * LevelData.PointGetRate );
        };
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

        towers.Add(character);
		var target =  per.CreateCharacter(tower,2,
			new EngineCore.GVector3(scene.towerEnemy.position.x,
				scene.towerEnemy.position.y,scene.towerEnemy.position.z),
			new EngineCore.GVector3(0,0,0));
		//per.State.AddElement (releaser);
		state.AddElement (target);
		per.ChangeCharacterAI (tower.AIResourcePath, target);
        towers.Add(target);


	}

   

	public void CreateCharacter(ExcelConfig.CharacterData data)
	{
        if (data.Cost <= pointLeft) {
            pointLeft -= data.Cost;
		} else
			return;
		var scene = UPerceptionView.Singleton.UScene;
		var per = this.state.Perception as BattlePerception;
		var character =  per.CreateCharacter(data,1,
			new EngineCore.GVector3(scene.startPoint.position.x,
				scene.startPoint.position.y,scene.startPoint.position.z),
			new EngineCore.GVector3(0,90,0));
		per.State.AddElement (character);
		per.ChangeCharacterAI (data.AIResourcePath, character);
        character.OnDead += (el) =>
        {
                var c = el as BattleCharacter;
                var t = ExcelConfig.ExcelToJSONConfigManager.Current
                    .GetConfigByID<ExcelConfig.CharacterData>(c.ConfigID); 
                pointRight += t.Cost * this.data.MonsterGetRate;
        };
	}

	public float LeftTime 
    { 
        get 
        {
            if (state == null || !state.IsEnable)
                return 0;
            return Mathf.Max (0, startTime + data.LimitTime - this.GetTime ().Time);
        } 
    }

}