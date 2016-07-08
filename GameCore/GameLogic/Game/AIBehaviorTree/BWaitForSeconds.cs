using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorTree;

namespace Game.AIBehaviorTree
{
    public class BWaitForSeconds : BehaviorTree.Composite
    {
        public override IEnumerable<BehaviorTree.RunStatus> Execute(ITreeRoot context)
        {
			float time = context.Time;
			//var lastTime = time;
			while (time + Seconds >= context.Time)
                yield return BehaviorTree.RunStatus.Running;
            yield return BehaviorTree.RunStatus.Success;
        }

        public float Seconds { set; get; }
    }
}
