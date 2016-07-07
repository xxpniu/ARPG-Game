
namespace org.vxwo.csharp.json
{
	public class JsonReader
	{
		public static JsonValue Read (string json)
		{
			return new JsonParser (json).Decode ();
		}
		
		public static JsonValue Read (object obj)
		{
			return new JsonObjParser (obj).Decode ();
		}
	}
}
