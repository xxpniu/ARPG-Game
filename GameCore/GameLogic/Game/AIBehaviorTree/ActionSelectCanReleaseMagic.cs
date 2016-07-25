using System;
using System.Collections.Generic;
using BehaviorTree;
using ExcelConfig;
using Layout.AITree;
using Layout.EditorAttributes;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeSelectCanReleaseMagic))]
	public class ActionSelectCanReleaseMagic:ActionComposite,ITreeNodeHandle
	{
		private HashSet<int> releaseHistorys = new HashSet<int>();

        [Label("当前魔法")]
        public string key;

        public override IEnumerable<RunStatus> Execute(ITreeRoot context)
        {
            var root = context as AITreeRoot;
            key = string.Empty;
            var magics = ExcelConfig.ExcelToJSONConfigManager.Current
                                    .GetConfigs<ExcelConfig.CharacterMagicData>(t => t.CharacterID == root.Character.ConfigID);
            if (magics == null || magics.Length == 0)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            var list = new List<CharacterMagicData>();
            foreach (var i in magics)
            {
                if (root.Character.IsCoolDown(i.ID, root.Time, false))
                {
                    list.Add(i);
                }
            }

            if (list.Count == 0)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            int result = -1;
            switch (Node.resultType)
            {
                case MagicResultType.Random:
                    result = GRandomer.RandomList(list).ID;
                    break;
                case MagicResultType.Frist:
                    result = list[0].ID;
                    break;
                case MagicResultType.Sequence:
                    foreach (var i in magics)
                    {
                        if (releaseHistorys.Contains(i.ID)) continue;
                        result = i.ID;
                    }
                    if (result == -1)
					{
						releaseHistorys.Clear();
						result = list[0].ID;
					}
					releaseHistorys.Add(result);
					break;
			}
			if (result == -1)
			{
				yield return RunStatus.Failure;
				yield break;
			}
			root[AITreeRoot.SELECT_MAGIC_ID] = result;
            var config = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterMagicData>(result);
            if (config != null)
                key = config.MagicKey;
			yield return RunStatus.Success;
        }


		private TreeNodeSelectCanReleaseMagic Node;

		public void SetTreeNode(TreeNode node)
		{
			Node = node as TreeNodeSelectCanReleaseMagic;
		}
	}
}

