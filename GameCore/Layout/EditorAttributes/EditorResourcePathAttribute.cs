using System;

namespace Layout.EditorAttributes
{
	//用来显示选择资源编辑
	public class EditorResourcePathAttribute:Attribute
	{

		public EditorResourcePathAttribute():this("Prefab")
		{}

		public EditorResourcePathAttribute (string exName)
		{
			ExName = exName;
		}

		public string ExName{ set; get;}
	}
}

