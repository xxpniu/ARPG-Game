using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using GameLogic.Game.Controllors;

namespace GameLogic.Game.Perceptions
{
	public class BattlePerception:GPerception
	{
		public BattlePerception (GState state,IBattlePerception view):base(state)
		{
			View = view;
			BattleCharacterControllor = new BattleCharacterControllor (this);
		}

		public IBattlePerception View{private set; get; }

		//初始化游戏中的控制器 保证唯一性
		public BattleCharacterControllor BattleCharacterControllor{ private set; get; }

		//获取一个非本阵营目标
		public BattleCharacter GetSingleTargetUseRandom(BattleCharacter owner)
		{
			BattleCharacter target = null;

			this.State.Each<BattleCharacter> ((t) => {
				if(t.TeamIndex != owner.TeamIndex)
				{
					target = t;
					return true;
				}
				return false;
			});

			return target;
		}

	}
}

