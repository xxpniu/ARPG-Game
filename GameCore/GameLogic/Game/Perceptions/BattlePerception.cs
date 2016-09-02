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
using Proto;
using Layout.LayoutEffects;
using ExcelConfig;
using System.Linq;

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
            view.SetPercetion(this);
		}


        public int GetEnemyTeamIndex(int teamIndex)
		{
			if (teamIndex == 1)
			{
				return 2;
			}
			else { return 1; }
		}

		public IBattlePerception View { private set; get; }

        public List<GObject> GetEnableElements()
        {
            var list = new List<GObject>();
            this.State.Each<GObject>((el) =>
            {
                list.Add(el);
                return false;
            });
            return list;
        }

		//初始化游戏中的控制器 保证唯一性
		public BattleCharacterControllor BattleCharacterControllor { private set; get; }
        public BattleMissileControllor BattleMissileControllor { private set; get; }
		public MagicReleaserControllor ReleaserControllor { private set; get; }
		public BattleCharacterAIBehaviorTreeControllor AIControllor { private set; get; }

        public MagicReleaser CreateReleaser(string key, IReleaserTarget target,ReleaserType ty)
		{
			var magic = View.GetMagicByKey(key);
			var releaser= CreateReleaser(magic, target,ty);
            return releaser;
		}

        public MagicReleaser CreateReleaser(MagicData magic, IReleaserTarget target,ReleaserType ty)
		{
			var view = View.CreateReleaserView(target.Releaser.View, target.ReleaserTarget.View, target.TargetPosition);
            var mReleaser = new MagicReleaser(magic, target, this.ReleaserControllor, view,ty);
            this.JoinElement(mReleaser);
			return mReleaser;
		}


        public BattleMissile CreateMissile(MissileLayout layout, MagicReleaser releaser)
		{
			var view = this.View.CreateMissile(releaser.View, layout);
			var missile= new BattleMissile(BattleMissileControllor,releaser,view,layout);
            this.JoinElement(missile);
            return missile;
		}


        #region Character
        public BattleCharacter CreateCharacter(
            int level,
            CharacterData data,
            List<CharacterMagicData> magics,
            int teamIndex, 
            GVector3 position,
            GVector3 forward,long userID)
		{
			var res = data.ResourcesPath;
			var view = View.CreateBattleCharacterView(res, position, forward);
            var battleCharacter = new BattleCharacter(data.ID,magics, this.BattleCharacterControllor, view, userID);
            battleCharacter[HeroPropertyType.MaxHP].SetBaseValue(data.HPMax);
            battleCharacter[HeroPropertyType.MaxMP].SetBaseValue(data.MPMax);
            battleCharacter[HeroPropertyType.Defance].SetBaseValue(data.Defance);
            battleCharacter[HeroPropertyType.DamageMin].SetBaseValue(data.DamageMin);
            battleCharacter[HeroPropertyType.DamageMax].SetBaseValue(data.DamageMax);
            battleCharacter[HeroPropertyType.Agility].SetBaseValue(data.Agility + (int)(level* data.AgilityGrowth));
            battleCharacter[HeroPropertyType.Force].SetBaseValue(data.Force+(int)(level*data.ForceGrowth));
            battleCharacter[HeroPropertyType.Knowledge].SetBaseValue(data.Knowledge +(int)(level*data.KnowledgeGrowth));
            battleCharacter[HeroPropertyType.MagicWaitTime].SetBaseValue((int)(data.AttackSpeed*1000));
            battleCharacter[HeroPropertyType.ViewDistance].SetBaseValue((int)(data.ViewDistance * 100));
            battleCharacter.Level = level;
			battleCharacter.TDamage = (Proto.DamageType)data.DamageType;
			battleCharacter.TDefance = (DefanceType)data.DefanceType;
            battleCharacter.Category = (HeroCategory)data.Category;
            battleCharacter.Name = data.Name;
			battleCharacter.TeamIndex = teamIndex;
			battleCharacter.Speed = data.MoveSpeed;
			view.SetPriorityMove(data.PriorityMove);
			battleCharacter.Init();
            this.JoinElement(battleCharacter);
			return battleCharacter;
		}

        internal IParticlePlayer CreateParticlePlayer(MagicReleaser relaser, ParticleLayout layout)
        {
            var p= View.CreateParticlePlayer(relaser.View, layout);
            return p;
        }

        internal void CharacterMoveTo(BattleCharacter character, GVector3 pos)
        {
           character.View.MoveTo(pos);
        }

        internal void CharacterStopMove(BattleCharacter character)
        {
            character.View.StopMove();
        }

        internal void PlayMotion(BattleCharacter releaser, string motionName)
        {
            releaser.View.PlayMotion(motionName);

        }

        internal void LookAtCharacter(BattleCharacter own, BattleCharacter target)
        {
            own.View.LookAtTarget(target.View);
        }

        internal void ProcessDamage(BattleCharacter sources,BattleCharacter effectTarget,DamageResult result)
        {
            View.ProcessDamage( sources.View,effectTarget.View, result);
            if (result.IsMissed) return;
            CharacterSubHP(effectTarget, result.Damage);
        }

        public void CharacterSubHP(BattleCharacter effectTarget, int lostHP)
        {
            effectTarget.SubHP(lostHP);
        }

        public void CharacterAddHP(BattleCharacter effectTarget, int addHp)
        {
            effectTarget.AddHP(addHp);
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
			var root = new AIBehaviorTree.AITreeRoot(View.GetTimeSimulater(), character, comp,ai);
            character.SetAITree( root);
			character.SetControllor(AIControllor);

			return root;
		}

        #endregion

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
        /// <param name="teamIndex">team</param>
		public List<BattleCharacter> FindTarget(
			BattleCharacter character,
			FilterType fitler,
            Layout.LayoutElements.DamageType damageType,
            float radius, float angle, float offsetAngle, GVector3 offset,int teamIndex)
		{
			switch (damageType)
			{
				case Layout.LayoutElements.DamageType.Single://单体直接对目标
					return new List<BattleCharacter> { character };
				case Layout.LayoutElements.DamageType.Rangle:
					{
						var orgin = character.View.Transform.Position + offset;
						var forward = character.View.Transform.Forward;

                        var q = GQuaternion.Identity;
                        q.Y = angle;
                        forward = GVector3.Transform(forward, q);

						var list = new List<BattleCharacter>();
						State.Each<BattleCharacter>((t) =>
						{

							//过滤
							switch (fitler)
							{
								case FilterType.Alliance:
								case FilterType.OwnerTeam:
                                        if (teamIndex != t.TeamIndex) return false;
									break;
								case FilterType.EmenyTeam:
                                        if (teamIndex == t.TeamIndex) return false;
									break;

							}
							//不在目标区域内
                            if ((orgin-t.View.Transform.Position).Length > radius) return false;
                            if (angle < 360)
                            {
                                if (GVector3.CalculateAngle(forward, t.View.Transform.Forward) > (angle / 2)) return false;
                            }
							list.Add(t);
							return false;
						});
						return list;
					}
			}

			return new List<BattleCharacter>();
		}

        public void StopAllReleaserByCharacter(BattleCharacter character)
        {
            State.Each<MagicReleaser>(t => {
                if (t.ReleaserTarget.Releaser == character)
                {
                    t.SetState(ReleaserStates.Ended);//防止AI错误
                    GObject.Destory(t);
                }
                return false;
            });
        }

        public void BreakReleaserByCharacter(BattleCharacter character, BreakReleaserType type)
        {
            State.Each<MagicReleaser>(t =>
            {
                if (t.ReleaserTarget.Releaser == character)
                {
                    switch (type)
                    {
                        case BreakReleaserType.InStartLayoutMagic:
                            {
                                if (t.RType == ReleaserType.Magic)
                                {
                                    if (!t.IsLayoutStartFinish)
                                    {
                                        t.StopAllPlayer();
                                    }
                                    t.SetState(ReleaserStates.ToComplete);
                                }
                            }
                            break;
                        case BreakReleaserType.Buff:
                            {
                                if (t.RType == ReleaserType.Buff)
                                {
                                    t.SetState(ReleaserStates.ToComplete);
                                }
                            }
                            break;
                        case BreakReleaserType.ALL:
                            {
                                t.SetState(ReleaserStates.ToComplete);
                            }
                            break;
                    }
                   
                  
                }
                return false;
            });
        }


        public float Distance(BattleCharacter c1, BattleCharacter c2)
        {
            return Math.Max(0, (c1.View.Transform.Position - c2.View.Transform.Position).Length-1);
        }
	}
}

