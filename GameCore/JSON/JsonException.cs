using System;

namespace org.vxwo.csharp.json
{
	public class JsonException: Exception
	{
		public JsonException (string message)
			: base(message)
		{			
		}
	}
}

