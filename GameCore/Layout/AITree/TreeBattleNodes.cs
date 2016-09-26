using System;
using Layout.EditorAttributes;
using Proto;

namespace Layout.AITree
{

	public enum TargetSaveType
	{
		SkillReleaseTarget,
		MoveTarget
	}

	public enum TargetSelectType
	{
		Nearest,
		Random,
		HPMax,
		HPMin,
		HPRateMax,
		HPRateMin
	}

	public enum TargetFilterType
	{
		None,
		Hurt
	}

    public enum TargetBodyType
    { 
        ALL,
        NoBuilding
    }

	[EditorAITreeNode("查找目标", "Act", "战斗节点",AllowChildType.None)]
	public class TreeNodeFindTarget:TreeNode
	{
		[Label("取值来源")]
		public DistanceValueOf valueOf;
		[Label("距离")]
		public float Distance;

		[Label("目标保存类型")]
		public TargetSaveType saveType;

		[Label("挑选方式")]
		public TargetSelectType selectType;

		[Label("过滤方式")]
		public TargetFilterType filter;

        [Label("使用魔法配置表")]
        public bool useMagicConfig;

		[Label("阵营类型")]
		public TargetTeamType teamType;

        [Label("重新查找")]
        public bool findNew;

	}

	public enum WaitTimeValueOf
	{
		
		Value,
		AttackSpeed
	}

	[EditorAITreeNode("等待时间", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeWaitForSeconds : TreeNode
	{
		[Label("取值来源")]
		public WaitTimeValueOf valueOf= WaitTimeValueOf.Value;
		[Label("等待秒数")]
		public float seconds;

	}


	public enum MagicValueOf
	{ 
	    BlackBoard,
		MagicKey
	}

	[EditorAITreeNode("释放技能", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeReleaseMagic : TreeNode
	{
		[Label("魔法Key")]
		public string magicKey = string.Empty;
		[Label("取值来源")]
		public MagicValueOf valueOf = MagicValueOf.MagicKey;
	}

	public enum DistanceValueOf
	{
		BlackboardMaigicRangeMin,
		BlackboardMaigicRangeMax,
		Value,
        ViewDistance
	}

	public enum CompareType
	{ 
	    Less,
		Greater
	}
	[EditorAITreeNode("判断目标距离", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeDistancTarget : TreeNode
	{
		[Label("取值来源")]
		public DistanceValueOf valueOf = DistanceValueOf.Value;
		[Label("距离")]
		public float distance=1;
		[Label("比较类型")]
		public CompareType compareType = CompareType.Less;
	}

	[EditorAITreeNode("靠近目标", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeMoveToTarget : TreeNode
	{
		[Label("取值来源")]
		public DistanceValueOf valueOf = DistanceValueOf.Value;
		[Label("停止距离")]
		public float distance=1;


	}
	public enum MagicResultType
	{
		Random,
		Frist,
		Sequence //顺序以此
	}

	[EditorAITreeNode("选择可释放魔法", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeSelectCanReleaseMagic : TreeNode
	{
		[Label("魔法选择类型")]
		public MagicResultType resultType;
	}

	// OK up
	[EditorAITreeNode("远离目标", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeFarFromTarget : TreeNode
	{
		[Label("取值来源")]
		public DistanceValueOf valueOf = DistanceValueOf.Value;
		[Label("远离距离")]
		public float distance = 1;
	}



	[EditorAITreeNode("比较目标数", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeCompareTargets : TreeNode
	{ 
		[Label("距离取值来源")]
		public DistanceValueOf valueOf = DistanceValueOf.Value;

	    [Label("距离")]
		public float Distance;

		[Label("比较值")]
		public int compareValue;

		[Label("比较类型")]
		public CompareType compareType;

		[Label("阵营类型")]
		public TargetTeamType teamType;
	}

	[EditorAITreeNode("靠近敌方阵营", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeMoveCloseEnemyCamp : TreeNode 
	{
	
	}

    [EditorAITreeNode("处理移动输入", "Act", "网络输入", AllowChildType.None)]
    public class TreeNodeNetActionMove:TreeNode
    {
        
    }
    [EditorAITreeNode("处理释放技能输入", "Act", "网络输入", AllowChildType.None)]
    public class TreeNodeNetActionSkill : TreeNode { }
}

