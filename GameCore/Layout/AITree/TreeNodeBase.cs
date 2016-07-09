using System;
using Layout.AITree;
using Layout.EditorAttributes;

namespace Layout.AITree
{
	[EditorAITreeNode("选择节点(Selector)","Sel","基础节点")]
	public class TreeNodeSelector :TreeNode{}
	[EditorAITreeNode("顺序节点(Sequence)", "Seq", "基础节点")]
	public class TreeNodeSequence : TreeNode { }
	[EditorAITreeNode("并行选择节点(ParallelSelector)", "PSel", "基础节点")]
	public class TreeNodeParallelSelector : TreeNode { }
	[EditorAITreeNode("顺序节点(ParallelSequence)", "PSeq", "基础节点")]
	public class TreeNodeParallelSequence : TreeNode { }
	[EditorAITreeNode("分段概率选择节点(ProbabilitySelector)", "PRSel", "基础节点", AllowChildType.Probability)]
	public class TreeNodeProbabilitySelector : TreeNode { }
	[EditorAITreeNode("分段概率节点(ProbabilityNode)", "PRNode", "基础节点", AllowChildType.One)]
	public class TreeNodeProbabilityNode : TreeNode{ }
}

