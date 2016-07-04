using System;
using Layout.EditorAttributes;
//using System.Xml;

namespace Layout.LayoutElements
{
	public enum DamageType
	{
		Single,//单体
		Rangle 
	}

	public enum FilterType
	{
		ALL,
		OwnerTeam, //自己队友
		EmenyTeam, //敌人队伍
		Alliance   //联盟队伍 
	}

	public class DamageLayout:LayoutBase
	{
		public DamageLayout ()
		{
			
		}

		[Label("目标")]
		public TargetType target;
		[Label("伤害筛选类型")]
		public DamageType damageType;
		[Label("过滤方式")]
		public FilterType fiterType;
		[Label("半径")]
		public float radius;
		[Label("方向")]
		public float angle;
		[Label("方向偏移角")]
		public float offsetAngle;
		[Label("偏移向量")]
		public Vector3 offsetPosition;
	}
}

