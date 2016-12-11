using System;
using System.Collections.Generic;

namespace EngineCore.Simulater
{
	public abstract class GState
	{
		private Dictionary<long,GObject> _elements = new Dictionary<long, GObject>();
        private LinkedList<GObject> _elementList = new LinkedList<GObject>();

		public GPerception Perception{ protected set; get; }

		private bool Enable = false;

		public void Init()
		{
			OnInit();
		}

		protected virtual void OnInit(){
			
		}

        public void Pause(bool isPause)
        {
            Enable = !isPause;
        }

		public GObject this[long index]
        {
            get
            {
                GObject outObj;
                if (_elements.TryGetValue(index, out outObj))
                {
                    if (outObj.Enable) return outObj;
                }
				return null;
			}
        }

		public void Start (GTime time)
		{
			Enable = true;
			this.Tick (time);
		}

		public  void Stop(GTime time)
		{
			foreach (var i in _elements) {
				GObject.Destory (i.Value);
			}
			this.Tick (time);
			Enable = false;
		}

		private void Tick(GTime time)
		{
            if (!Enable) return;
            var next = _elementList.First;
            while (next != null)
            {
                if (next.Value.Enable)
                {
                    next.Value.Controllor.GetAction(time, next.Value)
                        .Execute(time, next.Value);
                }
                if (!next.Value.Enable && next.Value.CanDestory)
                {
                    _elements.Remove(next.Value.Index);
                    GObject.ExitState(next.Value);
                    _elementList.Remove(next);
                }
                next = next.Next;
            }
		}

		public static void Tick(GState state,GTime now)
		{
			if (state.Enable) {
				state.Tick (now);
			}
			else {
				throw new Exception ("You can't tick a state before you start it.");
			}
		}

        internal void AddElement(GObject el)
        {

            var temp = el;
            if (_elements.ContainsKey(temp.Index))
                return;
            _elements.Add(temp.Index, temp);
            _elementList.AddLast(temp);
            GObject.JoinState(temp);

        }
			

		public delegate bool EachCondtion<T>(T el)  where T : GObject ;

		public void Each<T>(EachCondtion<T> cond)  where T : GObject{
			foreach (var i in _elements) 
			{
				if (!i.Value.Enable) continue;
                if (i.Value is T)
                {
                    var temp = i.Value as T;
                    if (cond (temp))
					return;
                }
                continue;
			}
		} 

        public bool IsEnable { get { return Enable; }}
	}
}

