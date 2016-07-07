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
	public override void ExitState (EngineCore.Simulater.GObject el)
	{
		base.ExitState (el);
		GameObject.Destroy (this.gameObject);
	}

	public override void JoinState (EngineCore.Simulater.GObject el)
	{
		base.JoinState (el);
		this.gameObject.name = string.Format ("Releaser_{0}", el.Index);
	}
}
