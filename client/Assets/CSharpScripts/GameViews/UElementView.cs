using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;

public class UElementView : MonoBehaviour, IBattleElement {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IBattleElement implementation

	public virtual void JoinState (EngineCore.Simulater.GObject el)
	{
		
	}

	public virtual void ExitState (EngineCore.Simulater.GObject el)
	{
		
	}

	#endregion
}
