using System;
using Layout.LayoutElements;
using GameLogic.Game.Elements;
using EngineCore.Simulater;
using Layout;
using System.Collections.Generic;

namespace GameLogic.Game.LayoutLogics
{
	public class TimeLinePlayer
	{
		public TimeLinePlayer (TimeLine timeLine, MagicReleaser releaser, EventContainer eventType)
		{
			Line = timeLine;
			Releaser = releaser;
			TypeEvent = eventType;
			players = new List<IParticlePlayer> ();
		}

		private float lastTime = -1;
		private float startTime = 0;
		private float totalTime = 0f;

		public TimeLine Line{ private set; get; }

		public MagicReleaser Releaser{ private set; get; }

		public EventContainer TypeEvent{ private set; get; }

		public bool Tick(GTime time)
		{
			if (lastTime < 0) 
			{
				startTime = time.Time;
				lastTime = time.Time - 0.001f;
				return false;
			}
			var old = lastTime - startTime;
			var now = time.Time- startTime;

			for(var i  = 0;i<Line.Points.Count;i++)
			{
				var point = Line.Points [i];
				if (point.Time > old && point.Time <= now) {
					var layout = Line.FindLayoutByGuid(point.GUID);
					LayoutBaseLogic.EnableLayout (layout, this);
				}
			}

			lastTime = time.Time;
			var result=  now > Line.Time ;
			isfinshed = result;
			totalTime = now;
			return result;
		}

		private bool isfinshed = false;

		public bool IsFinshed
		{
			get{ return isfinshed; }
		}

		private List<IParticlePlayer> players;

		public void AttachParticle(IParticlePlayer particle)
		{
			players.Add (particle);
		}

		public void Destory()
		{
			foreach (var i in players) 
            {
				if (i.CanDestory) {
					i.DestoryParticle ();
				}
			}
		}

		public float PlayTime { get { return totalTime; }}
	}
}

