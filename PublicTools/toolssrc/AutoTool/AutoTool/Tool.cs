using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool
{
    public class Tool
    {
        public static string ToJson<T>(T obj)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            using (var mem = new MemoryStream())
            {
                ser.WriteObject(mem, obj);
                return Encoding.UTF8.GetString(mem.ToArray());
            }
        }

        public static T ToObject<T>(string json)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            using (var mem = new   MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)ser.ReadObject(mem);
            }
        }


 
    }
}