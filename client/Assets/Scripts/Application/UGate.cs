using System;
using EngineCore.Simulater;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class UGate : MonoBehaviour
{
	protected virtual void JoinGate() { }
	protected virtual void ExitGate() { }
	protected virtual void Tick() { }

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		JoinGate();
	}

	private void OnDisable() => ExitGate();

	private void Update() => Tick();

	

}

