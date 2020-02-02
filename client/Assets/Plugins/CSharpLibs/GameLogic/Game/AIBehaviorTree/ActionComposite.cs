using System;
using BehaviorTree;

namespace GameLogic.Game.AIBehaviorTree
{
	public abstract class ActionComposite:BehaviorTree.Composite
	{
		public ActionComposite()
		{
		}

		public override Composite FindGuid(string id)
		{
			if (this.Guid == id) return this;
			return null;
		}
	}
}

