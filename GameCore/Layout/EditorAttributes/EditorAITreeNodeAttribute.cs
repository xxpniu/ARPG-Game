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

		public EditorAITreeNodeAttribute(string name,string shortName ,string flag) 
			: this(name ,shortName, flag, AllowChildType.Manay) { }



		public EditorAITreeNodeAttribute(string name, string sName, string flag, AllowChildType canAppendChild)
		{
			Name = name;
			ShorName = sName;
			Flag = flag;
			CanAppendChild = canAppendChild;
		}

		public string Name { set; get; }
		public string Flag { set; get; }

		public string ShorName { set; get; }

		public AllowChildType CanAppendChild { set; get; }
	}
}

