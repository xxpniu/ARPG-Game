using System;
using GameLogic.Game.Perceptions;

namespace GameLogic
{
	public interface IViewBase
	{
		IBattlePerception Create();
	}
}

