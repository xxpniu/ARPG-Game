using System;

namespace EngineCore.Simulater
{
	public interface ITimeSimulater
	{
		GTime Now { get; }
	}

	public struct GTime
	{
		public GTime(float time, float detal)
		{
			Time = time;
			DetalTime = detal;
		}
		public float Time;
		public float DetalTime;
	}
}

