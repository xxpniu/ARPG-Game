using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using UMath;
using UGameTools;
using EngineCore.Simulater;

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

}
