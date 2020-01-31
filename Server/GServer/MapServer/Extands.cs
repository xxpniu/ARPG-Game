using System;
using System.Collections.Generic;
using EngineCore;
using Proto;
using UMath;

namespace MapServer
{
    public static class Extands
    {

        public static UVector3 ToGVector3(this MapNode ver)
        {
            return new UVector3(ver.X, ver.Y, ver.Z);
        }

        public static UVector3 ToGVector3(this Vector3 ver)
        {
            return new UVector3(ver.X, ver.Y, ver.Z);
        }

        public static UVector3 ToGVector3(this Layout.Vector3 ver)
        {
            return new UVector3(ver.x, ver.y, ver.z);
        }

        public static Vector3 ToNetVer3(this UVector3 ver)
        {
            return new Vector3 { X = ver.x, Y = ver.y, Z = ver.z};
        }


        /// <summary>
        /// Splits to int.
        /// </summary>
        /// <returns>The to int.</returns>
        /// <param name="str">String.</param>
        /// <param name="sKey">Key.</param>
        public static List<int> SplitToInt(this string str, char sKey = '|')
        {
            var arrs = str.Split(sKey);
            var list = new List<int>();
            foreach (var i in arrs)
            {
                list.Add(int.Parse(i));
            }
            return list;
        }

    }
}

