using System;
using System.Collections.Generic;
using Astar;
using EngineCore;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;
using GameLogic.Game.Perceptions;
using Layout;
using Layout.AITree;
using Layout.LayoutElements;
using Proto;
using ServerUtility;
using UMath;

namespace MapServer.GameViews
{
    /// <summary>
    /// Battle perception view.
    /// </summary>
    public class BattlePerceptionView : IBattlePerception
    {
        
        public BattlePerceptionView(ITimeSimulater timeSimulater,Pathfinder finder)
        {
            Simulater = timeSimulater;
            Finder = finder;
        }

        /// <summary>
        /// 寻路
        /// </summary>
        /// <value>The finder.</value>
        public Pathfinder Finder { private set; get; }

        /// <summary>
        /// 地图配置
        /// </summary>
        /// <value>The map config.</value>
        public MapData MapConfig {set; get; }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <value>The simulater.</value>
        public ITimeSimulater Simulater { private set; get; }

        public void Update(GTime now)
        {
            foreach (var i in _AttachElements)
            {
                i.Value.Update(now);
            }
        }

        #region views
        private Dictionary<long, BattleElement> _AttachElements = new Dictionary<long, BattleElement>();

        internal void DeAttachView(BattleElement battleElement)
        {
            AddNotify(new Notify_ElementExitState { Index = battleElement.Index });
            _AttachElements.Remove(battleElement.Index);
        }

        internal void AttachView(BattleElement battleElement)
        {
            AddNotify(battleElement.GetInitNotify());
            AddNotify(new Notify_ElementJoinState { Index = battleElement.Index });
            _AttachElements.Add(battleElement.Index, battleElement);
        }
        #endregion

        #region notify
        private Queue<ISerializerable> _notify = new Queue<ISerializerable>();

        public void AddNotify(ISerializerable notify)
        {
            _notify.Enqueue(notify);
        }

        public ISerializerable[] GetAndClearNotify()
        {
            if (_notify.Count > 0)
            {
                var list = _notify.ToArray();
                _notify.Clear();
                return list;
            }
            else
                return new ISerializerable[0];
        }

        public ISerializerable[] GetInitNotify()
        {
            var list = new List<ISerializerable>();
            foreach (var i in _AttachElements)
            {
                var sElement = i.Value as ISerializerableElement;
                if (sElement != null)
                {
                    list.Add(sElement.GetInitNotify());
                    list.Add(new Notify_ElementJoinState { Index = i.Key });
                }
            }
            return list.ToArray();
        }
        #endregion

        #region IBattlePerception

        IBattleCharacter IBattlePerception.CreateBattleCharacterView(string res, UVector3 pos, UVector3 forword)
        {
            return new BattleCharactorView(pos, forword,this,Finder);
        }

        IBattleMissile IBattlePerception.CreateMissile(IMagicReleaser releaser, MissileLayout layout)
        {
            var missile = new BattleMissileView(this);
            var releaserCharacter = releaser as BattleMagicReleaserView;
            var pos = releaserCharacter.CharacterReleaser.Transform.position;
            missile.SetPosition( pos);
            return missile;
        }

        IParticlePlayer IBattlePerception.CreateParticlePlayer(IMagicReleaser releaser, ParticleLayout layout)
        {
            var notify = new Proto.Notify_LayoutPlayParticle
            {
                ReleaseIndex = releaser.Index,
                FromTarget = (int)layout.fromTarget,
                ToTarget = (int)layout.toTarget,
                Path = layout.path,
                ToBoneName = layout.toBoneName,
                FromBoneName = layout.fromBoneName,
                DestoryTime = layout.destoryTime,
                DestoryType = (int)layout.destoryType
            };
            AddNotify(notify);
            return new ParticlePlayerView();
        }

        IMagicReleaser IBattlePerception.CreateReleaserView(IBattleCharacter releaser, IBattleCharacter targt, UVector3? targetPos)
        {
            return new BattleMagicReleaserView(releaser, targt,this);
        }


        bool IBattlePerception.ExistMagicKey(string key)
        {
            return ResourcesLoader.Singleton.HaveMagicKey(key);
        }

        TreeNode IBattlePerception.GetAITree(string pathTree)
        {
            return ResourcesLoader.Singleton.GetAITree(pathTree);
        }

        MagicData IBattlePerception.GetMagicByKey(string key)
        {
            return ResourcesLoader.Singleton.GetMagicByKey(key);
        }

        TimeLine IBattlePerception.GetTimeLineByPath(string path)
        {
            return ResourcesLoader.Singleton.GetTimeLineByPath(path);
        }

        ITimeSimulater IBattlePerception.GetTimeSimulater()
        {
            return Simulater;
        }


        void IBattlePerception.ProcessDamage(IBattleCharacter sources, IBattleCharacter target, DamageResult result)
        {
            var notify = new Notify_DamageResult
            {
                Damage = result.Damage,
                IsMissed = result.IsMissed,
                Index = sources.Index,
                TargetIndex = target.Index
            };
            AddNotify(notify);
        }
        #endregion
    }
}

