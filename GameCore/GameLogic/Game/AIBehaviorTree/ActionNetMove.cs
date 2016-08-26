using System;
using System.Collections.Generic;
using BehaviorTree;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
    [TreeNodeParse(typeof(TreeNodeNetActionMove))]
    public class ActionNetMove:ActionComposite,ITreeNodeHandle
    {
        public ActionNetMove()
        {
        }

        public override IEnumerable<RunStatus> Execute(ITreeRoot context)
        {
            var root = context as AITreeRoot;
            var message= root[AITreeRoot.ACTION_MESSAGE] as Proto.Action_ClickMapGround;
            if (message == null)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            root[AITreeRoot.ACTION_MESSAGE] = null;
            var target = new EngineCore.GVector3(message.TargetPosition.x,
                                                 message.TargetPosition.y, 
                                                 message.TargetPosition.z);
            var path= root.Character.View.MoveTo(target);
            if (path == null || path.Count == 0)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            while (root.Perception.View.Distance(root.Character.View.Transform.Position, target) > 1)
            {
                yield return RunStatus.Running;
            }

            root.Character.View.StopMove();
            yield return RunStatus.Success;
        }

        public void SetTreeNode(TreeNode node)
        {
            
        }

        public override void Stop(ITreeRoot context)
        {
            base.Stop(context);
            if (LastStatus == RunStatus.Running)
            {
                var root = context as AITreeRoot;
                root.Character.View.StopMove();
            }
        }
    }
}

