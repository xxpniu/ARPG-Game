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

	public EngineCore.GVector3 GetPosition ()
	{
		return new EngineCore.GVector3 (this.transform.position.x,
			this.transform.position.y,
			this.transform.position.z);
	}

	public EngineCore.GVector3 GetForward ()
	{
		return new EngineCore.GVector3 (this.transform.localRotation.eulerAngles.x,
			this.transform.localRotation.eulerAngles.y,
			this.transform.localRotation.eulerAngles.z);
	}

	public override void ExitState (EngineCore.Simulater.GObject el)
	{
		base.ExitState (el);
		GameObject.Destroy (gameObject);
	}

	public override void JoinState (EngineCore.Simulater.GObject el)
	{
		base.JoinState (el);
		gameObject.name = string.Format ("Missile_{0}", el.Index);
	}
}
