using System;

namespace Layout.EditorAttributes
{

	public enum AllowChildType
	{
		Manay,
		None,
		One,
		Probability
	}

	public class EditorAITreeNodeAttribute : Attribute
	{

		public EditorAITreeNodeAttribute(string name,string shortName) 
			: this(name ,shortName, AllowChildType.Manay) { }

		public EditorAITreeNodeAttribute(string name, string sName, AllowChildType canAppendChild)
		{
			Name = name;
			ShorName = sName;
			CanAppendChild = canAppendChild;
		}

		public string Name { set; get; }

		public string ShorName { set; get; }

		public AllowChildType CanAppendChild { set; get; }
	}
}

