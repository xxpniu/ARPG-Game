using System;

namespace EngineCore.Simulater
{
	public  abstract class GAction
	{
		private class EmptyAction:GAction
		{
			public EmptyAction():base(null)
			{
				
			}
			public override void Execute (GTime time, GObject current)
			{
				return;
			}
		}

		static GAction()
		{
			Empty = new EmptyAction ();
		}

		public static GAction Empty { private set; get; }

		public GAction (GPerception perception)
		{
			Perceptipn = perception;
		}

		public GPerception Perceptipn{ private set; get; }

		public abstract void Execute (GTime time, GObject current);
	}
}

