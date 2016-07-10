using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeReleaseMagic))]
	public class ActionReleaseMagic:ActionComposite,ITreeNodeHandle
	{
		public ActionReleaseMagic()
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

			var release = root.Perception.CreateReleaser(magicKey, new ReleaseAtTarget(root.Character, target));
			root.Perception.State.AddElement(release);
			yield return RunStatus.Success;
		}

		public void SetTreeNode(TreeNode node)
		{
			var n = node as TreeNodeReleaseMagic;
			magicKey = n.magicKey;
		}

		public string magicKey;
	}
}

