using System;

namespace EngineCore.Simulater
{
	public abstract class GControllor
	{
		
		public GControllor (GPerception per)
		{
			Perception = per;
		}

		public GPerception Perception{ private set; get; }

		public  abstract  GAction GetAction (GTime time, GObject current);


	}
}

