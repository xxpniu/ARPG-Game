using System;
using EngineCore.Simulater;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class UGate : MonoBehaviour, ITimeSimulater
{
	public abstract void JoinGate();
	public abstract void ExitGate();
	public abstract void Tick();

	public GTime GetTime()
	{
		return new GTime(Time.time, Time.deltaTime);
	}

	#region ITimeSimulater implementation

	public GTime Now { get { return GetTime(); } }

	#endregion

}

