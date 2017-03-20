using System;
using EngineCore.Simulater;
using GameLogic.Game.Perceptions;

namespace GameLogic.Game.Elements
{
    public delegate void HanlderEvent(GObject el);

	public class BattleElement<T>: GObject  where T:IBattleElement
	{
		public BattleElement (GControllor controllor,T view):base(controllor)
		{
			View = view;
		}

		public T View{ private set; get; }
			
		protected override void OnJoinState ()
		{
			base.OnJoinState();
            View.AttachElement(this);
            View.JoinState (this.Index);
            if (OnJoinedState != null) OnJoinedState(this);


		}

		protected override void OnExitState ()
		{
			base.OnExitState ();
            View.ExitState (this.Index);
            if (OnExitedState != null) OnExitedState(this);
           
		}

		public HanlderEvent OnJoinedState;
		public HanlderEvent OnExitedState;
	}
}

