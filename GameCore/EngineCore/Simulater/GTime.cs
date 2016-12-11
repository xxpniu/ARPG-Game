using System;

namespace EngineCore.Simulater
{
	public interface ITimeSimulater
	{
		GTime Now { get; }
	}

	public struct GTime
	{
		public GTime(float time, float delta)
		{
			Time = time;
            DeltaTime = delta;
		}
		public float Time;
		public float DeltaTime;

        public override string ToString()
        {
            return string.Format("({0:0.0},{1:0.00})",Time,DeltaTime);
        }
	}
}

