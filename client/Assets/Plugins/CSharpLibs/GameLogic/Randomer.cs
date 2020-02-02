using System;

namespace GameLogic
{
	public sealed class Randomer
	{
		private static Random _random = new Random();
		public Randomer ()
		{
		}

		public static int RandomMinAndMax(int min,int max)
		{
			return _random.Next (min, max);
		}

		public static bool RandomTorF()
		{
			return _random.Next (0, 2) == 1;
		}
	}
}

