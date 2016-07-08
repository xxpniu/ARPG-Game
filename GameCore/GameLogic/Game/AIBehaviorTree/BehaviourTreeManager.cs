using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorTree;


namespace Game.AIBehaviorTree
{
    /// <summary>
    /// 解析树
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BehaviourNodeParser : Attribute
    {
        public Type NodeType { set; get; }
        /// <summary>
        /// 游戏树的解析
        /// </summary>
        /// <param name="type"></param>
        public BehaviourNodeParser(Type type)
        {
            NodeType = type;
        }
    }
    /// <summary>
    /// Node 
    /// </summary>
    public interface INodeParser
    {
        void Parser(TreeNode node);
    }

    /// <summary>
    /// 游戏树的解析
    /// </summary>
    public class TreeParser
    {
        public static BehaviorTree.Composite ParserComposite(TreeNode tree, BehaviorTree.Composite parent)
        {
            Composite current = null;


            //Debug.Log(string.Format("NODE:{0} TYPE:{1}",tree.Name,tree.GetType()));

            #region decoratorNode
            if (tree is DecoratorNode)
            {
                var child = ParserComposite(tree.Children[0], null);
                current = new Decorator(child);
            }

            if (tree is DecoratorNegationNode)
            { 
                var child = ParserComposite(tree.Children[0], null);
                current = new DecoratorNegation(child);
            }

            if (tree is DecoratorRunUnitlFailureNode)
            { 
                var child = ParserComposite(tree.Children[0], null);
                current = new DecoratorRunUnitlFailure(child);
            }

            if (tree is DecoratorRunUntilSuccessNode)
            {
                var child = ParserComposite(tree.Children[0], null);
                current = new DecoratorRunUntilSuccess(child);
            }

            if (tree is DecoratorTickNode)
            {
                var child = ParserComposite(tree.Children[0], null);
                current = new DecoratorTick(child)
                {
                    TickTime = (tree as DecoratorTickNode).TickTime
                };
            }

            if (tree is DecoratorTickUntilSuccessNode)
            { 
                var child = ParserComposite(tree.Children[0], null);
                current = new DecoratorTickUntilSuccess(child)
                {
                    TickTime = (tree as DecoratorTickUntilSuccessNode).TickTime/1000f
                };
            }

            if (tree is WaitForSecondsNode)
            {
                current = new BWaitForSeconds() {
                    Seconds = (tree as WaitForSecondsNode).Milseconds/1000f
                };
            }
            #endregion

            #region Liner
            if (tree is LinerNode)
            {
                List<Composite> childs = new List<Composite>();
                foreach (var i in tree.Children)
                {
                    childs.Add(ParserComposite(i, null));
                }

                if (tree is ParallelPrioritySelectorNode)
                {
                    current = new ParallelPrioritySelector(childs.ToArray());
                }
                else if (tree is ParallelSequenceNode)
                {
                    current = new ParallelSequence(childs.ToArray());
                }
                else if (tree is SequenceNode)
                {
                    current = new Sequence(childs.ToArray());
                }
                else if (tree is PrioritySelectorNode)
                {
                    current = new PrioritySelector(childs.ToArray());
                }
            }
            #endregion

            #region ProbabilitySelectorNode
            if (tree is ProbabilitySelectorNode)
            {
                List<ProbabilitySelection> randomChilds = new List<ProbabilitySelection>();
                foreach (var i in tree.Children)
                {
                    var item = i as ProbabilitySelectorItemNode;
                    if (item == null)
                        throw new Exception("ProbabilitySelectorNode's child is not ProbabilitySelectorItemNode");
                    var pChild = ParserComposite(item.Children[0], null);
                    randomChilds.Add(new ProbabilitySelection(pChild, (int)item.Probability));
                }
                current = new ProbabilitySelector(randomChilds.ToArray());
            }
            if (tree is ProbabilitySelectorItemNode)
            {
                throw new Exception("ProbabilitySelectorItemNode can't be parsed with out parent!");
            }
            #endregion

            #region Action 
            Type type;

            if (tree is ActionNode)
            {
                if (_ALL_PARSER.TryGetValue(tree.GetType(), out type))
                {
                    var item = System.Activator.CreateInstance(type);
                    current = (Composite)item;
                    if (current != null)
                    {
                        (current as INodeParser).Parser(tree);
                    }
                    else {
                        Debug.LogError(string.Format("ITEM:{0} current:{1} TYPE:{2}",item, tree ,type));
                    }
                }
                else
                {
                    Debug.Log(string.Format("Action {0} No Paser!!!", tree.GetType()));
                }
            }
            #endregion

            if (current != null)
            {
                current.Name = tree.Name;
                //Debug.Log(tree.Name);
            }
            else
            {
                Debug.Log(string.Format("Action {0} No Paser!!!", tree.GetType()));
            }
            //下面解析动作，条件
            if (parent == null)
                parent = current;
            return parent;
        }

        static TreeParser()
        {
            var assembly = typeof(TreeParser).Assembly;
            var types = assembly.GetTypes();
            foreach (var i in types)
            {
                var cs = i.GetConstructors();
                if (cs != null && cs.Length > 0)
                {
                    var t = i.GetCustomAttributes(typeof(BehaviourNodeParser), false);
                    if (t.Length == 0) continue;
                    var p = t[0] as BehaviourNodeParser;
                    _ALL_PARSER.Add(p.NodeType, i);
                    //Debug.Log(string.Format("PARSER TYPE:{0} RUNTYPE:{1}", p.NodeType, i));
                }
            }
        }

        private static Dictionary<Type, Type> _ALL_PARSER = new Dictionary<Type, Type>();
    }
}
