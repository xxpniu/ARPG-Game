using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
  
	public abstract class Decorator : GroupComposite
    {
       
        public Decorator(Composite child)
            : base(child)
        {
        }

        public Composite DecoratedChild { get { return Children[0]; } }
    }
}
