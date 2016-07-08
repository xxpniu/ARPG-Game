using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    /// <summary>
    ///   Will execute each branch of logic in order, until one succeeds. This composite
    ///   will fail only if all branches fail as well.
    /// </summary>
    public class PrioritySelector : Selector
    {
        public PrioritySelector(params Composite[] children)
            : base(children)
        {
        }

        public override IEnumerable<RunStatus> Execute(ITreeRoot context)
        {
            // Keep in mind; we ARE an enumerator here. So we do execute each child in tandem.
            foreach (Composite node in Children)
            {
                // All behaviors are 'Decorators' by default. This just makes it simple.
                // and allows us to not have another class that is nothing but a Decorator : Behavior
                // Though; it may be a good idea in the future to add.
                // Keep in mind!!!
                // Start is called EVERY time we check a behavior! REGARDLESS OF IT'S RETURN VALUE!
                // This makes sure we don't end up with a corrupted state that always returns 'Running' 
                // when it's actualled 'Success' or 'Failed'
                node.Start(context);
                // If the current node is still running; return so. Don't 'break' the enumerator
                while (node.Tick(context) == RunStatus.Running)
                {
                    Selection = node;
                    yield return RunStatus.Running;
                }

                // Clear the selection... since we don't have one! Duh.
                Selection = null;
                // Call Stop to allow the node to cleanup anything. Since we don't need it anymore.
                node.Stop(context);
                // If it succeeded (since we're a selector) we return that this GroupNode
                // succeeded in executing.
                if (node.LastStatus == RunStatus.Success)
                {
                    yield return RunStatus.Success;
                    yield break;
                }

                // XXX - Removed. This would make us use an extra 'tick' just to get to the next child composite.
                // Still running, so continue on!
                //yield return RunStatus.Running;
            }
            // We ran out of children, and none succeeded. Return failed.
            yield return RunStatus.Failure;
            // Make sure we tell our parent composite, that we're finished.
            yield break;
        }
    }
}