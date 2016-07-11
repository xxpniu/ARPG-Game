using System;
using System.Collections.Generic;

namespace EngineCore.Simulater
{
	public abstract class GState
	{
		private Dictionary<long,GObject> _elements = new Dictionary<long, GObject>();


		public GPerception Perception{ protected set; get; }

		private bool Enable = false;

		public void Init()
		{
			OnInit();
		}

		protected virtual void OnInit(){
			
		}

		public GObject this[long index] 
		{
			get
			{
				GObject outObj ;
				if (_elements.TryGetValue (index, out outObj))
					return outObj;
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
			while (_add.Count > 0) {
				var temp = _add.Dequeue ();
				if (_elements.ContainsKey (temp.Index))
					continue;
				_elements.Add (temp.Index, temp);
				GObject.JoinState (temp);
			}
			foreach (var i in _elements) 
			{
				if (i.Value.Enable) 
				{
					i.Value.Controllor.GetAction (time,i.Value)
						.Execute (time, i.Value);
				}

				if (!i.Value.Enable && i.Value.CanDestory) {
					_del.Enqueue (i.Value);
				}
			}
			while (_del.Count > 0) {
				var temp = _del.Dequeue ();
				_elements.Remove (temp.Index);
				GObject.ExitState (temp);
			}
		}

		private Queue<GObject> _del = new Queue<GObject>();
		private Queue<GObject> _add = new Queue<GObject> ();

		public static void Tick(GState state,GTime now)
		{
			if (state.Enable) {
				state.Tick (now);
			}
			else {
				throw new Exception ("You can't tick a state before you start it.");
			}
		}

		public  void AddElement(GObject el)
		{
			_add.Enqueue (el);
		}
			

		public delegate bool EachCondtion<T>(T el)  where T : GObject ;

		public void Each<T>(EachCondtion<T> cond)  where T : GObject{
			foreach (var i in _elements) 
			{
				if (!i.Value.Enable) continue;
				var temp = i.Value as T;
				if (temp == null)
					continue;
				if (cond (temp))
					return;
			}
		} 


	}
}

