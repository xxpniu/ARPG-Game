using System;
using Layout.EditorAttributes;

namespace Layout.AITree
{
	[EditorAITreeNode("结果取反", "Dec", AllowChildType.One)]
	public class TreeNodeNegation : TreeNode{}
	[EditorAITreeNode("运行直到结果为Failure(返回Failure)", "Dec", AllowChildType.One)]
	public class TreeNodeRunUnitlFailure : TreeNode { }
	[EditorAITreeNode("运行直到结果为Success(返回Success)", "Dec", AllowChildType.One)]
	public class TreeNodeRunUnitlSuccess : TreeNode { }
	[EditorAITreeNode("间隔时间执行", "Dec", AllowChildType.One)]
	public class TreeNodeTick : TreeNode {
		[Label("间隔时间(秒)")]
		public float tickTime;
	}
	[EditorAITreeNode("间隔时间执行直到Success(返回Success)", "Dec", AllowChildType.One)]
	public class TreeNodeTickUntilSuccess : TreeNode 
	{
		[Label("间隔时间(秒)")]
		public float tickTime;
	}

	[EditorAITreeNode("终止树并启动子树", "Dec", AllowChildType.One)]
	public class TreeNodeBreakTreeAndRunChild:TreeNode{ }
}

