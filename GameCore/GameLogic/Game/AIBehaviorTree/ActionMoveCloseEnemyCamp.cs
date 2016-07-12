using System;
using System.Collections.Generic;
using BehaviorTree;
using EngineCore;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeMoveCloseEnemyCamp))]
	public class ActionMoveCloseEnemyCamp:ActionComposite,ITreeNodeHandle
	{
		public ActionMoveCloseEnemyCamp()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			var root = context as AITreeRoot;
			var enemyTeamIndex = root.Perception.GetEnemyTeamIndex(root.Character.TeamIndex);
			GVector3 bornPos = root.Perception.View.GetBornPosByTeamIndex(enemyTeamIndex);

			root.Character.View.MoveTo(bornPos);
			while (root.Perception.View.Distance(bornPos, root.Character.View.Transform.Position)>1)
			{
				yield return RunStatus.Running;
			}

			root.Character.View.StopMove();
			yield return RunStatus.Success;
		}

		public void SetTreeNode(TreeNode node)
		{
			Node = node as TreeNodeMoveCloseEnemyCamp;
		}

		public TreeNodeMoveCloseEnemyCamp Node;

		public override void Stop(ITreeRoot context)
		{
			if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running)
			{
				var root = context as AITreeRoot;
				root.Character.View.StopMove();
			}
			base.Stop(context);
		}
	}
}

