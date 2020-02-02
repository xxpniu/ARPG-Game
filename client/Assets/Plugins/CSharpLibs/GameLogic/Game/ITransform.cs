using System;
using EngineCore;
using UMath;

namespace GameLogic
{
	public interface ITransform
	{
		UVector3 Position { get; }
		UVector3 ForwardEulerAngles { get;}
		UVector3 Forward { get; }
		void LookAt(ITransform trans);
	}
}

