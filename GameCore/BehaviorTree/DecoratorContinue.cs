using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    /// <summary>
    ///   A decorator that allows you to execute code only if some condition is met. It does not 'break' the current
    ///   tree if the condition fails, or children fail.
    /// 		 
    ///   This is useful for "if I need to, go ahead, otherwise, ignore" in sequences.
    /// 		 
    ///   It can be thought of as an optional execution.
    /// </summary>
    /// <remarks>
    ///   Created 1/13/2011.
    /// </remarks>
    public class DecoratorContinue : Decorator
    {
        public DecoratorContinue(CanRunDecoratorDelegate func, Composite decorated)
            : base(func, decorated)
        {
        }

        public DecoratorContinue(Composite child)
            : base(child)
        {
        }

        private RunStatus GetContinuationStatus()
        {
            // Selectors run until we fail.
            if (Parent is Selector)
            {
                return RunStatus.Failure;
            }
            // Everything else, we want to 'succeed'.
            return RunStatus.Success;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            if (!CanRun(context))
            {
                yield return RunStatus.Success;
                yield break;
            }

            DecoratedChild.Start(context);
            while (DecoratedChild.Tick(context) == RunStatus.Running)
            {
                yield return RunStatus.Running;
            }

            DecoratedChild.Stop(context);
            if (DecoratedChild.LastStatus == RunStatus.Failure)
            {
                yield return GetContinuationStatus();
                yield break;
            }

            // Note: if the condition was met, and we succeeded in running the children, we HAVE to tell our parent
            // that we've ran successfully, or we'll skip down to the next child, regardless of whether we ran, or not.
            yield return RunStatus.Success;
            yield break;
        }
    }
}
