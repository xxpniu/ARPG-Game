using System;
using Layout.EditorAttributes;

namespace Layout.LayoutElements
{
	public enum ParticleDestoryType
	{
		Normal,
		LayoutTimeOut,
		Time
	}

	[EditorLayout("粒子播放器")]
	public class ParticleLayout:LayoutBase
	{
		public ParticleLayout ()
		{
			
		}

		[Label("资源","客户端显示的资源目录")]
		[EditorResourcePath]
		public string path;

		[Label("开始对象")]
		public TargetType fromTarget;
		[Label("目标对象")]
		public TargetType toTarget;
		[Label("起始骨骼","绑定目标骨骼")]
		[EditorBone]
		public string fromBoneName;

		[Label("目标骨骼","绑定目标骨骼")]
		[EditorBone]
		public string toBoneName;

        [Label("绑定骨骼")]
        public bool Bind;

		[Label("销毁类型")]
		public ParticleDestoryType destoryType;
      
		[Label("销毁时间")]
		public float destoryTime;
		public override string ToString ()
		{
			return string.Format ("资源{0}",path);
		}

	}
}

