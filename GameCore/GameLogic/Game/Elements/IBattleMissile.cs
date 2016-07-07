using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleMissile:IBattleElement
	{
		GVector3 GetPosition();
		GVector3 GetForward();
	}
}

