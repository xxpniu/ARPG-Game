using System;
using Layout.AITree;
using Layout.EditorAttributes;

namespace Layout.AITree
{
	[EditorAITreeNode("选择节点(Selector)","Sel")]
	public class TreeNodeSelector :TreeNode{}
	[EditorAITreeNode("顺序节点(Sequence)", "Seq")]
	public class TreeNodeSequence : TreeNode { }
	[EditorAITreeNode("并行选择节点(ParallelSelector)", "PSel")]
	public class TreeNodeParallelSelector : TreeNode { }
	[EditorAITreeNode("顺序节点(ParallelSequence)", "PSeq")]
	public class TreeNodeParallelSequence : TreeNode { }
	[EditorAITreeNode("分段概率选择节点(ProbabilitySelector)", "PRSel", AllowChildType.Probability)]
	public class TreeNodeProbabilitySelector : TreeNode { }
	[EditorAITreeNode("分段概率节点(ProbabilityNode)", "PRNode", AllowChildType.One)]
	public class TreeNodeProbabilityNode : TreeNode{ }
}

