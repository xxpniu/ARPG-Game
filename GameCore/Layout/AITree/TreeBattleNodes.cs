using System;
using Layout.EditorAttributes;

namespace Layout.AITree
{

	public enum TargetSaveType
	{
		SkillReleaseTarget,
		MoveTarget
	}

	public enum TargetFilterType
	{
		Nearest,
		Random
	}

	public enum TargetTeamType
	{
		ALL,
		Enemy,
		OwnTeam
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

		[Label("过滤类型")]
		public TargetFilterType filter;

		[Label("阵营类型")]
		public TargetTeamType teamType;
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
		Value
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


}

