using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorTree;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	//TreeNodeWaitForSeconds
	[TreeNodeParse(typeof(TreeNodeWaitForSeconds))]
	public class BWaitForSeconds : BehaviorTree.Composite,ITreeNodeHandle
    {
        public override IEnumerable<BehaviorTree.RunStatus> Execute(ITreeRoot context)
        {
			float time = context.Time;
			//var lastTime = time;
			while (time + Seconds >= context.Time)
                yield return BehaviorTree.RunStatus.Running;
            yield return BehaviorTree.RunStatus.Success;
        }

		public override Composite FindGuid(string id)
		{
			if (Guid == id) return this;
			return null;
		}

		public void SetTreeNode(TreeNode node)
		{
			var n = node as TreeNodeWaitForSeconds;
			Seconds = n.seconds;
		}

		public float Seconds { set; get; }

    }
}
