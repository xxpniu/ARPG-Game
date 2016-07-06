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

	public enum EffectType
	{
		EffectGroup, //event group
		EffectConfig //config
	}

	[EditorLayoutAttribute("目标判定")]
	public class DamageLayout:LayoutBase
	{
		public DamageLayout ()
		{
			target = TargetType.Releaser;
			damageType = DamageType.Rangle;
			fiterType = FilterType.EmenyTeam;
			radius = 1;
			angle = 360;
			offsetAngle = 0;
			offsetPosition = new Vector3(){ x = 0, y=0,z=0};
			effectType = EffectType.EffectGroup;
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

		[Label("效果取值来源")]
		public EffectType effectType;
		[Label("执行的效果组Key")]
		public string effectKey;

		public override string ToString ()
		{
			return string.Format ("目标{0} 范围{1} 效果 {2}",target , damageType, effectKey);
		}
	}
}

