using System;
using System.Collections.Generic;
using EngineCore;
using GameLogic.Utility;
using Proto;
using UMath;

namespace GameLogic.Game.Elements
{
	public interface IBattleCharacter:IBattleElement
	{
        UTransform Transform { get; }

        void SetPosition(UVector3 pos);
		void SetForward(UVector3 forward);
        void LookAtTarget(IBattleCharacter target);

        [NeedNotify(typeof(Notify_LayoutPlayMotion),"Motion")]
		void PlayMotion(string motion);
        void MoveTo(UVector3 position);
		void StopMove();
		void Death();
		void SetSpeed(float _speed);
		void SetPriorityMove(float priorityMove);
		void SetScale(float scale);
        void ShowHPChange(int hp,int cur,int max);
        void ShowMPChange(int mp, int cur, int maxMP);
        void ProtertyChange(HeroPropertyType type, int finalValue);
        bool IsMoving { get; }
        void AttachMaigc(int magicID, float cdCompletedTime);
        void SetAlpha(float alpha);
    }
}

