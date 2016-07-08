using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    public delegate bool CanRunDecoratorDelegate(object context);

    public class Decorator : GroupComposite
    {
        public Decorator(CanRunDecoratorDelegate runFunc, Composite child)
            : this(child)
        {
            Runner = runFunc;
        }

        public Decorator(Composite child)
            : base(child)
        {
        }

        protected CanRunDecoratorDelegate Runner { get; private set; }

        public Composite DecoratedChild { get { return Children[0]; } }

        protected virtual bool CanRun(object context)
        {
            return true;
        }

        public override void Start(object context)
        {
            if (Children.Count != 1)
            {
                throw new ApplicationException("Decorators must have only one child.");
            }
            base.Start(context);
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            if (Runner != null)
            {
                if (!Runner(context))
                {
                    yield return RunStatus.Failure;
                    yield break;
                }
            }
            else if (!CanRun(context))
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
            if (DecoratedChild.LastStatus == RunStatus.Failure)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            yield return RunStatus.Success;
            yield break;
        }

    }
}
