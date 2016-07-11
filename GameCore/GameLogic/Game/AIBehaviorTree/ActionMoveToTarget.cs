﻿using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeMoveToTarget))]
	public class ActionMoveToTarget:ActionComposite,ITreeNodeHandle
	{
		public ActionMoveToTarget()
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
			float stopDistance = Node.distance;
			switch (Node.valueOf)
			{
				case DistanceValueOf.BlackboardMaigicRangeMax:
					{
						var data = root["MagicID"];
						if (data == null)
						{
							yield return RunStatus.Failure;
							yield break;
						}
					}
					break;
				case DistanceValueOf.BlackboardMaigicRangeMin:
					{
						var data = root["MagicID"];
						if (data == null)
						{
							yield return RunStatus.Failure;
							yield break;
						}
					}
					break;
				case DistanceValueOf.Value:
					break;
			}
			//float lastTime = root.Time-2;
			var pos = target.View.Transform.Position;
			root.Character.View.MoveTo(pos);
			view = root.Character.View;
			while (root.Perception.Distance(target, root.Character) > stopDistance)
			{
				if (root.Perception.View.Distance(pos, target.View.Transform.Position) > 0.2f)
				{
					root.Character.View.MoveTo(target.View.Transform.Position);
					pos = target.View.Transform.Position;
				}
				if(!target.Enable)
				{
					root.Character.View.StopMove();
					yield return RunStatus.Failure;
					yield break;
				}
				yield return RunStatus.Running;
			}

			root.Character.View.StopMove();

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
			
			if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running && view != null)
				view.StopMove();
			base.Stop(context);
		}
	}
}
