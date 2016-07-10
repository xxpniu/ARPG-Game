using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using GameLogic.Game.Controllors;
using Layout.LayoutElements;
using System.Collections.Generic;
using EngineCore;
using GameLogic.Game.LayoutLogics;
using Layout;
using Layout.AITree;
using GameLogic.Game.States;
using GameLogic.Game.AIBehaviorTree;

namespace GameLogic.Game.Perceptions
{
	public class BattlePerception : GPerception
	{
		public BattlePerception(GState state, IBattlePerception view) : base(state)
		{
			View = view;
			BattleCharacterControllor = new BattleCharacterControllor(this);
			ReleaserControllor = new MagicReleaserControllor(this);
			BattleMissileControllor = new BattleMissileControllor(this);
			AIControllor = new BattleCharacterAIBehaviorTreeControllor(this);

		}

		public IBattlePerception View { private set; get; }

		//初始化游戏中的控制器 保证唯一性
		public BattleCharacterControllor BattleCharacterControllor { private set; get; }
		public BattleMissileControllor BattleMissileControllor { private set; get; }
		public MagicReleaserControllor ReleaserControllor { private set; get; }
		public BattleCharacterAIBehaviorTreeControllor AIControllor { private set; get; }

		public MagicReleaser CreateReleaser(string key, IReleaserTarget target)
		{
			var magic = View.GetMagicByKey(key);
			return CreateReleaser(magic, target);
		}

		public MagicReleaser CreateReleaser(MagicData magic, IReleaserTarget target)
		{
			var view = View.CreateReleaserView(target.Releaser.View, target.ReleaserTarget.View, target.TargetPosition);
			var mReleaser = new MagicReleaser(magic, target, this.ReleaserControllor, view);
			return mReleaser;
		}

		public BattleMissile CreateMissile(MissileLayout layout, MagicReleaser releaser)
		{
			var view = this.View.CreateMissile(releaser.View, layout);
			return new BattleMissile(BattleMissileControllor,releaser,view,layout);
		}

		public BattleCharacter CreateCharacter(ExcelConfig.CharacterData data, int teamIndex, GVector3 position, GVector3 forward)
		{
			var res = data.ResourcesPath;
			var view = View.CreateBattleCharacterView(res, position, forward);
			//no used
			//var magics = ExcelConfig.ExcelToJSONConfigManager.Current
			//					   .GetConfigs<ExcelConfig.CharacterMagicData>(t => { return t.CharacterID == data.ID; });
			var battleCharacter = new BattleCharacter(this.BattleCharacterControllor, view);
			battleCharacter.HPMax.SetBaseValue(data.HPMax);
			battleCharacter.Defence.SetBaseValue(data.Defance);
			battleCharacter.DamageMin.SetBaseValue(data.DamageMin);
			battleCharacter.DamageMax.SetBaseValue(data.DamageMax);
			battleCharacter.Attack.SetBaseValue(data.Attack);
			battleCharacter.Level = data.Level;
			battleCharacter.TDamage = (Layout.LayoutEffects.DamageType)data.DamageType;
			battleCharacter.TDefance = (DefanceType)data.DefanceType;
			battleCharacter.TBody = (BodyType)data.BodyType;
			battleCharacter.TAttack = (AttackType)data.AttackType;
			battleCharacter.Name = data.Name;
			battleCharacter.TeamIndex = teamIndex;
			battleCharacter.Init();
			return battleCharacter;
		}

		public AITreeRoot ChangeCharacterAI(string pathTree, BattleCharacter character)
		{
			TreeNode ai = View.GetAITree(pathTree);
			return ChangeCharacterAI(ai, character);
		}

		public AITreeRoot ChangeCharacterAI(TreeNode ai, BattleCharacter character)
		{
			var comp = AIBehaviorTree.AITreeParse.CreateFrom(ai);
			//var state = State as BattleState;
			var root = new AIBehaviorTree.AITreeRoot(View.GetTimeSimulater(), character, comp);
			character.AIRoot = root;
			character.SetControllor(AIControllor);

			return root;
		}

		//获取一个非本阵营目标
		public BattleCharacter GetSingleTargetUseRandom(BattleCharacter owner)
		{
			BattleCharacter target = null;

			this.State.Each<BattleCharacter>((t) =>
			{
				if (t.TeamIndex != owner.TeamIndex)
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
			float radius, float angle, float offsetAngle, GVector3 offset)
		{
			switch (damageType)
			{
				case DamageType.Single://单体直接对目标
					return new List<BattleCharacter> { character };
				case DamageType.Rangle:
					{
						var orgin = character.View.Transform.Position + offset;
						var forward = character.View.Transform.Forward;

						forward = View.RotateWithY(forward, offsetAngle);

						var list = new List<BattleCharacter>();
						State.Each<BattleCharacter>((t) =>
						{

							//过滤
							switch (fitler)
							{
								case FilterType.Alliance:
								case FilterType.OwnerTeam:
									if (character.TeamIndex != t.TeamIndex) return false;
									break;
								case FilterType.EmenyTeam:
									if (character.TeamIndex == t.TeamIndex) return false;
									break;

							}
							//不在目标区域内
							if (View.Distance(orgin, t.View.Transform.Position) > radius) return false;
							if (View.Angle(forward, t.View.Transform.Forward) > (angle / 2)) return false;

							list.Add(t);
							return false;
						});
						return list;
					}
			}

			return new List<BattleCharacter>();
		}

		public float Distance(BattleCharacter c1, BattleCharacter c2)
		{
			return View.Distance(c1.View.Transform.Position, c2.View.Transform.Position);
		}
	}
}

