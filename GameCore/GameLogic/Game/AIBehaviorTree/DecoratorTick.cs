using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic.Game.AIBehaviorTree
{
    public class DecoratorTick : BehaviorTree.Decorator
    {
        public DecoratorTick(BehaviorTree.Composite child) : base(child) { }

        public override IEnumerable<BehaviorTree.RunStatus> Execute(ITreeRoot context)
        {
			float lastTime = context.Time;

            while (true)
            {
				if (lastTime + (TickTime / 1000f) <= context.Time)
                {
					lastTime = context.Time;
                    DecoratedChild.Start(context);
                    while (DecoratedChild.Tick(context) == RunStatus.Running)
                    {
                        yield return RunStatus.Running;
                    }
                    DecoratedChild.Stop(context);
                }
                yield return BehaviorTree.RunStatus.Running;
            }
        }

        public float TickTime { set; get; }

		public override void Stop(ITreeRoot context)
		{
			base.Stop(context);
			DecoratedChild.Stop(context);
		}
    }
}
