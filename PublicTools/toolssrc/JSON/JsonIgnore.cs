using System;

namespace org.vxwo.csharp.json
{
    /// <summary>
    /// Ignore
    /// </summary>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false)]
	
	public sealed class JsonIgnore: Attribute
	{
	}
}
