using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleMissile:IBattleElement
	{
		ITransform Transform { get; }
	}
}

