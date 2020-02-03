using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

    // <summary>
    /// 游戏中序列化和反序列化xml的类
    /// author:xxp
    /// date:2013/04/03
    /// </summary>
    public sealed class XmlParser
    {
        /// <summary>
        /// 序列化一个对象，返回对象被序列化后的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            var xml = new XmlSerializer(typeof(T));

            var mem = new StringBuilder();
            {
                using (var sw = new System.IO.StringWriter(mem))
                {
                    xml.Serialize(sw, obj);
                }
                return mem.ToString();
            }
        }

        /// <summary>
        /// 反序列化一个xml文本 成一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string xml)
        {
            var xmler = new XmlSerializer(typeof(T));

            using (var tr = new System.IO.StringReader(xml))
            {
                return (T)(xmler.Deserialize(tr));
            }
        }

        /// <summary>
        /// UTF No BOM
        /// </summary>
        public static Encoding UTF8 = new UTF8Encoding(false);
    }

