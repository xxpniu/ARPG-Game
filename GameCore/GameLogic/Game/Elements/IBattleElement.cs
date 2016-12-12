using System;
using EngineCore.Simulater;

namespace GameLogic.Game.Elements
{
	public interface IBattleElement
	{
		void JoinState(int index);
        void ExitState(int index);
        void AttachElement(GObject el);
        int Index { set; get; }
	}
}

