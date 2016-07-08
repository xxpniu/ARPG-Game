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

        public override void Start(object context)
        {
            CleanupHandlers.Push(new ChildrenCleanupHandler(this, context));
            base.Start(context);
        }

        public void AddChild(Composite child)
        {
            if (child != null)
            {
                child.Parent = this;
                Children.Add(child);
            }
        }

        public void InsertChild(int index, Composite child)
        {
            if (child != null)
            {
                child.Parent = this;
                Children.Insert(index, child);
            }
        }

        #region Nested type: ChildrenCleanupHandler

        protected class ChildrenCleanupHandler : CleanupHandler
        {
            public ChildrenCleanupHandler(GroupComposite owner, object context)
                : base(owner, context)
            {
            }

            protected override void DoCleanup(object context)
            {
                foreach (Composite composite in (Owner as GroupComposite).Children)
                {
                    composite.Stop(context);
                }
            }
        }

        #endregion

        public override void Stop(object context)
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
