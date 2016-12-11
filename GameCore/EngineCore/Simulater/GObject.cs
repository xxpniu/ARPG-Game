using System;

namespace EngineCore.Simulater
{
	public class GObject
	{
	
		public int Index{ private set; get; }

        private GObject(int index)
        {
            this.Index = index;
        }

        public GObject (GControllor controllor) :this(controllor.Perception.State.NextElementID())
		{
			Controllor = controllor;	
		}

		public GControllor Controllor {private set; get; }

		public void SetControllor(GControllor controllor)
		{
			OnChangedControllor (this.Controllor, controllor);
			this.Controllor = controllor;
		}

		private bool HadBeenDestory = false;

		public bool Enable{private set; get; }


        public bool IsAliveAble { get { return !HadBeenDestory;}}

		#region Events

		protected virtual void OnJoinState()
		{
			
		}

		protected virtual void OnExitState()
		{
			
		}

		protected virtual void OnChangedControllor(GControllor old, GControllor current)
		{
			
		}

		#endregion

		private DateTime? destoryTime;

		public bool CanDestory
        {
            get
            {
                if (this.Enable) return false;
                if (destoryTime.HasValue)
                {
                    return destoryTime.Value < DateTime.Now;
				}

				return true;
            } 
        }

		public static void JoinState(GObject el)
		{
			if (!el.HadBeenDestory)
				el.Enable = true;
			el.OnJoinState();
		}

		public static void ExitState(GObject el)
		{
			el.OnExitState ();
		}

		public static void Destory(GObject el)
		{
			el.HadBeenDestory = true;
			if (el.Enable) 
			{
				el.Enable = false;
			}

		}


		public static void Destory(GObject el, float time)
		{
			if(time>0)
			el.destoryTime = DateTime.Now.AddSeconds(time);
			Destory(el);
		}

	
	}
}

