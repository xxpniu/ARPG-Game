using System;

namespace org.vxwo.csharp.json
{
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false)]
	
	public sealed class JsonName: Attribute
	{
		private string name;
		
		public JsonName (string name)
		{
			this.name = name;
		}
		
		public string GetName()
		{
			return name;
		}
	}
}

