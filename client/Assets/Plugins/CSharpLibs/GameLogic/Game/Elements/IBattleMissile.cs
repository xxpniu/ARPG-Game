using System;
using EngineCore;
using UMath;

namespace GameLogic.Game.Elements
{
	public interface IBattleMissile:IBattleElement
	{
		UTransform Transform { get; }
	}
}

