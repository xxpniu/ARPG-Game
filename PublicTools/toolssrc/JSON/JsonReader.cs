
namespace org.vxwo.csharp.json
{
    /// <summary>
    /// 
    /// </summary>
	public class JsonReader
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
		public static JsonValue Read (string json)
		{
			return new JsonParser (json).Decode ();
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public static JsonValue Read (object obj)
		{
			return new JsonObjParser (obj).Decode ();
		}
	}
}
