using System;

namespace org.vxwo.csharp.json
{
    /// <summary>
    /// Json Tools
    /// </summary>
	public class JsonTool
	{
		private JsonTool ()
		{
		}

        /// <summary>
        /// Deserialize json to type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
		{
			return JsonWriter.Write<T>(JsonReader.Read(json));
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
		public static object Deserialize(Type type, string json)
		{
			return JsonWriter.Write(type, JsonReader.Read(json));
		}
		
        /// <summary>
        /// Serialize object to json 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public static string Serialize(object obj)
		{
			return JsonWriter.Write(JsonReader.Read(obj));
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public static string SerializeShrink(object obj)
		{
			return JsonWriter.WriteShrink(JsonReader.Read(obj));
		}		
	}
}

