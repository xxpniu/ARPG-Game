using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.AIBehaviorTree
{
    public class DecoratorTickUntilSuccess : BehaviorTree.Decorator
    {
        public DecoratorTickUntilSuccess(BehaviorTree.Composite child) : base(child) { }

        public override IEnumerable<BehaviorTree.RunStatus> Execute(ITreeRoot context)
        {
			float lastTime = context.Time;

            while (true)
            {
				if (lastTime + (TickTime) >= context.Time)
                {
                    yield return BehaviorTree.RunStatus.Running;
                }

				lastTime = context.Time;
                DecoratedChild.Start(context);

                while (DecoratedChild.Tick(context) == RunStatus.Running)
                {
                    yield return RunStatus.Running;
                }
                DecoratedChild.Stop(context);

                if (DecoratedChild.LastStatus.HasValue)
                {
                    if (DecoratedChild.LastStatus.Value == RunStatus.Success)
                    {
                        yield return BehaviorTree.RunStatus.Success;
                        yield break;
                    }
                }

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

