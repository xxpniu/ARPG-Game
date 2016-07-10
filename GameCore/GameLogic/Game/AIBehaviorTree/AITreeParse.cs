using System;
using System.Collections.Generic;
using BehaviorTree;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{

	public interface ITreeNodeHandle
	{
		void SetTreeNode(TreeNode node);
	}

	public class TreeNodeParseAttribute : Attribute
	{
		public TreeNodeParseAttribute(Type paserType)
		{
			ParserType = paserType;
		}

		public Type ParserType { set; get; }
	}

	public class AITreeParse
	{
		private static Dictionary<Type, Type> _handler = new Dictionary<Type, Type>();
		static AITreeParse()
		{
			_handler.Clear();
			var types = typeof(AITreeRoot).Assembly.GetTypes();
			foreach (var i in types)
			{
				if (i.IsSubclassOf(typeof(Composite)))
				{
					var attrs = i.GetCustomAttributes(typeof(TreeNodeParseAttribute), false) as TreeNodeParseAttribute[];
					if (attrs.Length == 0) continue;
					_handler.Add(attrs[0].ParserType, i);
				}
			}
		}

		public static Composite CreateFrom(TreeNode node)
		{
		    if (node is TreeNodeProbabilitySelector)
			{
				var sels = new List<ProbabilitySelection>();
				foreach (var i in node.childs)
				{
					var n = i as TreeNodeProbabilityNode;
					var comp = CreateFrom(i.childs[0]);
					var ps = new ProbabilitySelection(comp, n.probability);
					sels.Add(ps);
				}
				return new ProbabilitySelector(sels.ToArray()) {Guid = node.guid };
			}

			var list = new List<Composite>();
			foreach (var i in node.childs)
			{
				list.Add(CreateFrom(i));
			}

			if (node is TreeNodeSequence)
			{
				return new Sequence(list.ToArray()) { Guid = node.guid };
			}
			else if (node is TreeNodeSelector)
			{
				return new PrioritySelector(list.ToArray()) { Guid = node.guid };
			}
			else if (node is TreeNodeParallelSelector)
			{
				return new ParallelPrioritySelector(list.ToArray()) { Guid = node.guid };
			}
			else if (node is TreeNodeParallelSequence)
			{
				return new ParallelSequence(list.ToArray()) { Guid = node.guid };
			}
			else if (node is TreeNodeTick)
			{
				var n = node as TreeNodeTick;
				return new DecoratorTick(list[0]) { TickTime = n.tickTime, Guid = node.guid };
			}
			else if (node is TreeNodeNegation)
			{
				return new DecoratorNegation(list[0]) { Guid = node.guid };
			}
			else if (node is TreeNodeRunUnitlSuccess)
			{
				return new DecoratorRunUntilSuccess(list[0]) { Guid = node.guid };
			}
			else if (node is TreeNodeRunUnitlFailure)
			{
				return new DecoratorRunUnitlFailure(list[0]) { Guid = node.guid };
			}
			else if (node is TreeNodeTickUntilSuccess)
			{
				return new DecoratorTickUntilSuccess(list[0]) { Guid = node.guid };
			}
			else if (node is TreeNodeBreakTreeAndRunChild)
			{
				return new DecoratonBreakTreeAndRunChild(list[0]) { Guid = node.guid };
			}
			else {
				return Parse(node);
			}
		}

		private static Composite Parse(TreeNode node)
		{
			Type handle;
			if (_handler.TryGetValue(node.GetType(), out handle))
			{
				var comp = Activator.CreateInstance(handle) as ITreeNodeHandle;
				comp.SetTreeNode(node);

				var t = comp as Composite;
				t.Guid = node.guid;
				return t;
			}

			throw new Exception("Not parsed TreeNode:" + node.GetType());
			//return null;
		}

	}
}

