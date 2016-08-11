using System;
using GameLogic.Game.Elements;

namespace MapServer.GameViews
{
    public class BattleMagicReleaserView : BattleElement, GameLogic.Game.Elements.IMagicReleaser
    {
        public BattleMagicReleaserView(IBattleCharacter releaser, IBattleCharacter target,BattlePerceptionView view) : base(view)
        {
            CharacterTarget = target;
            CharacterReleaser = releaser;
        }

        public IBattleCharacter CharacterTarget { private set; get; }
        public IBattleCharacter CharacterReleaser { private set; get; }
    }
}

