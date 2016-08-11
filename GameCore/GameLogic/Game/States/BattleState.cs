using System;
using GameLogic.Game.Perceptions;
using EngineCore.Simulater;

namespace GameLogic.Game.States
{
	
	public class BattleState:GState
	{
        public BattleState (IViewBase viewBase,IStateLoader loader, ITimeSimulater simulater)
		{
			ViewBase = viewBase;
			Perception = new BattlePerception (this, viewBase.Create (simulater));
			loader.Load (this);
		}

		public IViewBase ViewBase{ private set; get; }


		protected override void OnInit ()
		{
			base.OnInit ();

		}
	}
}

