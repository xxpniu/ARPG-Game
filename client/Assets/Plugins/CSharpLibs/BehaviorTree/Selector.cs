using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    /// <summary>
    ///   The base selector class. This will attempt to execute all branches of logic, until one succeeds. 
    ///   This composite will fail only if all branches fail as well.
    /// </summary>
    public abstract class Selector : GroupComposite
    {
        public Selector(params Composite[] children)
            : base(children)
        {
        }

        public abstract override IEnumerable<RunStatus> Execute(ITreeRoot context);
    }

}
