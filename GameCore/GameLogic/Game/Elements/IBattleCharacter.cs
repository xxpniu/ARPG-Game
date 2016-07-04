using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleCharacter:IBattleElement
	{
		GVector3 GetPosition();
		GVector3 GetForward();
		void PlayMotion(string motion);
	}
}

