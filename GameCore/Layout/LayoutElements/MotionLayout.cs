using System;
using Layout.EditorAttributes;


namespace Layout.LayoutElements
{
	[EditorLayoutAttribute("动画状态播放器")]
	public class MotionLayout:LayoutBase
	{
		public MotionLayout ()
		{
			
		}

		[Label("动画名称","settrigger")]
		public string motionName;
		[Label("动画播放者")]
		public TargetType targetType;

		public override string ToString ()
		{
			return string.Format ("目标{0} 动画{1} ",targetType , motionName);
		}
	}
}

