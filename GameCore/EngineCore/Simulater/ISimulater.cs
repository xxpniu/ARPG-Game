using System;

namespace EngineCore.Simulater
{
	public interface ISimulater
	{
		void Tick();
		GState GetCurrent();
		GTime GetNow();
	}
}

