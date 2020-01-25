using System;

namespace org.vxwo.csharp.json
{
    /// <summary>
    /// 
    /// </summary>
	public class JsonException: Exception
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
		public JsonException (string message)
			: base(message)
		{			
		}
	}
}

