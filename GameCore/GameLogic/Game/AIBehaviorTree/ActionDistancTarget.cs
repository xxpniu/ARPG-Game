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

			var target = root.Perception.State[(int)index] as BattleCharacter;
			if (target == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			float distance;
			if (!root.GetDistanceByValueType(Node.valueOf, Node.distance, out distance))
			{
				yield return RunStatus.Failure;
				yield break;
			}
			switch (Node.compareType)
			{
				case CompareType.Less:
					if (root.Perception.Distance(target, root.Character) > distance)
						yield return RunStatus.Failure;
					else
						yield return RunStatus.Success;
					break;
				case CompareType.Greater:
					if (root.Perception.Distance(target, root.Character) > distance)
						yield return RunStatus.Success;
					else
						yield return RunStatus.Failure;
					break;
			}


		}

		private TreeNodeDistancTarget Node;

        public void SetTreeNode(TreeNode node)
        {
            Node = node as TreeNodeDistancTarget;
        }

		//private float distance;
	}
}

