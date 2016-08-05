using System;
using System.Collections.Generic;
using BehaviorTree;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeReleaseMagic))]
	public class ActionReleaseMagic : ActionComposite, ITreeNodeHandle
	{
		public ActionReleaseMagic()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			var root = context as AITreeRoot;
			var index = root[AITreeRoot.TRAGET_INDEX];
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

			string key = Node.magicKey;
			switch (Node.valueOf)
			{
				case MagicValueOf.BlackBoard:
					{
						var id = root[AITreeRoot.SELECT_MAGIC_ID];
						if (id == null)
						{
							yield return RunStatus.Failure;
							yield break;
						}
						var magicData = ExcelToJSONConfigManager.Current
																.GetConfigByID<CharacterMagicData>((int)id);
						key = magicData.MagicKey;
						root.Character.AttachMagicHistory(magicData.ID, root.Time);
					}
					break;
				case MagicValueOf.MagicKey:
					{
						key = Node.magicKey;
					}
					break;
			}

			if (!root.Perception.View.ExistMagicKey(key))
			{
				yield return RunStatus.Failure;
				yield break;
			}

			var release = root.Perception.CreateReleaser(key, new ReleaseAtTarget(root.Character, target));
			
			yield return RunStatus.Success;
		}

		private TreeNodeReleaseMagic Node;

		public void SetTreeNode(TreeNode node)
		{
			Node = node as TreeNodeReleaseMagic;
			//magicKey = n.magicKey;
		}

	}
}

