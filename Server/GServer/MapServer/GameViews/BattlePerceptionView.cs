using System;
using System.Collections.Generic;
using Astar;
using EngineCore;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;
using GameLogic.Game.Perceptions;
using Layout;
using Layout.AITree;
using Layout.LayoutElements;
using OpenTK;
using Proto;
using Vector3 = OpenTK.Vector3;

namespace MapServer.GameViews
{
    public class BattlePerceptionView : IBattlePerception
    {
        public BattlePerceptionView(ITimeSimulater timeSimulater,Pathfinder finder)
        {
            Simulater = timeSimulater;
            Finder = finder;
        }

        public Pathfinder Finder { private set; get; }

        public MapData MapConfig {set; get; }

        public ITimeSimulater Simulater { private set; get; }

        public float Angle(GVector3 v, GVector3 v2)
        {
            return Vector3.CalculateAngle(v.ToVector3(), v2.ToVector3());
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
            return new ParticlePlayerView();
        }

        public IMagicReleaser CreateReleaserView(IBattleCharacter releaser, IBattleCharacter targt, GVector3? targetPos)
        {
            return new BattleMagicReleaserView(releaser, targt,this);
        }

        public float Distance(GVector3 v, GVector3 v2)
        {
            var r = v.ToVector3() - v2.ToVector3();
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

        public GVector3 NormalVerctor(GVector3 gVector3)
        {
            var vn = gVector3.ToVector3().Normalized();
            return new GVector3(vn.X, vn.Y, vn.Z);
        }

        public GVector3 RotateWithY(GVector3 v, float angle)
        {
            var vn = v.ToVector3();
            var q = Quaternion.Identity;
            q.Y = angle;

            var result = Vector3.Transform(vn, q);
            return new GVector3(result.X, result.Y, result.Z);
        }

        private Dictionary<long, BattleElement> _AttachElements = new Dictionary<long, BattleElement>();

        internal void DeAttachView(BattleElement battleElement)
        {
            _AttachElements.Remove(battleElement.Index);
        }

        internal void AttachView(BattleElement battleElement)
        {
            _AttachElements.Add(battleElement.Index, battleElement);
        }

        public void Update(GTime now)
        {
            //var now = this.Simulater.Now;
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
            var list = _notify.ToArray();
            _notify.Clear();
            return list;
        }
    }
}

