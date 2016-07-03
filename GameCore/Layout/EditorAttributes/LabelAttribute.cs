using System;

namespace Layout.EditorAttributes
{
	//用来显示编辑的名字
	public class LabelAttribute:Attribute
	{
		public LabelAttribute (string displayName,string des)
		{
			DisplayName = displayName;
			Description = des;
		}

		public LabelAttribute(string displayName):this(displayName,string.Empty){}

		public string Description{ set; get; }

		public string DisplayName{ set; get; }
	}
}

