using System;

namespace Layout.EditorAttributes
{
	//用来给获取时间轴编辑
	public class EditorLineAttribute:Attribute
	{
		public EditorLineAttribute (string name)
		{
			Name = name;
		}

		public string Name{ set; get; }
	}
}

