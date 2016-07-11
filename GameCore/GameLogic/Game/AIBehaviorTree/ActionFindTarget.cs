using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeFindTarget))]
	public class ActionFindTarget:ActionComposite, ITreeNodeHandle
	{
		public ActionFindTarget()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			var character = context.UserState as BattleCharacter;
			var per = character.Controllor.Perception as BattlePerception;
			var root = context as AITreeRoot;
			var list = new List<BattleCharacter>();
			var distance = node.Distance;
			if (!root.GetDistanceByValueType(node.valueOf, distance, out distance))
			{
				yield return RunStatus.Failure;
			}
			per.State.Each<BattleCharacter>(t => {
				switch (node.teamType)
				{
					case TargetTeamType.Enemy:
						if (character.TeamIndex == t.TeamIndex)
							return false;
						break;
					case TargetTeamType.OwnTeam:
						if (character.TeamIndex == t.TeamIndex)
							return false;
						break;
				}
				if (per.Distance(t, root.Character) <= distance)
					list.Add(t);
				return false;
			});

			if (list.Count == 0)
			{
				yield return RunStatus.Failure;
				yield break;
			}


			BattleCharacter target =null;

			switch (node.filter)
			{
				case TargetFilterType.Nearest:
					{
						target = list[0];
						var d = per.View.Distance(target.View.Transform.Position, character.View.Transform.Position);
						foreach (var i in list)
						{
							var temp =per.View.Distance(i.View.Transform.Position, character.View.Transform.Position);
							if (temp < d)
							{
								d = temp;
								target = i;
							}
						}
					}
					break;
				case TargetFilterType.Random:
					target = GRandomer.RandomList(list);
					break;
			}

			root[AITreeRoot.TRAGET_INDEX] = target.Index;


			yield return RunStatus.Success;
		}

		public void SetTreeNode(TreeNode node)
		{
			this.node  = node as TreeNodeFindTarget;
		}

		TreeNodeFindTarget node;

	}
}

