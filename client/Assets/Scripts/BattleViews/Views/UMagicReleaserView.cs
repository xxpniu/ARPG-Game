using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using Google.Protobuf;
using Proto;
using EngineCore.Simulater;

public class UMagicReleaserView :UElementView,IMagicReleaser
{

    public override void OnAttachElement(GObject el)
    {
        base.OnAttachElement(el);
        this.gameObject.name = string.Format ("Releaser_{0}", el.Index);
        var releaser = el as MagicReleaser;
        SetCharacter(releaser.ReleaserTarget.Releaser.View,
            releaser.ReleaserTarget.ReleaserTarget.View);
    }

    public void SetCharacter(IBattleCharacter releaser, IBattleCharacter target)
    {
        CharacterTarget = target;
        CharacterReleaser = releaser;
    }

	public IBattleCharacter CharacterTarget{private set; get;}
	public IBattleCharacter CharacterReleaser{private set; get; }

    public override IMessage ToInitNotify()
    {
        var mReleaser = this.Element as MagicReleaser;
        var createNotify = new Notify_CreateReleaser
        {
            Index = mReleaser.Index,
            ReleaserIndex = mReleaser.ReleaserTarget.Releaser.Index,
            TargetIndex = mReleaser.ReleaserTarget.ReleaserTarget.Index,
            MagicKey = mReleaser.Magic.key
        };
        return createNotify;
    }
}
