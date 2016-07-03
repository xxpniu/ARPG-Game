using System;

namespace EngineCore.Simulater
{
	public abstract class GControllor
	{
		private class EmptyControllor:GControllor
		{
			public EmptyControllor():base(null){}
			public override GAction GetAction (GTime time, GObject current)
			{
				return GAction.Empty;
			}
		}
		static GControllor()
		{
			Empty = new EmptyControllor ();
		}

		public static GControllor Empty{ private set; get; }

		public GControllor (GPerception per)
		{
			Perception = per;
		}

		public GPerception Perception{ private set; get; }

		public  abstract  GAction GetAction (GTime time, GObject current);


	}
}

