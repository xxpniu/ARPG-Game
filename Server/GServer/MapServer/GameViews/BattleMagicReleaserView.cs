using System;
using GameLogic.Game.Elements;
using Google.Protobuf;
using Proto;

namespace MapServer.GameViews
{
    public class BattleMagicReleaserView : BattleElement, IMagicReleaser
    {
        public BattleMagicReleaserView(IBattleCharacter releaser, IBattleCharacter target,BattlePerceptionView view) : base(view)
        {
            CharacterTarget = target;
            CharacterReleaser = releaser;
        }

        public IBattleCharacter CharacterTarget { private set; get; }
        public IBattleCharacter CharacterReleaser { private set; get; }

        public override  IMessage GetInitNotify()
        {
            var mReleaser = this.Element as MagicReleaser;
            var createNotify = new Notify_CreateReleaser
            {
                Index = mReleaser.Index,
                ReleaserIndex = mReleaser.ReleaserTarget.Releaser.Index,
                TargetIndex = mReleaser.ReleaserTarget.ReleaserTarget.Index,
                MagicKey = mReleaser.Magic.key
            };
            return (createNotify);
        }
    }
}

