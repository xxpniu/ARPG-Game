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
            
        }

		public string Guid { set; get; }

        private IEnumerator<RunStatus> _current { set; get; }
           
        public virtual void Start(ITreeRoot context)
        {
            _attachVariables.Clear();
            LastStatus = null;
            _current = Execute(context).GetEnumerator();

        }

        public virtual void Stop(ITreeRoot context)
        {
            if (_current != null)
            {
                _current.Dispose();
                _current = null;
            }

            if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running)
            {
                Attach("failure","block by other");
                LastStatus = RunStatus.Failure;
            }
        }

        public RunStatus Tick(ITreeRoot context)
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

        public abstract IEnumerable<RunStatus> Execute(ITreeRoot context);

        public RunStatus? LastStatus { private set; get; }

        private Dictionary<string, object> _attachVariables = new Dictionary<string, object>();

        public string Name { set; get; }

		public abstract Composite FindGuid(string id);

		public static Composite FindCompositByGuid(Composite c, string guid) 
		{
			return c.FindGuid(guid);
		}

        protected void Attach(string key, object val)
        {
            if (_attachVariables.ContainsKey(key)) _attachVariables.Remove(key);
            _attachVariables.Add(key,val);
        }

        public void DebugVals(Action<string, object> callback) 
        {
            foreach (var i in _attachVariables)
            {
                callback(i.Key, i.Value);
            }
        }

    }
}