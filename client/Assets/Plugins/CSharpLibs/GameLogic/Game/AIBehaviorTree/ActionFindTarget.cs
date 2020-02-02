using System;
using System.Collections.Generic;
using BehaviorTree;
using EConfig;
using EngineCore;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using Layout.AITree;
using Layout.EditorAttributes;
using Proto;
using UMath;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeFindTarget))]
	public class ActionFindTarget:ActionComposite, ITreeNodeHandle
    {

        [Label("当前查找距离")]
        public float getDistanceValue;
        [Label("获得目标数")]
        public int getTargets;
        [Label("目标Index")]
        public long index;

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
            //state =LastStatus.HasValue? LastStatus.Value:RunStatus.Failure;
			var character = context.UserState as BattleCharacter;
			var per = character.Controllor.Perception as BattlePerception;
			var root = context as AITreeRoot;
			var list = new List<BattleCharacter>();
			var distance = node.Distance;
			if (!root.GetDistanceByValueType(node.valueOf, distance, out distance))
			{
                getDistanceValue = -1;
				yield return RunStatus.Failure;
                yield break;
			}
            getDistanceValue = distance;

            //是否保留之前目标
            if (!node.findNew)
            {
                var older = root[AITreeRoot.TRAGET_INDEX];
                if (older != null)
                {
                    if (per.State[(int)older] is BattleCharacter targetCharacter)
                    {
                        if (per.Distance(targetCharacter, root.Character) <= distance)
                        {
                            yield return RunStatus.Success;
                            yield break;
                        }
                    }
                }
            }
            //清除
            root[AITreeRoot.TRAGET_INDEX] = -1L;
            var type = node.teamType;
            //处理使用魔法目标
            if (node.useMagicConfig)
            {
                var magicID = root[AITreeRoot.SELECT_MAGIC_ID];
                if (magicID == null)
                {
                    yield return RunStatus.Failure;
                    yield break;
                }
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterMagicData>((int)magicID);
                if (data == null)
                {
                    yield return RunStatus.Failure;
                    yield break;
                }
                type = (TargetTeamType)data.ReleaseAITargetType;
            }

			per.State.Each<BattleCharacter>(t => 
            {
                //隐身的不进入目标查找
                if (t.Lock.IsLock(ActionLockType.Inhiden))
                    return false;

                switch (type)
				{
                    case TargetTeamType.Enemy:
						if (character.TeamIndex == t.TeamIndex)
							return false;
						break;
					case TargetTeamType.OwnTeam:
						if (character.TeamIndex != t.TeamIndex)
							return false;
						break;
                    case TargetTeamType.OwnTeamWithOutSelf:
                        if (character.Index == t.Index) return false;
                        if (character.TeamIndex != t.TeamIndex)
                            return false;
                        break;
                    case TargetTeamType.Own:
                        {
                            if (character.Index != t.Index) return false;
                        }
                        break;
                    case TargetTeamType.All: 
                        {
                            //all
                        }
                        break;
                    default:
                        return false;
				}


				if (per.Distance(t, root.Character) > distance) return false;
				switch (node.filter)
				{
					case TargetFilterType.Hurt:
                        if (t.HP == t.MaxHP) return false;
						break;
				}
				list.Add(t);
				return false;
			});

            //getTargets = list.Count;
			if (list.Count == 0)
			{
				yield return RunStatus.Failure;
				yield break;
			}


			BattleCharacter target =null;

			switch (node.selectType)
			{
				case TargetSelectType.Nearest:
					{
						target = list[0];
						var d = UVector3.Distance (target.View.Transform.position, character.View.Transform.position);
						foreach (var i in list)
						{
							var temp =UVector3.Distance(i.View.Transform.position, character.View.Transform.position);
							if (temp < d)
							{
								d = temp;
								target = i;
							}
						}
					}
					break;
				case TargetSelectType.Random:
					target = GRandomer.RandomList(list);
					break;
				case TargetSelectType.HPMax:
					{
						target = list[0];
						var d = target.HP;
						foreach (var i in list)
						{
							var temp = i.HP;
							if (temp > d)
							{
								d = temp;
								target = i;
							}
						}
					}
					break;
				case TargetSelectType.HPMin:
					{
						target = list[0];
						var d = target.HP;
						foreach (var i in list)
						{
							var temp = i.HP;
							if (temp < d)
							{
								d = temp;
								target = i;
							}
						}
					}
					break;
				case TargetSelectType.HPRateMax:
					{
						target = list[0];
                        var d = (float)target.HP/(float)target.MaxHP;
						foreach (var i in list)
						{
                            var temp = (float)i.HP / (float)i.MaxHP; ;
							if (temp > d)
							{
								d = temp;
								target = i;
							}
						}
					}
				break;
				case TargetSelectType.HPRateMin:
					{
						target = list[0];
                        var d = (float)target.HP / (float)target.MaxHP;
						foreach (var i in list)
						{
                            var temp = (float)i.HP / (float)i.MaxHP; 
							if (temp < d)
							{
								d = temp;
								target = i;
							}
						}
					}
					break;
			}

            index = target.Index;
			root[AITreeRoot.TRAGET_INDEX] = target.Index;


			yield return RunStatus.Success;
		}

		public void SetTreeNode(TreeNode node)
		{
			this.node  = node as TreeNodeFindTarget;
		}

		private TreeNodeFindTarget node;

	}
}

