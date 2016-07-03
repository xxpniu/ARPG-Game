using System;

namespace Layout.EditorAttributes
{
	//显示可以在时间轴上的点
	public class TimeLinePropertyAttribute
	{
		public TimeLinePropertyAttribute (Type type)
		{
			PropertyType = type;
		}

		public Type PropertyType{ set; get;}
	}
}

