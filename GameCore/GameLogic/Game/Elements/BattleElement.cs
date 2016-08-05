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
			base.OnJoinState ();
			View.JoinState (this);
            if (OnJoinedState != null) OnJoinedState(this);

            var per = this.Controllor.Perception as BattlePerception;
            per.AddNotify(new Proto.Notify_ElementJoinState { Index = Index});

		}

		protected override void OnExitState ()
		{
			base.OnExitState ();
			View.ExitState (this);
            if (OnExitedState != null) OnExitedState(this);
            var per = this.Controllor.Perception as BattlePerception;
            per.AddNotify(new Proto.Notify_ElementExitState { Index = Index});
		}

		public HanlderEvent OnJoinedState;
		public HanlderEvent OnExitedState;
	}
}

