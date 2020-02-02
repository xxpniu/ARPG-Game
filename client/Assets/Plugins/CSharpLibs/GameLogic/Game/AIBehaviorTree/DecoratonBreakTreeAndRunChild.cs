using System;
using System.Collections.Generic;
using BehaviorTree;

namespace GameLogic.Game.AIBehaviorTree
{
	public class DecoratonBreakTreeAndRunChild:BehaviorTree.Decorator
	{
		public DecoratonBreakTreeAndRunChild(Composite comp):base(comp)
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			if (DecoratedChild == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			context.Chanage(this.DecoratedChild);

			yield return RunStatus.Success;
		}
	}
}

