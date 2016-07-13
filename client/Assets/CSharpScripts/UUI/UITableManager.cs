using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UGameTools;


/// <summary>
/// 提供给表格操作时 对表格复制行的抽象定义
/// </summary>
public class UITableItem
{
	public Transform Root { private set; get; }
	public virtual void OnCreate(Transform root)
	{
		Root = root;
	}
	public T FindChild<T>(string name) where T : Component
	{
		return this.Root.FindChild<T>(name);
	}
}

/// <summary>
/// 一个简单的表格管理，支持遍历和随意修改表格的行数
/// </summary>
/// <typeparam name="T"></typeparam>
public class UITableManager<T> : IEnumerable<T> where T : UITableItem, new()
{
	public UITableManager()
	{
		Cached = false;
		AutoLayout = true;
	}
	private int _count = 0;

	public bool Cached { set; get; }
	public int Count
	{
		get { return _count; }
		set
		{
			if (templet == null)
				throw new Exception("Try to manage a not init table.");
			for (var i = 0; i < _items.Count; i++)
			{
				if (_items[i].Root.gameObject.activeSelf) continue;
				_items[i].Root.ActiveSelfObject(true);
			}
			if (_items.Count != value)
			{

				if (_items.Count > value)
				{
					#region add
					Queue<T> dels = new Queue<T>();
					for (var i = value; i < _items.Count; i++)
					{
						if (Cached)
						{
							if (_items[i].Root.gameObject.activeSelf)
							{
								_items[i].Root.gameObject.SetActive(false);
							}
						}
						else
						{
							dels.Enqueue(_items[i]);
						}
					}
					while (dels.Count > 0)
					{
						var item = dels.Dequeue();
						GameObject.Destroy(item.Root.gameObject);
						_items.Remove(item);
					}
					#endregion
				}
				else
				{
					var count = _items.Count;
					for (var i = count; i < value; i++)
					{
						var item = new T();
						var obj = GameObject.Instantiate(templet.gameObject) as GameObject;
						obj.name = string.Format("{0}_{1:0000}", templet.gameObject.name, i);
						obj.transform.parent = templet.parent;
						obj.transform.localScale = Vector3.one;
						obj.SetActive(true);
						item.OnCreate(obj.transform);
						_items.Add(item);
					}
				}

			}
			_count = value;
			if (AutoLayout)
				RepositionLayout();
		}
	}

	public bool AutoLayout { set; get; }
	public void RepositionLayout()
	{
		
	}

	private List<T> _items = new List<T>();

	private Transform templet;
	public void Init(Transform root)
	{
		if (root.childCount > 0)
			templet = root.GetChild(0);
		else
			throw new Exception("Can't init table from a empty root!");
		templet.ActiveSelfObject(false);
	}

	public T this[int index]
	{
		get
		{
			if (index >= 0 && index < Count)
				return _items[index];
			else return null;
		}
	}
	public IEnumerator<T> GetEnumerator()
	{
		return _items.Take(_count).GetEnumerator();
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this._items.Take(_count).GetEnumerator();
	}

	private GridLayoutGroup currentGrid;
	private VerticalLayoutGroup currentTable;
	internal void InitFromGridLayoutGroup(GridLayoutGroup grid)
	{
		currentGrid = grid;
		Init(grid.transform);
	}

	public void InitFromVerticalLayoutGroup(VerticalLayoutGroup grid)
	{
		currentTable = grid;
		Init(grid.transform);
	}
}

/// <summary>
/// 支持逻辑分离中的模板的抽象
/// </summary>
public abstract class TableItemTemplate
{
	public virtual void Init(UITableItem item)
	{
		Item = item;
		InitTemplate();
	}
	private UITableItem Item { set; get; }
	public T FindChild<T>(string name) where T : Component
	{
		return this.Item.FindChild<T>(name);
	}

	public abstract void InitTemplate();
}

/// <summary>
///支持逻辑分离中的逻辑抽象
/// </summary>
/// <typeparam name="UITemplate"></typeparam>
public abstract class TableItemModel<UITemplate> where UITemplate : TableItemTemplate, new()
{
	public virtual void Init(UITemplate template, UITableItem item)
	{
		Template = template;
		Item = item;
		InitModel();
	}
	public UITemplate Template { private set; get; }
	public UITableItem Item { private set; get; }
	public abstract void InitModel();
}

/// <summary>
/// 自动生成代码中的表格管理
/// </summary>
/// <typeparam name="UITemplate"></typeparam>
/// <typeparam name="UIModel"></typeparam>
public class AutoGenTableItem<UITemplate, UIModel> : UITableItem
	where UITemplate : TableItemTemplate, new()
	where UIModel : TableItemModel<UITemplate>, new()
{
	public AutoGenTableItem()
	{ }

	public UITemplate Template { private set; get; }
	public UIModel Model { private set; get; }

	public override void OnCreate(Transform root)
	{
		base.OnCreate(root);
		Template = new UITemplate();
		Template.Init(this);
		Model = new UIModel();
		Model.Init(Template, this);
	}
}

