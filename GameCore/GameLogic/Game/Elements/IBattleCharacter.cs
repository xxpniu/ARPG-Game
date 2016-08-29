using System;
using System.Collections.Generic;
using EngineCore;
using Proto;

namespace GameLogic.Game.Elements
{
	public interface IBattleCharacter:IBattleElement
	{
		ITransform Transform { get; }
		void SetPosition(GVector3 pos);
		void SetForward(GVector3 eulerAngles);
		void PlayMotion(string motion);
		void LookAtTarget(IBattleCharacter target);
        void MoveTo(GVector3 position);
		void StopMove();
		void Death();
		void SetSpeed(float _speed);
		void SetPriorityMove(float priorityMove);
		void SetScale(float scale);
        void ShowHPChange(int hp,int cur,int max);
        void ShowMPChange(int mp, int cur, int maxMP);
        void ProtertyChange(HeroPropertyType type, int finalValue);
        bool IsMoving { get; }
    }
}

