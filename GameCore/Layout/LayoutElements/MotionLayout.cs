using System;
using Layout.EditorAttributes;


namespace Layout.LayoutElements
{
	[EditorLine("动画状态播放器")]
	public class MotionLayout:LayoutBase
	{
		public MotionLayout ()
		{
			
		}

		[Label("动画名称","settrigger")]
		public string motionName;
		[Label("动画播放者")]
		public TargetType targetType;
	}
}

