using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    public abstract class GroupComposite : Composite
	{
		protected GroupComposite(params Composite[] children)
        {
            Children = new List<Composite>(children);
            foreach (Composite composite in Children)
            {
                if (composite != null)
                {
                   composite.Parent = this;
                }
            }
        }

        public List<Composite> Children { get; set; }

        public Composite Selection { get; protected set; }

        public override void Start(ITreeRoot context)
		{
            base.Start(context);
        }

        public override void Stop(ITreeRoot context)
        {
            base.Stop(context);
            foreach (var i in Children)
            {
                if (i.LastStatus.HasValue&& i.LastStatus == RunStatus.Running)
                    i.Stop(context);
            }
        }
    }
}
