using System;
using Layout.EditorAttributes;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Layout.AITree
{
	[
		XmlInclude(typeof(TreeNodeSelector)),
		XmlInclude(typeof(TreeNodeSequence)),
		XmlInclude(typeof(TreeNodeParallelSelector)),
		XmlInclude(typeof(TreeNodeParallelSequence)),
		XmlInclude(typeof(TreeNodeProbabilitySelector)),
		XmlInclude(typeof(TreeNodeProbabilityNode)),
		/*基础结束*/
		XmlInclude(typeof(TreeNodeNegation)),
		XmlInclude(typeof(TreeNodeRunUnitlFailure)),
		XmlInclude(typeof(TreeNodeRunUnitlSuccess)),
		XmlInclude(typeof(TreeNodeTick)),
		XmlInclude(typeof(TreeNodeTickUntilSuccess)),
		/*装饰节点结束*/
		XmlInclude(typeof(TreeNodeFindTarget)),
		XmlInclude(typeof(TreeNodeWaitForSeconds)),
		XmlInclude(typeof(TreeNodeReleaseMagic)),
		XmlInclude(typeof(TreeNodeDistancTarget)),
		XmlInclude(typeof(TreeNodeMoveToTarget)),
		XmlInclude(typeof(TreeNodeCompareTargets)),
		XmlInclude(typeof(TreeNodeFarFromTarget)),
		XmlInclude(typeof(TreeNodeSelectCanReleaseMagic)),
		XmlInclude(typeof(TreeNodeMoveCloseEnemyCamp)),

        XmlInclude(typeof(TreeNodeNetActionSkill)),
        XmlInclude(typeof(TreeNodeNetActionMove))

	]
	public class TreeNode
	{
		public struct TreeNodeRelation
		{
			public TreeNode Parant;
			public TreeNode Node;
		}

		public TreeNode()
		{
			childs = new List<TreeNode>();
		}

		public List<TreeNode> childs;

		[Label("名称")]
		public string name;

		[HideInEditor]
		public string guid;

		public bool FindChildByGuid(string guid, out TreeNode child)
		{
			child = null;
			foreach (var i in childs)
			{
				if (i.guid == guid)
				{
					child = i;
					return true;
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			return guid.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var node = obj as TreeNode;
			if (node == null) return false;
			if (guid == node.guid) return true;
			return false;
		}

		public static T CreateInstance<T>() where T : TreeNode, new()
		{
			var node = new T();
			node.guid = Guid.NewGuid().ToString();
			var attrs = typeof(T).GetCustomAttributes(typeof(EditorAITreeNodeAttribute), false) as EditorAITreeNodeAttribute[];
			if (attrs.Length > 0)
			{
				node.name = attrs[0].Name;
			}
			return node;
		}

		public static TreeNode CreateInstance(Type t)
		{
			if (!t.IsSubclassOf(typeof(TreeNode)))
			{
				throw new Exception("[" + t.ToString() + "] is not subclassof TreeNode");
			}

			var treeNode = Activator.CreateInstance(t) as TreeNode;
			treeNode.guid = Guid.NewGuid().ToString();
			var attrs = t.GetCustomAttributes(typeof(EditorAITreeNodeAttribute), false) as EditorAITreeNodeAttribute[];
			if (attrs.Length > 0)
			{
				treeNode.name = attrs[0].Name;
			}
			return treeNode;
		}

		public static TreeNodeRelation? FindNodeByGuid(TreeNode root, string guid, TreeNode parent = null)
		{
			if (root.guid == guid)
			{
				return new TreeNodeRelation { Parant = parent, Node = root };
			}

			foreach (var i in root.childs)
			{
				var result = FindNodeByGuid(i, guid, root);
				if (result.HasValue)
				{
					return result.Value;
				}
			}

			return null;
		}

        public void NewGuid()
        {
            foreach (var i in childs)
            {
                i.NewGuid();
            }
            this.guid = Guid.NewGuid().ToString();
        }

		public  override string ToString()
		{
			var attrs = GetType().GetCustomAttributes(typeof(EditorAITreeNodeAttribute), false) as EditorAITreeNodeAttribute[];
			if (attrs.Length > 0)
			{
				return attrs[0].Name;
			}
			return GetType().Name;
		}

	}
}

