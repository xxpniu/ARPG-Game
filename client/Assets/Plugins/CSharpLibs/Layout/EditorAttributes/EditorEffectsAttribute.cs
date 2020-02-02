using System;

namespace Layout.EditorAttributes
{
	
	public class EditorEffectAttribute :Attribute
	{
		public string Name { get; private set; }
		public EditorEffectAttribute (string name)
		{
			Name = name;
		}
	}


	public class EditorEffectsAttribute : Attribute
	{ }
}

