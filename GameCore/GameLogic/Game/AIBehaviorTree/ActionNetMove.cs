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
            root.Character.View.MoveTo(target);

            while (root.Character.View.IsMoving)
            {
                yield return RunStatus.Running;
            }
            var time = root.Time;
            if (time + 0.2f < root.Time)
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

