using UnityEngine;
using System.Collections;
using GameLogic.Game.Perceptions;
using EngineCore.Simulater;

public class UView :XSingleton<UView>,GameLogic.IViewBase {

	// Use this for initialization
	void Start () 
	{
	  
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void Awake()
	{
		
	}

	#region IViewBase implementation

	public IBattlePerception Create ()
	{
		return UPerceptionView.Singleton;
	}

	#endregion
}
