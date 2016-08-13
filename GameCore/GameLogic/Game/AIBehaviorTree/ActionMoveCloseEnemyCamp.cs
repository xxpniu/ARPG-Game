using System;
using System.Collections.Generic;
using BehaviorTree;
using EngineCore;
using GameLogic.Game.Perceptions;
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
			

            var character = root.Perception.GetSingleTargetUseRandom(root.Character);

            if (character == null) {
                yield return RunStatus.Failure;
                yield break;
            }
            GVector3 bornPos = character.View.Transform.Position;

            var per = root.Perception as BattlePerception;

            per.CharacterMoveTo(root.Character, bornPos);
			while (root.Perception.View.Distance(bornPos, root.Character.View.Transform.Position)>1)
			{
				yield return RunStatus.Running;
			}

            per.CharacterStopMove(root.Character);
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
                var per = root.Perception as BattlePerception;
                per.CharacterStopMove(root.Character);
    
			}
			base.Stop(context);
		}
	}
}

