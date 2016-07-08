using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    public delegate object ContextChangeHandler(object original);

    /// <summary>
    ///   The base sequence class. This will execute each branch of logic, in order.
    ///   If all branches succeed, this composite will return a successful run status.
    ///   If any branch fails, this composite will return a failed run status.
    /// </summary>
    public class Sequence : GroupComposite
    {
        public Sequence(params Composite[] children)
            : base(children)
        {
        }

        public Sequence(ContextChangeHandler contextChange, params Composite[] children)
            : this(children)
        {
            ContextChanger = contextChange;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            if (ContextChanger != null)
            {
                context = ContextChanger(context);
            }
            foreach (Composite node in Children)
            {
                node.Start(context);
                while (node.Tick(context) == RunStatus.Running)
                {
                    Selection = node;
                    yield return RunStatus.Running;
                }

                Selection = null;
                node.Stop(context);

                if (node.LastStatus == RunStatus.Failure)
                {
                    yield return RunStatus.Failure;
                    yield break;
                }
            }
            yield return RunStatus.Success;
            yield break;
        }
    }
}
