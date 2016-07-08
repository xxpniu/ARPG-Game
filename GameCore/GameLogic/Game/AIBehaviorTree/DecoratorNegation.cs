using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.AIBehaviorTree
{
    public class DecoratorNegation : BehaviorTree.Decorator
    {
        public DecoratorNegation(BehaviorTree.Composite child) : base(child) { }

        public override IEnumerable<BehaviorTree.RunStatus> Execute(ITreeRoot context)
        {
            if (DecoratedChild == null)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            DecoratedChild.Start(context);
            while (DecoratedChild.Tick(context) == RunStatus.Running)
            {
                yield return RunStatus.Running;
            }
            DecoratedChild.Stop(context);
            var last = DecoratedChild.LastStatus.Value;
            if (last == RunStatus.Success)
            {
                yield return RunStatus.Failure;
            }
            else
            {
                yield return RunStatus.Success;
            }
        }
    }
}
