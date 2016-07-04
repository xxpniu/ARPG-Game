using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using GameLogic.Game.Controllors;
using Layout.LayoutElements;
using System.Collections.Generic;
using EngineCore;
using GameLogic.Game.LayoutLogics;

namespace GameLogic.Game.Perceptions
{
	public class BattlePerception:GPerception
	{
		public BattlePerception (GState state,IBattlePerception view):base(state)
		{
			View = view;
			BattleCharacterControllor = new BattleCharacterControllor (this);
			ReleaserControllor = new MagicReleaserControllor (this);
		}

		public IBattlePerception View{private set; get; }

		//初始化游戏中的控制器 保证唯一性
		public BattleCharacterControllor BattleCharacterControllor{ private set; get; }

		public MagicReleaserControllor ReleaserControllor{ private set; get; }

		public MagicReleaser CreateReleaser(string key,IReleaserTarget target)
		{
			var view = View.CreateReleaserView (target.Releaser.View, target.ReleaserTarget.View, target.TargetPosition);
			var magic = View.GetMagicByKey (key);
			var mReleaser = new MagicReleaser (magic, target, this.ReleaserControllor, view);
			return mReleaser;
		}

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

		/// <summary>
		/// Finds the target.
		/// </summary>
		/// <returns>The target.</returns>
		/// <param name="character">Character.</param>
		/// <param name="fitler">Fitler.</param>
		/// <param name="damageType">Damage type.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="offsetAngle">Offset angle.</param>
		/// <param name="offset">Offset.</param>
		public List<BattleCharacter> FindTarget(
			BattleCharacter character, 
			FilterType fitler,
			DamageType damageType,
			float radius, float angle, float offsetAngle,GVector3 offset )
		{
			switch (damageType) {
			case DamageType.Single://单体直接对目标
				return new List<BattleCharacter>{ character };
			case DamageType.Rangle:
				{
					var orgin = character.View.GetPosition () + offset ;
					var forward =  character.View.GetForward ();

					forward = View.RotateWithY (forward, offsetAngle);

					var list = new List<BattleCharacter> ();
					State.Each<BattleCharacter> ((t) => {
					  
						//过滤
						switch(fitler)
						{
						case FilterType.Alliance:
						case FilterType.OwnerTeam:
							if(character.TeamIndex != character.TeamIndex) return false;
							break;
						case FilterType.EmenyTeam:
							if(character.TeamIndex == character.TeamIndex) return false;
							break;
						
						}
						//不在目标区域内
						if(View.Distance(orgin,t.View.GetPosition())>radius) return false;
						if(View.Angle(forward,t.View.GetForward())>(angle/2))return false;

						list.Add(t);
						return false;
					});
					return list;
				}
			}

			return new List<BattleCharacter> ();
		}

	}
}

