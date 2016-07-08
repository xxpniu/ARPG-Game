using System;

namespace EngineCore.Simulater
{
	public class GObject
	{
		private static volatile int _id = 1;

		private GObject ()
		{
			Index =_id++;
			if (_id == int.MaxValue) {
				_id = 1;
			}
		}

		public long Index{ private set; get; }

		public GObject (GControllor controllor) : this ()
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


	
	}
}

