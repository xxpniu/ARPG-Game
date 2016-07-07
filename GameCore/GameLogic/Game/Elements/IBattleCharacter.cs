using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleCharacter:IBattleElement
	{
		GVector3 GetPosition();
		GVector3 GetForward();
		void SetPosition(GVector3 pos);
		void SetForward(GVector3 forward);
		void PlayMotion(string motion);
	}
}

