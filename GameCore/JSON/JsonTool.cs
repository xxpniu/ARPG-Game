using System;

namespace org.vxwo.csharp.json
{
	public class JsonTool
	{
		private JsonTool ()
		{
		}
		
		public static T Deserialize<T>(string json)
		{
			return JsonWriter.Write<T>(JsonReader.Read(json));
		}
		
		public static object Deserialize(Type type, string json)
		{
			return JsonWriter.Write(type, JsonReader.Read(json));
		}
		
		public static string Serialize(object obj)
		{
			return JsonWriter.Write(JsonReader.Read(obj));
		}
		
		public static string SerializeShrink(object obj)
		{
			return JsonWriter.WriteShrink(JsonReader.Read(obj));
		}		
	}
}

