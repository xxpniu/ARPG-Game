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

        private Queue<ISerializerable> NotifyMessage = new Queue<ISerializerable>();

        public ISerializerable[] GetNotifyMessageAndClear()
        {
            var result= NotifyMessage.ToArray();
            NotifyMessage.Clear();
            return result;
        }

        public void AddNotify(ISerializerable message)
        {
            NotifyMessage.Enqueue(message);
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
            var createNotify = new Proto.Notify_CreateReleaser {
                Index  = mReleaser.Index,
                ReleaserIndex = target.Releaser.Index,
                TargetIndex = target.ReleaserTarget.Index,
                MagicKey = magic.key
            };
            AddNotify(createNotify);
            this.JoinElement(mReleaser);
			return mReleaser;
		}

        public BattleMissile CreateMissile(MissileLayout layout, MagicReleaser releaser)
		{
			var view = this.View.CreateMissile(releaser.View, layout);
			var missile= new BattleMissile(BattleMissileControllor,releaser,view,layout);
            var createNotify = new Proto.Notify_CreateMissile
            {
                Index = missile.Index,
                Position = missile.View.Transform.Position.ToV3(),
                ResourcesPath = layout.resourcesPath,
                Speed = layout.speed,
                ReleaserIndex = releaser.Index,
                formBone = layout.fromBone,
                toBone = layout.toBone,
                offset = layout.offset.ToV3()
            };
            AddNotify(createNotify);
            this.JoinElement(missile);
            return missile;
		}

       


        #region Character
        public BattleCharacter CreateCharacter(
            int level,
            CharacterData data,
            int teamIndex, 
            GVector3 position,
            GVector3 forward,long userID)
		{
			var res = data.ResourcesPath;
			var view = View.CreateBattleCharacterView(res, position, forward);
            var battleCharacter = new BattleCharacter(data.ID, this.BattleCharacterControllor, view, userID);
            battleCharacter[HeroPropertyType.MaxHP].SetBaseValue(data.HPMax);
            battleCharacter[HeroPropertyType.MaxMP].SetBaseValue(data.MPMax);
            battleCharacter[HeroPropertyType.Defance].SetBaseValue(data.Defance);
            battleCharacter[HeroPropertyType.DamageMin].SetBaseValue(data.DamageMin);
            battleCharacter[HeroPropertyType.DamageMax].SetBaseValue(data.DamageMax);
            battleCharacter[HeroPropertyType.Agility].SetBaseValue(data.Agility + (int)(level* data.AgilityGrowth));
            battleCharacter[HeroPropertyType.Force].SetBaseValue(data.Force+(int)(level*data.ForceGrowth));
            battleCharacter[HeroPropertyType.Knowledge].SetBaseValue(data.Knowledge +(int)(level*data.KnowledgeGrowth));
            battleCharacter[HeroPropertyType.MagicWaitTime].SetBaseValue((int)(data.AttackSpeed*1000));
            battleCharacter.Level = level;
			battleCharacter.TDamage = (Proto.DamageType)data.DamageType;
			battleCharacter.TDefance = (DefanceType)data.DefanceType;
            battleCharacter.Name = data.Name;
			battleCharacter.TeamIndex = teamIndex;
			battleCharacter.Speed = data.MoveSpeed;
			view.SetPriorityMove(data.PriorityMove);
			battleCharacter.Init();

            var properties = new List<HeroProperty>();
            foreach (var i in Enum.GetValues(typeof(HeroPropertyType)))
            {
                var p = (HeroPropertyType)i;
                properties.Add(new HeroProperty { Property = p, Value = battleCharacter[p].FinalValue });
            }
            var createNotity = new Proto.Notify_CreateBattleCharacter
            {
                Index = battleCharacter.Index,
                UserID = userID,
                ConfigID = battleCharacter.ConfigID,
                Position = view.Transform.Position.ToV3(),
                Forward = view.Transform.Forward.ToV3(),
                HP = battleCharacter.HP,
                Property = properties,
                Level = battleCharacter.Level,
                TDamage = battleCharacter.TDamage,
                TDefance = battleCharacter.TDefance,
                Name = battleCharacter.Name,
                Category = battleCharacter.Category,
                TeamIndex = battleCharacter.TeamIndex,
                Speed = battleCharacter.Speed
            };
            AddNotify(createNotity);
            this.JoinElement(battleCharacter);
			return battleCharacter;
		}

        internal IParticlePlayer CreateParticlePlayer(MagicReleaser relaser, ParticleLayout layout)
        {
            var p= View.CreateParticlePlayer(relaser.View, layout);

            var notify = new Proto.Notify_LayoutPlayParticle
            {
                ReleaseIndex = relaser.Index,
                FromTarget = (int)layout.fromTarget,
                ToTarget = (int)layout.toTarget,
                Path = layout.path,
                ToBoneName = layout.toBoneName,
                FromBoneName = layout.fromBoneName,
                DestoryTime = layout.destoryTime,
                DestoryType = (int)layout.destoryType
            };
            AddNotify(notify);
            return p;
        }

        internal void CharacterMoveTo(BattleCharacter character, GVector3 pos)
        {
            character.View.MoveTo(pos);
            var notify = new Proto.Notify_CharacterBeginMove 
            {
                Index = character.Index,
                StartForward = character.View.Transform.Forward.ToV3(),
                StartPosition = character.View.Transform.Position.ToV3(),
                TargetPosition = pos.ToV3(),
                Speed = character.Speed
            };
            AddNotify(notify);
        }

        internal void CharacterStopMove(BattleCharacter character)
        {
            character.View.StopMove();
            var notify = new Proto.Notify_CharacterStopMove 
            {
                Index = character.Index,
                TargetForward = character.View.Transform.Forward.ToV3(),
                TargetPosition = character.View.Transform.Position.ToV3()
            };
            AddNotify(notify);
        }

        internal void PlayMotion(BattleCharacter releaser, string motionName)
        {
            releaser.View.PlayMotion(motionName);
            var notify = new Proto.Notify_LayoutPlayMotion
            { 
                Index = releaser.Index,
                Motion = motionName
            };
            AddNotify(notify);
        }

        internal void LookAtCharacter(BattleCharacter own, BattleCharacter target)
        {
            own.View.LookAt(
                target.View.Transform);
            var notify = new Proto.Notify_LookAtCharacter { 
                Own = own.Index,
                Target = target.Index
            };
            AddNotify(notify);
        }

        internal void ProcessDamage(BattleCharacter sources,BattleCharacter effectTarget,DamageResult result)
        {
            var notify = new Notify_DamageResult {
                Damage = result.Damage,
                IsMissed =result.IsMissed,
                Index = sources.Index,
                TargetIndex = effectTarget.Index
            };

            AddNotify(notify);

            if (result.IsMissed) return;
            CharacterSubHP(effectTarget, result.Damage);
        }

        internal void CharacterSubHP(BattleCharacter effectTarget, int lostHP)
        {
            
            effectTarget.SubHP(lostHP);
            var notify = new Proto.Notity_EffectSubHP
            {
                Index = effectTarget.Index,
                LostHP = lostHP,
                TargetHP = effectTarget.HP,
                Max =effectTarget.MaxHP
            };
            AddNotify(notify);
        }

        internal void CharacterAddHP(BattleCharacter effectTarget, int addHp)
        {
            effectTarget.AddHP(addHp);
            var notify = new Proto.Notity_EffectAddHP
            {
                Index = effectTarget.Index,
                CureHP = addHp,
                TargetHP = effectTarget.HP,
                Max = effectTarget.MaxHP
            };
            AddNotify(notify);
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

						forward = View.RotateWithY(forward, offsetAngle);

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
							if (View.Distance(orgin, t.View.Transform.Position) > radius) return false;
                            if (angle < 360)
                            {
                                if (View.Angle(forward, t.View.Transform.Forward) > (angle / 2)) return false;
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

        internal void ModifyProperty(
            BattleCharacter character, 
            HeroPropertyType property, 
            AddType addType, 
            float addValue)
        {
            var value = character[property];
            switch (addType)
            {
                case AddType.Append:
                    {
                        value.SetAppendValue(value.AppendValue + (int)addValue);
                    }
                    break;
                case AddType.Base:
                    {
                        value.SetBaseValue(value.BaseValue + (int)addValue);
                    }
                    break;
                case AddType.Rate:
                    {
                        value.SetRate(value.Rate + (int)addValue);
                    }
                    break;
            }

            var notify = new Notify_PropertyValue { Index = character.Index, FinallyValue = value.FinalValue };
            AddNotify(notify);
        }

		public float Distance(BattleCharacter c1, BattleCharacter c2)
		{
			return Math.Max(0, View.Distance(c1.View.Transform.Position, c2.View.Transform.Position) -1);
		}
	}
}

