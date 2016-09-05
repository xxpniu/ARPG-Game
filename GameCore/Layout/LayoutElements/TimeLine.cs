using System;
using Layout.EditorAttributes;
using System.Collections.Generic;

namespace Layout.LayoutElements
{
	public class TimeLine
	{
		public TimeLine ()
		{
			Points = new List<TimePoint> ();
			Layouts = new List<LayoutBase> ();
			Time = 1f;
		}

		[Label("持续时间","单位秒(s)")]
		public float Time;


		public List<TimePoint> Points;

		public List<LayoutBase> Layouts;

		public LayoutBase FindLayoutByGuid(string guid)
		{
			foreach (var i in Layouts)
				if (i.GUID == guid)
					return i;
			return null;
		}

		public T FindLayoutByGuid<T>(string guid) where T:LayoutBase
		{
			return FindLayoutByGuid (guid) as T;
		}

		public void RemoveByGuid(string guid)
		{
            Points.RemoveAll((obj) => { return obj.GUID == guid; });

			foreach (var i in Layouts) {
				if (i.GUID == guid) {
					Layouts.Remove (i);
					break;
				}
			}
		}

		public List<TimePoint> FindPointByGuid(string guid)
		{
            var result = new List<TimePoint>();
			foreach (var i in Points) {
                if (i.GUID == guid)
                {
                    result.Add(i);
                }
			}
            return result;
		}
	}

	public class TimePoint
	{
		[Label("对应的Layout")]
		public string GUID;
		[Label("时间点")]
		public float Time;
	}
		
}

