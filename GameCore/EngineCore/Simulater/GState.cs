using System;
using System.Collections.Generic;

namespace EngineCore.Simulater
{
    public abstract class GState
    {
        private int lastIndex = 0;

        public int NextElementID()
        {
            lastIndex++;
            return lastIndex;
        }

        private Dictionary<int, GObject> _elements = new Dictionary<int, GObject>();
        private LinkedList<GObject> _elementList = new LinkedList<GObject>();

        public GPerception Perception { protected set; get; }

        private bool Enable = false;

        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {

        }

        public void Pause(bool isPause)
        {
            Enable = !isPause;
        }

        public GObject this[int index]
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

        public void Start(GTime time)
        {
            Enable = true;
            this.Tick(time);
        }

        public void Stop(GTime time)
        {
            foreach (var i in _elements)
            {
                GObject.Destory(i.Value);
            }
            this.Tick(time);
            Enable = false;
        }

        private void Tick(GTime time)
        {
            if (!Enable) return;
            var current = _elementList.First;
           
            while (current != null)
            {
                var next = current.Next;
                if (current.Value.Enable)
                {
                    current.Value.Controllor.GetAction(time, current.Value)
                        .Execute(time, current.Value);
                }
                if (!current.Value.Enable && current.Value.CanDestory)
                {
                    _elements.Remove(current.Value.Index);
                    GObject.ExitState(current.Value);
                    _elementList.Remove(current);
                }
                current = next;
            }
        }

        public static void Tick(GState state, GTime now)
        {
            if (state.Enable)
            {
                state.Tick(now);
            }
            else {
                throw new Exception("You can't tick a state before you start it.");
            }
        }

        internal bool AddElement(GObject el)
        {
            var temp = el;
            if (_elements.ContainsKey(temp.Index))
                return false;
            _elements.Add(temp.Index, temp);
            _elementList.AddLast(temp);
            GObject.JoinState(temp);
            return true;
        }

        public delegate bool EachCondtion<T>(T el) where T : GObject;

        public void Each<T>(EachCondtion<T> cond) where T : GObject
        {
            foreach (var i in _elements)
            {
                if (!i.Value.Enable) continue;
                if (i.Value is T)
                {
                    var temp = i.Value as T;
                    if (cond(temp))
                        return;
                }
                continue;
            }
        }

        public bool IsEnable { get { return Enable; } }

    }
}

