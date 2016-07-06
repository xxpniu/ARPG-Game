using System;

namespace Layout.EditorAttributes
{
	public class EditorLayoutAttribute:Attribute
	{
		public EditorLayoutAttribute (string name)
		{
			Name = name;
		}
		public string Name{set;get;}
	}
}

