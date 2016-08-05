using System;

namespace EngineCore.Simulater
{
	public class GPerception
	{
		public GPerception (GState state)
		{
			this.State = state;
		}

		public GState State{set;get;}

        public void JoinElement(GObject el)
        {
            State.AddElement(el);
        }
	}
}

