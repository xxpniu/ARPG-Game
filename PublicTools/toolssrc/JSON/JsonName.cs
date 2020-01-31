using System;

namespace org.vxwo.csharp.json
{
    /// <summary>
    /// json name 
    /// </summary>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false)]
	
	public sealed class JsonName: Attribute
	{
		private string name;
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
		public JsonName (string name)
		{
			this.name = name;
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public string GetName()
		{
			return name;
		}
	}
}

