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
			float Seconds = Node.seconds;
			var root = context as AITreeRoot;
			switch (Node.valueOf)
			{
				case WaitTimeValueOf.AttackSpeed:
					{
						var data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData>(root.Character.ConfigID);
						if (data == null)
						{
							yield return RunStatus.Failure;
							yield break;
						}
						Seconds = data.AttackSpeed;
					}
					break;
			}
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

		private TreeNodeWaitForSeconds Node;

		public void SetTreeNode(TreeNode node)
		{
			Node= node as TreeNodeWaitForSeconds;
			//Seconds = n.seconds;
		}

    }
}
