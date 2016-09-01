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
            var target  = character;

            var per = root.Perception as BattlePerception;


            var pos = target.View.Transform.Position;
            per.CharacterMoveTo(root.Character, pos);
            var view = root.Character.View;
            var last = root.Time;

            while (root.Perception.Distance(target, root.Character) > 0f)
            {
                
                if ((root.Time-last)>3&& GVector3.Distance(pos, target.View.Transform.Position) > 1f)
                {
                    per.CharacterMoveTo(root.Character, target.View.Transform.Position);
                    pos = target.View.Transform.Position;
                }
                if (!target.Enable)
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

