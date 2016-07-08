using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;

public class UBattleMissileView : UElementView ,IBattleMissile
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public GameLogic.ITransform Transform 
	{
		get
		{
			return new GTransform (this.transform);
		}
	}
		


	public override void JoinState (EngineCore.Simulater.GObject el)
	{
		base.JoinState (el);
		gameObject.name = string.Format ("Missile_{0}", el.Index);
		//missile = el as BattleMissile;
	}

}
