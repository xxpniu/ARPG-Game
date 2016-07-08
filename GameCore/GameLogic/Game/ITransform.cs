using System;
using EngineCore;

namespace GameLogic
{
	public interface ITransform
	{
		GVector3 Position { get; }
		GVector3 ForwardEulerAngles { get;}
		GVector3 Forward { get; }
		void LookAt(ITransform trans);
	}
}

