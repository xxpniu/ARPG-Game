using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLogic.Game.States;
using GameLogic;

public class UGameGate:UGate,IStateLoader
{
	public UGameGate (int levelID)
	{
		this.data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.LevelData> (levelID);
	}	

	private ExcelConfig.LevelData data;

	private AsyncOperation operation;
	public override void JoinGate ()
	{
		operation = SceneManager.LoadSceneAsync ("Level1");
	}

	public override void ExitGate ()
	{
		if (state != null)
			state.Stop (this.GetTime());
	}

	private GState state;

	public override void Tick ()
	{
		if (operation != null) {
			if (!operation.isDone)
				return;
			operation = null;
			state = new BattleState (UView.Singleton, this);
			state.Init ();
			state.Start (this.GetTime());
		}

		if (state == null)
			return;
		GState.Tick (state, this.GetTime ());
	}

	public override  GTime GetTime ()
	{
		return new GTime (Time.time, Time.deltaTime);
	}


	public void Load (GState state)
	{
		 //empty
	}


}