using System;
using Layout.AITree;
using Layout.EditorAttributes;

namespace Layout.AITree
{
	[EditorAITreeNode("选择节点","Sel","基础节点")]
	public class TreeNodeSelector :TreeNode{}
	[EditorAITreeNode("顺序节点", "Seq", "基础节点")]
	public class TreeNodeSequence : TreeNode { }
	[EditorAITreeNode("并行选择节点", "PSel", "基础节点")]
	public class TreeNodeParallelSelector : TreeNode { }
	[EditorAITreeNode("顺序节点", "PSeq", "基础节点")]
	public class TreeNodeParallelSequence : TreeNode { }
	[EditorAITreeNode("分段概率选择节点", "PRSel", "基础节点", AllowChildType.Probability)]
	public class TreeNodeProbabilitySelector : TreeNode { }
	[EditorAITreeNode("分段概率子节点", "PRNode", "基础节点", AllowChildType.One)]
	public class TreeNodeProbabilityNode : TreeNode{
		[Label("概率")]
		public int probability = 1;
	}
}

