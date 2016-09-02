using System;
using System.Collections.Generic;
using EngineCore;
using Proto;

namespace MapServer
{
    public static class Extands
    {

        public static GVector3 ToGVector3(this MapNode ver)
        {
            return new GVector3(ver.X, ver.Y, ver.Z);
        }

        public static GVector3 ToGVector3(this Vector3 ver)
        {
            return new GVector3(ver.x, ver.y, ver.z);
        }

        public static GVector3 ToGVector3(this Layout.Vector3 ver)
        {
            return new GVector3(ver.x, ver.y, ver.z);
        }

        public static Vector3 ToNetVer3(this GVector3 ver)
        {
            return new Vector3 { x = ver.x, y = ver.y, z = ver.z};
        }


        public static List<int> SplitToInt(this string str, char key)
        {
            var arrs = str.Split(key);
            var list = new List<int>();

            foreach (var i in arrs)
            {
                list.Add(int.Parse(i));
            }
            return list;
        }

    }
}

