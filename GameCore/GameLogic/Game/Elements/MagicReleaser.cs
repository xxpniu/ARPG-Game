using System;
using Layout;
using GameLogic.Game.LayoutLogics;
using EngineCore.Simulater;
using System.Collections.Generic;
using GameLogic.Game.Perceptions;

namespace GameLogic.Game.Elements
{
	public enum ReleaserStates
	{
		NOStart,
		Releasing,
		Completing,
		Ended
	}
	
	public class MagicReleaser:BattleElement<IMagicReleaser>
	{
		public MagicReleaser (MagicData magic,
			IReleaserTarget target, 
			GControllor controllor, 
			IMagicReleaser view):base(controllor,view)
		{
			ReleaserTarget = target;
			Magic = magic;

			OnExitedState = ReleaseAll;
		}



		public MagicData Magic{ private set; get;}

		public IReleaserTarget ReleaserTarget{ private set; get;}

		public ReleaserStates State{ private set; get; }

		public void SetState(ReleaserStates state)
		{
			State = state;
		}

		public void OnEvent(EventType eventType)
		{
			var per = this.Controllor.Perception as BattlePerception;

			LastEvent = eventType;
			for (var index =0; index<Magic.Containers.Count;index++) 
			{
				var i = Magic.Containers [index];
				if (i.type == eventType) 
				{
					var timeLine = i.line == null ? per.View.GetTimeLineByPath(i.layoutPath) : i.line;
					var player = new TimeLinePlayer (timeLine, this,i);  
					_add.Enqueue (player);
					if (i.type == EventType.EVENT_START)
					{
						if (startLayout != null) {
							throw new Exception ("Start layout must have one!");
						}
						startLayout = player;
					}
				}
			}
		}

		private TimeLinePlayer startLayout;

		public HashSet<GObject> _objs = new HashSet<GObject> ();

		public void AttachElement(GObject el)
		{
			if (_objs.Contains (el)) {
				return;
			}
			_objs.Add (el);
		}

		private Queue<TimeLinePlayer> _add = new Queue<TimeLinePlayer> ();
		private Queue<TimeLinePlayer> _del = new Queue<TimeLinePlayer> ();
		private List<TimeLinePlayer> _players = new List<TimeLinePlayer> ();

		public void TickTimeLines(GTime time)
		{
			while (_add.Count > 0) {
				_players.Add (_add.Dequeue ());
			}
			for (var i =0;i< _players.Count;i++)
			{
				if (_players [i].Tick (time)) {
					_del.Enqueue (_players [i]);
				}
			}
			while (_del.Count > 0)
			{
				var p = _del.Dequeue ();
				p.Destory ();
				_players.Remove (p);
			}
		}

		public bool IsCompleted
		{
			get{ 
				if (State == ReleaserStates.NOStart)
					return false;
				if (_add.Count > 0)
					return false;
				for (var i = 0; i < _players.Count; i++) {
					if (!_players [i].IsFinshed)
						return false;
				}

				foreach(var i in _objs){
					if (i.Enable)
						return false;
				}

				return true;
			}
		}

		public float GetLayoutTimeByPath(string path)
		{
			for (var i = 0; i < _players.Count;i++)
			{
				var p = _players[i];
				if (p.TypeEvent.layoutPath == path) return p.PlayTime;
			}
			return -1;
		}

		public EventType? LastEvent { get; private set; }

		private void ReleaseAll()
		{

			foreach (var i in _objs)
			{
				Destory(i);
			}

			_objs.Clear();
			foreach (var i in _players)
			{
				i.Destory();
			}

			_players.Clear();

		}


		public bool IsRuning(EventType type)
		{
			foreach (var i in _players)
			{
				if (i.IsFinshed) continue;
				if (i.TypeEvent.type == type) return true;
			}

			return false;
		}
	}
}

