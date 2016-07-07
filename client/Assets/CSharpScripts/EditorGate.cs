using System;
using GameLogic;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLogic.Game.Perceptions;
using GameLogic.Game.Elements;

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
			//throw new NotImplementedException ();
			var per = state.Perception as BattlePerception;

		}
		#endregion
		
	}

	public const string EDITOR_LEVEL_NAME ="EditorReleaseMagic";

	#region implemented abstract members of UGate


	private  AsyncOperation operation;

	public override void JoinGate ()
	{
		operation= SceneManager.LoadSceneAsync (EDITOR_LEVEL_NAME);
	}

	private GState curState;

	public override void ExitGate ()
	{
		curState.Stop (CTime);
	}

	public override void Tick ()
	{
		if (operation != null) {
			if (operation.isDone) {
				curState = new GameLogic.Game.States.BattleState(UView.Singleton, new StateLoader(this));
				curState.Start (CTime);
				operation = null;
			}
		}
		if (curState != null) 
		{
			GState.Tick (curState, CTime );
		}
	}

	#endregion

	public GTime CTime{ get { return new GTime (){ DetalTime = Time.deltaTime, Time = Time.time }; } }

	public MagicReleaser currentReleaser;

	public BattleCharacter releaser;
	public BattleCharacter target;
}
