using System;
using EngineCore;
using EngineCore.Simulater;
using GameLogic;
using GameLogic.Game.Elements;
using Google.Protobuf;
using Proto;
using UMath;

namespace MapServer.GameViews
{
    public class BattleMissileView : BattleElement, IBattleMissile
    {
        public BattleMissileView(BattlePerceptionView view) : base(view)
        {
            transform = new UTransform();

        }

        private UTransform transform;

        public void SetPosition(UVector3 pos)
        {
            transform.position = pos;
        }

        public override IMessage GetInitNotify()
        {
            var missile = Element as BattleMissile;
            var createNotify = new Proto.Notify_CreateMissile
            {
                Index = missile.Index,
                Position = missile.View.Transform.position.ToV3(),
                ResourcesPath = missile.Layout.resourcesPath,
                Speed = missile.Layout.speed,
                ReleaserIndex = missile.Releaser.Index,
                FormBone = missile.Layout.fromBone,
                ToBone = missile.Layout.toBone,
                Offset = missile.Layout.offset.ToV3()
            };
            return (createNotify);
        }

        #region impl

        UTransform IBattleMissile.Transform
        {
            get
            {
                return transform;
            }
        }

        #endregion
    }
}

