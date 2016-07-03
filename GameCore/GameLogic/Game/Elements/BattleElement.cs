using System;
using EngineCore.Simulater;

namespace GameLogic.Game.Elements
{
	public class BattleElement<T>:GObject  where T:IBattleElement
	{
		public BattleElement (GControllor controllor,T view):base(controllor)
		{
			View = view;
		}

		public T View{ private set; get; }
			
		protected override void OnJoinState ()
		{
			base.OnJoinState ();
			View.JoinState (this);

		}

		protected override void OnExitState ()
		{
			base.OnExitState ();
			View.ExitState (this);
		}
	}
}

