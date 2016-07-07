using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;

public class UCharacterView : UElementView,IBattleCharacter {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public EngineCore.GVector3 GetPosition ()
	{
		return new EngineCore.GVector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	public EngineCore.GVector3 GetForward ()
	{
		var qu = this.transform.localRotation;
		return new EngineCore.GVector3(qu.eulerAngles.x,qu.eulerAngles.y,qu.eulerAngles.z);
	}

	public void SetPosition (EngineCore.GVector3 pos)
	{
		this.transform.position = new Vector3 (pos.x, pos.y, pos.z);
	}

	public void SetForward (EngineCore.GVector3 forward)
	{
		this.transform.localRotation = Quaternion.Euler (forward.x, forward.y, forward.z);
	}

	public void PlayMotion (string motion)
	{
		var an =character. GetComponent<Animator> ();
		an.SetTrigger (motion);
	}

	public GameObject character;
}
