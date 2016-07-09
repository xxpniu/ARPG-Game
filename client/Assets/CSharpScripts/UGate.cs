using System;
using EngineCore.Simulater;

public abstract class UGate :ITimeSimulater
{
	public abstract void JoinGate();
	public abstract void ExitGate();
	public abstract void Tick();
	public abstract GTime GetTime();


	#region ITimeSimulater implementation

	public GTime Now {
		get {
			return GetTime ();
		}
	}

	#endregion
}

