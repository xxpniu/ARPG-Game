using System;
using EngineCore.Simulater;
using UnityEngine;

public class UGameGate:UGate
{
	public UGameGate ()
	{
	}

	#region implemented abstract members of UGate

	public override void JoinGate ()
	{

	}

	public override void ExitGate ()
	{
		 
	}

	public override void Tick ()
	{
		 
	}

	public override  GTime GetTime ()
	{
		return new GTime (Time.time, Time.deltaTime);
	}

	#endregion
}