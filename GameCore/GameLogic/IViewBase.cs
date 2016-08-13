using System;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game.Perceptions;

namespace GameLogic
{
	public interface IViewBase
	{
		IBattlePerception Create(ITimeSimulater simulater);
	}
}

