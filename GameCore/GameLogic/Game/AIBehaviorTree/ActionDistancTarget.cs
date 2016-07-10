using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeDistancTarget))]
	public class ActionDistancTarget : ActionComposite,ITreeNodeHandle
	{
		public ActionDistancTarget()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{

			var root = context as AITreeRoot;
			var index = root["TargetIndex"];
			if (index == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			var target = root.Perception.State[(long)index] as BattleCharacter;
			if (target == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			if (root.Perception.Distance(target, root.Character) > distance)
				yield return RunStatus.Failure;
			else
				yield return RunStatus.Success;

		}

		public void SetTreeNode(TreeNode node)
		{
			var n = node as TreeNodeDistancTarget;
			distance = n.distance;
		}

		private float distance;
	}
}

