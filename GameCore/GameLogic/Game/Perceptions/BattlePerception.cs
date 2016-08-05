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

		public MagicReleaser CreateReleaser(string key, IReleaserTarget target)
		{
			var magic = View.GetMagicByKey(key);
			var releaser= CreateReleaser(magic, target);
            return releaser;
		}

        public MagicReleaser CreateReleaser(MagicData magic, IReleaserTarget target)
		{
			var view = View.CreateReleaserView(target.Releaser.View, target.ReleaserTarget.View, target.TargetPosition);
			var mReleaser = new MagicReleaser(magic, target, this.ReleaserControllor, view);
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
            var createNotify = new Proto.Notity_CreateMissile
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
        public BattleCharacter CreateCharacter(ExcelConfig.CharacterData data, int teamIndex, GVector3 position, GVector3 forward)
		{
			var res = data.ResourcesPath;
			var view = View.CreateBattleCharacterView(res, position, forward);
			var battleCharacter = new BattleCharacter(data.ID, this.BattleCharacterControllor, view);
			battleCharacter.HPMax.SetBaseValue(data.HPMax);
			battleCharacter.Defence.SetBaseValue(data.Defance);
			battleCharacter.DamageMin.SetBaseValue(data.DamageMin);
			battleCharacter.DamageMax.SetBaseValue(data.DamageMax);
			battleCharacter.Attack.SetBaseValue(data.Attack);
			battleCharacter.Level = data.Level;
			battleCharacter.TDamage = (Proto.DamageType)data.DamageType;
			battleCharacter.TDefance = (DefanceType)data.DefanceType;
			battleCharacter.TBody = (BodyType)data.BodyType;
			battleCharacter.TAttack = (AttackType)data.AttackType;
			battleCharacter.Name = data.Name;
			battleCharacter.TeamIndex = teamIndex;
			battleCharacter.Speed = data.MoveSpeed;
			view.SetPriorityMove(data.PriorityMove);
			battleCharacter.Init();

            var createNotity = new Proto.Notify_CreateBattleCharacter
            {
                Index = battleCharacter.Index,
                ConfigID = battleCharacter.ConfigID,
                Position = view.Transform.Position.ToV3(),
                Forward = view.Transform.Forward.ToV3(),
                MaxHP = (int)battleCharacter.HPMax,
                HP = battleCharacter.HP,
                Defence = (int)battleCharacter.Defence,
                Attack = (int)battleCharacter.Attack,
                DamageMax = (int)battleCharacter.DamageMax,
                DamageMin = (int)battleCharacter.DamageMin,
                Level = battleCharacter.Level,
                TDamage = battleCharacter.TDamage,
                TDefance = battleCharacter.TDefance,
                TBody = battleCharacter.TBody,
                TAttack = battleCharacter.TAttack,
                Name = battleCharacter.Name,
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
            var notify = new Proto.Notity_LayoutPlayParticle 
            {
                ReleaseIndex = relaser.Index
            };
            AddNotify(notify);
            return p;
        }

        internal void CharacterMoveTo(BattleCharacter character, GVector3 pos)
        {
            character.View.MoveTo(pos);
            var notify = new Proto.Notity_CharacterBeginMove 
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
            var notify = new Proto.Notity_CharacterStopMove 
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
            var notify = new Proto.Notity_LayoutPlayMotion
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
            var notify = new Proto.Notity_LookAtCharacter { 
                Own = own.Index,
                Target = target.Index
            };
            AddNotify(notify);
        }

        internal void CharacterSubHP(BattleCharacter effectTarget, int lostHP)
        {
            effectTarget.SubHP(lostHP);
            var notify = new Proto.Notity_EffectSubHP
            {
                Index = effectTarget.Index,
                LostHP = lostHP,
                TargetHP = effectTarget.HP
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
                TargetHP = effectTarget.HP
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
                    GObject.Destory(t);
                }
                return false;
            });
        }

		public float Distance(BattleCharacter c1, BattleCharacter c2)
		{
			return Math.Max(0, View.Distance(c1.View.Transform.Position, c2.View.Transform.Position) -1);
		}
	}
}

