using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    public delegate RunStatus ActionDelegate(object context);

    public delegate void ActionSucceedDelegate(object context);

    /// <summary>
    ///   The base Action class. A simple, easy to use, way to execute actions, and return their status of execution.
    ///   These are normally considered 'atoms' in that they are executed in their entirety.
    /// </summary>
    public class Action : Composite
    {
        public Action()
        {
        }

        public Action(ActionDelegate action )
        {
            Runner = action;
        }

        public Action(ActionSucceedDelegate action )
        {
            SucceedRunner = action;
        }

        public ActionDelegate Runner { get; private set; }

        public ActionSucceedDelegate SucceedRunner { get; private set; }

        /// <summary>
        ///   Runs this action, and returns a <see cref = "RunStatus" /> describing it's current state of execution.
        ///   If this method is not overriden, it returns <see cref = "RunStatus.Failure" />.
        /// </summary>
        /// <returns></returns>
        protected virtual RunStatus Run(object context)
        {
            return RunStatus.Failure;
        }

        public override sealed IEnumerable<RunStatus> Execute(object context)
        {
            if (Runner != null)
            {
                yield return Runner(context);
            }
            else if (SucceedRunner != null)
            {
                SucceedRunner(context);
                yield return RunStatus.Success;
            }
            else
            {
                yield return Run(context);
            }
        }
    }
}
