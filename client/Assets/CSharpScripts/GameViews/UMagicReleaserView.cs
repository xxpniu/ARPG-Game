using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;

public class UMagicReleaserView :UElementView,IMagicReleaser {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnAttachElement(EngineCore.Simulater.GObject el)
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
}
