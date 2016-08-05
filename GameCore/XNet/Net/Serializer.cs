using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.Utility
{
    /// <summary>
    /// 网络序列化
    /// </summary>
    public class NetSerializer
    {
        /// <summary>
        /// 把一个对象序列化成二进制
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            if (Serializer == null)
                throw new NotImplementedException();
            return Serializer.Serialize(obj);
        }

        /// <summary>
        /// 把一个数据流反射为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(byte[] data)
        {
            if (Serializer == null)
                throw new NotImplementedException();
            return Serializer.DeSerialize<T>(data);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        public static Serializer Serializer { set; get; }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class Serializer
    {
        public abstract byte[] Serialize(object obj);

        public abstract T DeSerialize<T>(byte[] data);
    }
}
