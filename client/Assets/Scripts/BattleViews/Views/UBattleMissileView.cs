using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using UMath;
using UGameTools;
using EngineCore.Simulater;
using Google.Protobuf;
using GameLogic;
using Proto;

public class UBattleMissileView : UElementView ,IBattleMissile
{
	// Use this for initialization
	void Start () 
    {
	
	}
	
    void Awake()
    {
        trans = new UTransform();
    }

	// Update is called once per frame
	void Update () 
    {
        trans.localPosition = this.transform.localPosition.ToGVer3();
        trans.localRotation = this.transform.localRotation.ToGQu();
        trans.localScale = this.transform.localScale.ToGVer3();
	}

    private UTransform trans;

    UTransform IBattleMissile.Transform 
	{
		get
		{
            return trans;
		}
	}

    public override void OnAttachElement (GObject el)
	{
        base.OnAttachElement (el);
		gameObject.name = string.Format ("Missile_{0}", el.Index);
		//missile = el as BattleMissile;
	}

    public override IMessage ToInitNotify()
    {
        var missile = this.Element as BattleMissile;
        var createNotify = new Notify_CreateMissile
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
        return createNotify;
    }

}
