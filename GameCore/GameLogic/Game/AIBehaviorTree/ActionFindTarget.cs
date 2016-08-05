using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using Layout.AITree;
using Layout.EditorAttributes;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeFindTarget))]
	public class ActionFindTarget:ActionComposite, ITreeNodeHandle
	{
		public ActionFindTarget()
		{
            
		}

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
                    var targetCharacter = per.State[(long)older] as BattleCharacter;
                    if (targetCharacter != null)
                    {
                        if (per.Distance(targetCharacter, root.Character)<=distance)
                        {
                            yield return RunStatus.Success;
                            yield break;
                        }
                    }
                }
            }

			per.State.Each<BattleCharacter>(t => 
            {
				switch (node.teamType)
				{
					case TargetTeamType.Enemy:
						if (character.TeamIndex == t.TeamIndex)
							return false;
						break;
					case TargetTeamType.OwnTeam:
						if (character.TeamIndex != t.TeamIndex)
							return false;
						break;
				}

                switch (node.bodyType)
                {
                    case TargetBodyType.NoBuilding:
                        if (character.TBody == Proto.BodyType.Building) return false;
                        break;
                }

				if (per.Distance(t, root.Character) > distance) return false;
				switch (node.filter)
				{
					case TargetFilterType.Hurt:
						if (t.HP == t.HPMax.FinalValue) return false;
						break;
				}
				list.Add(t);
				return false;
			});

            getTargets = list.Count;
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
						var d = (float)target.HP/(float)target.HPMax.FinalValue;
						foreach (var i in list)
						{
							var temp = (float)i.HP / (float)i.HPMax.FinalValue; ;
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
						var d = (float)target.HP / (float)target.HPMax.FinalValue;
						foreach (var i in list)
						{
							var temp = (float)i.HP / (float)i.HPMax.FinalValue; ;
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

