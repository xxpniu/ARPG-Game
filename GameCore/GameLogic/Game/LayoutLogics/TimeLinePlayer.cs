using System;
using Layout.LayoutElements;
using GameLogic.Game.Elements;
using EngineCore.Simulater;
using Layout;

namespace GameLogic.Game.LayoutLogics
{
	public class TimeLinePlayer
	{
		public TimeLinePlayer (TimeLine timeLine, MagicReleaser releaser, EventContainer eventType)
		{
			Line = timeLine;
			Releaser = releaser;
			TypeEvent = eventType;
		}

		private float lastTime = -1;
		private float startTime = 0;

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
			return result;
		}

		private bool isfinshed = false;

		public bool IsFinshed
		{
			get{ return isfinshed; }
		}
	}
}

