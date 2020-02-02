using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using Layout.AITree;
using Proto;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeCompareTargets))]
	public class ActionCompareTargets:ActionComposite,ITreeNodeHandle
	{
		public ActionCompareTargets()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{

			var root = context as AITreeRoot;
			var per = root.Perception;
			var targets = new List<BattleCharacter>();
			float distance = Node.Distance;
			if (!root.GetDistanceByValueType(Node.valueOf, distance, out distance))
			{
				yield return RunStatus.Failure;
				yield break;
			}
			per.State.Each<BattleCharacter>(t =>
			{
				switch (Node.teamType)
				{
					case TargetTeamType.Enemy:
						if (t.TeamIndex == root.Character.TeamIndex) return false;
						break;
					case TargetTeamType.OwnTeam:
						if (t.TeamIndex != root.Character.TeamIndex) return false;
						break;
				}
				if (per.Distance(t, root.Character) <= distance)
					targets.Add(t);
				return false;
			});

			int count = Node.compareValue;
			switch (Node.compareType)
			{
				case CompareType.Greater:
					if (count < targets.Count)
						yield return RunStatus.Success;
					else
						yield return RunStatus.Failure;
					break;
				case CompareType.Less:
					if (count > targets.Count)
						yield return RunStatus.Success;
					else
						yield return RunStatus.Failure;
					break;
			}
			yield break;

		}
		private TreeNodeCompareTargets Node;
		public void SetTreeNode(TreeNode node)
		{
			Node = node as TreeNodeCompareTargets;
		}
	}
}

