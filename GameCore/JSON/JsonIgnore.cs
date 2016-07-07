using System;

namespace org.vxwo.csharp.json
{
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false)]
	
	public sealed class JsonIgnore: Attribute
	{
		public JsonIgnore ()
		{
		}
	}
}
