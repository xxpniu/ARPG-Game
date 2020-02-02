using System;
namespace BehaviorTree
{
	public interface ITreeRoot
	{
		float Time { get; }
		Object UserState { get; }

		void Chanage(Composite cur);

        void SetInt(string key, int value);
        int GetInt(string key);
	}
}

