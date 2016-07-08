using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    public abstract class Composite
    {
        public Composite()
        {
            CleanupHandlers = new Stack<CleanupHandler>();
            Guid = new Guid();
        }

        protected ContextChangeHandler ContextChanger { get; set; }

        private IEnumerator<RunStatus> _current { set; get; }

        /// <summary>
        ///   Simply an identifier to make sure each composite is 'unique'.
        ///   Useful for XML declaration parsing.
        /// </summary>
        public Guid Guid { get; set; }

        public virtual void Start(object context)
        {
            LastStatus = null;
            _current = Execute(context).GetEnumerator();

        }

        public virtual void Stop(object context)
        {
            Cleanup();
            if (_current != null)
            {
                _current.Dispose();
                _current = null;
            }

            if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running)
            {
                LastStatus = RunStatus.Failure;
            }
        }

        protected void Cleanup()
        {
            if (CleanupHandlers.Count != 0)
            {
                while (CleanupHandlers.Count != 0)
                {
                    CleanupHandlers.Pop().Dispose();
                }
            }
        }

        public RunStatus Tick(object context)
        {
            if (LastStatus.HasValue && LastStatus.Value != RunStatus.Running)
            {
                return LastStatus.Value;
            }
            if (_current == null)
            {
                throw new Exception("You Must start it!");
            }
            if (_current.MoveNext())
            {
                LastStatus = _current.Current;
            }
            else
            {
                throw new Exception("Nothing to run? Somethings gone terribly, terribly wrong!");
            }

            if (LastStatus != RunStatus.Running)
            {
                Stop(context);
            }
            return this.LastStatus.Value;
        }

        public abstract IEnumerable<RunStatus> Execute(object context);

        public RunStatus? LastStatus { private set; get; }

        public Stack<CleanupHandler> CleanupHandlers { get; private set; }

        public Composite Parent { set; get; }


        public string Name { set; get; }


    }

    public abstract class CleanupHandler : IDisposable
    {
        protected CleanupHandler(Composite owner, object context)
        {
            Owner = owner;
            Context = context;
        }

        protected Composite Owner { get; set; }

        private object Context { get; set; }

        private bool IsDisposed { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                DoCleanup(Context);
            }
        }

        #endregion

        protected abstract void DoCleanup(object context);
    }

    public static class CompositeDebuger
    {
        public static void Debug(string message) {
            if (Printer != null)
                Printer(message);
        }

        public static PrintDebugMessage Printer;

        public delegate void PrintDebugMessage(string message);
    }
}