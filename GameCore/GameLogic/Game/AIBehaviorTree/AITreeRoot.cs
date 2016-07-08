using System;
using BehaviorTree;
using EngineCore.Simulater;

namespace GameLogic
{
	public class AITreeRoot:ITreeRoot
	{
		public AITreeRoot(ITimeSimulater timeSimulater, object userstate, Composite root)
		{
			TimeSimulater = timeSimulater;
			UserState = userstate;
			Root = root;
		}

		public ITimeSimulater TimeSimulater { private set; get; }

		public float Time
		{
			get
			{
				return TimeSimulater.Now.Time;
			}
		}

		public object UserState
		{
			get;
			private set;
		}

		public Composite Root { private set; get; }

		public void Tick()
		{
			if (Current == null)
			{
				Current = Root;
			}

			if (next != null)
			{
				Current.Stop(this);
				Current = next;
				next = null;
				Current.Start(this);
			}

			Current.Tick(this);
			if (Current.LastStatus.HasValue
				&& Current.LastStatus.Value != BehaviorTree.RunStatus.Running)
			{
				Current.Stop(this);
				//重新从根执行
				Current = Root;
				Current.Start(this);
			}
		}

		private Composite next;

		public void Chanage(Composite cur)
		{
			next = cur;
		}

		private Composite Current;


	}
}

