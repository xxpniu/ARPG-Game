using System;

namespace org.vxwo.csharp.json
{
    public class JsonWriter
    {
        public static string Write(JsonValue obj)
        {
            return new JsonSerializer(true, true).ConvertToJSON(obj);
        }
		
		public static string WriteShrink(JsonValue obj)
        {
            return new JsonSerializer(false, false).ConvertToJSON(obj);
        }
		
        public static object Write(Type type, JsonValue obj)
        {
            return new JsonObjSerializer().ConvertToObject(type, obj);
        }
		
        public static T Write<T>(JsonValue obj)
        {
            return (T)Write(typeof(T), obj);
        }
    }
}
