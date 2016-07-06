using System;
using Layout.EditorAttributes;

namespace Layout.LayoutElements
{
	public enum MovementType
	{
		Line,//直线
		Paracurve,//抛物线
		Follow//跟踪
	}


	[EditorLayoutAttribute("子物体发射器")]
	public class MissileLayout:LayoutBase
	{
		public MissileLayout ()
		{
			maxLifeTime = -1;
			maxDistance = -1;
		}

		[Label("子物体资源目录")]
		[EditorResourcePath("prefab")]
		public string resourcesPath;
		[Label("轨迹类型")]
		public MovementType movementType;
		[Label("最大存活时间")]
		public float maxLifeTime;
		[Label("最大运动距离")]
		public float maxDistance;
		[Label("速度")]
		public float speed;
		[Label("起始骨骼")]
		public string fromBone;
		[Label("目标骨骼")]
		public string toBone;

		public override string ToString ()
		{
			return string.Format ("轨迹{0} 速度{1} 资源 {2}",movementType , speed, resourcesPath);
		}
	}
}

