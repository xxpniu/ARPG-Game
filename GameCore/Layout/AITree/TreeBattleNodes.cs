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
		[Label("距离")]
		public float Distance;

		[Label("目标保存类型")]
		public TargetSaveType saveType;

		[Label("过滤类型")]
		public TargetFilterType filter;

		[Label("阵营类型")]
		public TargetTeamType teamType;
	}

	[EditorAITreeNode("等待时间", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeWaitForSeconds : TreeNode
	{
		[Label("等待秒数")]
		public float seconds;

	}

	[EditorAITreeNode("释放技能", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeReleaseMagic : TreeNode
	{
		[Label("魔法Key")]
		public string magicKey;
	}

	[EditorAITreeNode("判断目标距离", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeDistancTarget : TreeNode
	{
		[Label("距离")]
		public float distance;
	}

	[EditorAITreeNode("靠近目标", "Act", "战斗节点", AllowChildType.None)]
	public class TreeNodeMoveToTarget : TreeNode
	{
		[Label("停止距离")]
		public float distance;
	}
}

