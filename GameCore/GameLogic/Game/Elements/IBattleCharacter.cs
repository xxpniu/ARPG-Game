using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleCharacter:IBattleElement
	{
		GVector3 GetPosition();
	}
}

