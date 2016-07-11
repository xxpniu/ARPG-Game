using System;
using EngineCore;

namespace GameLogic.Game.Elements
{
	public interface IBattleCharacter:IBattleElement
	{
		ITransform Transform { get; }
		void SetPosition(GVector3 pos);
		void SetForward(GVector3 eulerAngles);
		void PlayMotion(string motion);
		void LookAt(ITransform target);
		void MoveTo(GVector3 position);
		void StopMove();
		void Death();
	}
}

