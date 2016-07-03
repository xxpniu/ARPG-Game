using System;
using GameLogic.Game.Perceptions;
using EngineCore.Simulater;

namespace GameLogic.Game.States
{
	
	public class BattleState:GState
	{
		public BattleState (IViewBase viewBase)
		{
			ViewBase = viewBase;
			Perception = new BattlePerception (this, viewBase.Create ());
		}

		public IViewBase ViewBase{ private set; get; }

		protected override void OnInit ()
		{
			base.OnInit ();
		}
	}
}

