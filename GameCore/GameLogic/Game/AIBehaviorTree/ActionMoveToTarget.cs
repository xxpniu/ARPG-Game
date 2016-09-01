using System;
using System.Collections.Generic;
using BehaviorTree;
using EngineCore;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeMoveToTarget))]
	public class ActionMoveToTarget:ActionComposite,ITreeNodeHandle
	{
		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			var root = context as AITreeRoot;
			var index = root[AITreeRoot.TRAGET_INDEX];
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
			float stopDistance;
			if (!root.GetDistanceByValueType(Node.valueOf, Node.distance, out stopDistance))
			{
				yield return RunStatus.Failure;
				yield break;
			}
            var per = root.Perception as BattlePerception;
            //var offset = new GVector3(r);
			//float lastTime = root.Time-2;
			var pos = target.View.Transform.Position;
            per.CharacterMoveTo(root.Character, pos);
			view = root.Character.View;

			while (root.Perception.Distance(target, root.Character) > stopDistance)
			{
                if (GVector3.Distance(pos, target.View.Transform.Position) > stopDistance)
                {
                    per.CharacterMoveTo(root.Character, target.View.Transform.Position);
                    pos = target.View.Transform.Position;
                }

				if(!target.Enable)
				{
                    per.CharacterStopMove(root.Character);
					yield return RunStatus.Failure;
					yield break;
				}
				yield return RunStatus.Running;
			}

            var time = root.Time;
            if (time + 0.2f < root.Time)
            {
                yield return RunStatus.Running;
            }
			per.CharacterStopMove(root.Character);

			yield return RunStatus.Success;

		}

		private TreeNodeMoveToTarget Node;

		public void SetTreeNode(TreeNode node)
		{
			var n = node as TreeNodeMoveToTarget;
			Node = n;
		}

		private IBattleCharacter view;

		//private float stopDistance = 0f;

		public override void Stop(ITreeRoot context)
		{
            var root = context as AITreeRoot;
            var per = root.Perception as BattlePerception;
            if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running && view != null)
            {
                per.CharacterStopMove(root.Character);
            }
			base.Stop(context);
		}
	}
}

