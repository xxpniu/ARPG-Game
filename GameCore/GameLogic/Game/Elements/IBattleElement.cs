using System;
using EngineCore.Simulater;

namespace GameLogic.Game.Elements
{
	public interface IBattleElement
	{
		void JoinState(GObject el);
		void ExitState(GObject el);
        long Index { set; get; }
	}
}

