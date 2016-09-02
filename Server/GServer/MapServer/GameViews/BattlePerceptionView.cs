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

namespace MapServer.GameViews
{
    public class BattlePerceptionView : IBattlePerception
    {
        
        public BattlePerceptionView(ITimeSimulater timeSimulater,Pathfinder finder)
        {
            Simulater = timeSimulater;
            Finder = finder;
            processer = new NotifyProcessor();
        }


        public BattlePerception Per { set; get; }

        private NotifyProcessor processer;

        public Pathfinder Finder { private set; get; }

        public MapData MapConfig {set; get; }

        public ITimeSimulater Simulater { private set; get; }

        public float Angle(GVector3 v, GVector3 v2)
        {
            return GVector3.CalculateAngle(v, v2);
        }

        public IBattleCharacter CreateBattleCharacterView(string res, GVector3 pos, GVector3 forword)
        {
            return new BattleCharactorView(pos, forword,this,Finder);
        }

        public IBattleMissile CreateMissile(IMagicReleaser releaser, MissileLayout layout)
        {
            var missile = new BattleMissileView(this);
            var releaserCharacter = releaser as BattleMagicReleaserView;
            var pos = releaserCharacter.CharacterReleaser.Transform.Position;
            missile.SetPosition( pos);
            return missile;
        }

        public IParticlePlayer CreateParticlePlayer(IMagicReleaser releaser, ParticleLayout layout)
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

        public IMagicReleaser CreateReleaserView(IBattleCharacter releaser, IBattleCharacter targt, GVector3? targetPos)
        {
            return new BattleMagicReleaserView(releaser, targt,this);
        }

        public float Distance(GVector3 v, GVector3 v2)
        {
            var r = v - v2;
            return r.Length;
        }

        public bool ExistMagicKey(string key)
        {
            return ResourcesLoader.Singleton.HaveMagicKey(key);
        }

        public TreeNode GetAITree(string pathTree)
        {
            return ResourcesLoader.Singleton.GetAITree(pathTree);
        }

        public MagicData GetMagicByKey(string key)
        {
            return ResourcesLoader.Singleton.GetMagicByKey(key);
        }

        public TimeLine GetTimeLineByPath(string path)
        {
            return ResourcesLoader.Singleton.GetTimeLineByPath(path);
        }

        public ITimeSimulater GetTimeSimulater()
        {
            return Simulater;
        }

        public GVector3 RotateWithY(GVector3 v, float angle)
        {
            var vn = v;
            var q = GQuaternion.Identity;
            q.Y = angle;

            var result = GVector3.Transform(vn, q);
            return new GVector3(result.X, result.Y, result.Z);
        }

        private Dictionary<long, BattleElement> _AttachElements = new Dictionary<long, BattleElement>();

        internal void DeAttachView(BattleElement battleElement)
        {
            AddNotify(new Proto.Notify_ElementExitState { Index = battleElement.Index });
            _AttachElements.Remove(battleElement.Index);
        }

        internal void AttachView(BattleElement battleElement)
        {
            AddNotify(this.processer.NotityElementCreate(battleElement.Element));
            AddNotify(new Proto.Notify_ElementJoinState { Index = battleElement.Index });
            _AttachElements.Add(battleElement.Index, battleElement);
        }

        public void Update(GTime now)
        {
            foreach (var i in _AttachElements)
            {
                i.Value.Update(now);
            }
        }

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

        public void ProcessDamage(IBattleCharacter sources, IBattleCharacter target, DamageResult result)
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

        public ISerializerable[] GetInitNotify()
        {
            var els = Per.GetEnableElements();
            var list = new List<ISerializerable>();
            foreach (var i in els)
            {
                list.Add(this.processer.NotityElementCreate(i));
                list.Add(new Proto.Notify_ElementJoinState { Index = i.Index });
            }
            return list.ToArray();
        }

        public void SetPercetion(BattlePerception per)
        {
            this.Per = per;
        }
    }
}

