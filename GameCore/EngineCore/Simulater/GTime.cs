using System;

namespace EngineCore.Simulater
{
	public class GTimeSimulater
	{
		public GTimeSimulater ()
		{
			
		}

		public static void Reset()
		{
			time = 0;
			detalTime = 0;
		}

		private static float time =0;
		private static float detalTime = 0;

		public static GTime GetTime()
		{
			if (Math.Abs(detalTime) < 0.001f) {
				throw new Exception ("You must tick Simulater before you get time!");
			}
			return  new GTime{ DetalTime = detalTime, Time =time };
		}

		public static void Tick(float timeDetal)
		{
			time += timeDetal;
			detalTime = timeDetal;
		}
			
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

