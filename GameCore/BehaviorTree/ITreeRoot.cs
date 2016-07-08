using System;
namespace BehaviorTree
{
	public interface ITreeRoot
	{
		float Time { get; }
		Object UserState { get; }

		void Chanage(Composite cur);
	}
}

