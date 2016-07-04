using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleMissle:IBattleElement
	{
		GVector3 GetPosition();
		GVector3 GetForward();
	}
}

