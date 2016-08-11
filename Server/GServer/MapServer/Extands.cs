using System;
using EngineCore;
using OpenTK;
namespace MapServer
{
    public static class Extands
    {

        public static Vector3 ToVector3(this GVector3 ver)
        {
            return new Vector3(ver.x, ver.y, ver.z);
        }

        public static GVector3 ToGVector3(this Vector3 ver)
        {
            return new GVector3(ver.X, ver.Y, ver.Z);
        }
    }
}

