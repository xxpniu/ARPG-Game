using System;

namespace org.vxwo.csharp.json
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonWriter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Write(JsonValue obj)
        {
            return new JsonSerializer(true, true).ConvertToJSON(obj);
        }
		/// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public static string WriteShrink(JsonValue obj)
        {
            return new JsonSerializer(false, false).ConvertToJSON(obj);
        }
		/// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Write(Type type, JsonValue obj)
        {
            return new JsonObjSerializer().ConvertToObject(type, obj);
        }
		/// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Write<T>(JsonValue obj)
        {
            return (T)Write(typeof(T), obj);
        }
    }
}
