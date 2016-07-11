using System;
using System.Collections.Generic;
using BehaviorTree;
using EngineCore;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeFarFromTarget))]
	public class ActionFarFromTarget:ActionComposite,ITreeNodeHandle
	{
		public ActionFarFromTarget()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			var root = context as AITreeRoot;
			var per = root.Perception as BattlePerception;
			var distance = Node.distance;
			if (!root.GetDistanceByValueType(Node.valueOf, distance, out distance))
			{
				yield return RunStatus.Failure;
				yield break;
			}

			GVector3 target;

			var targetIndex = root[AITreeRoot.TRAGET_INDEX];
			if (targetIndex == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}
			var targetCharacter = root.Perception.State[(long)targetIndex] as BattleCharacter;
			if (targetCharacter == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			GVector3 noraml = per.View.NormalVerctor(root.Character.View.Transform.Position - targetCharacter.View.Transform.Position);
			target = noraml * distance + root.Character.View.Transform.Position;

			root.Character.View.MoveTo(target);
			while (per.View.Distance(root.Character.View.Transform.Position, target) > 0.2f)
			{
				yield return RunStatus.Running;
			}

			root.Character.View.StopMove();
			yield return RunStatus.Success;


		}

		private TreeNodeFarFromTarget Node;

		public void SetTreeNode(TreeNode node)
		{
			Node = node as TreeNodeFarFromTarget;
		}
	}
}

