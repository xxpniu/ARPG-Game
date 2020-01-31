using System;
using Proto;
using EngineCore;
using UMath;
using Google.Protobuf;

namespace GameLogic
{
    public static class Extands
    {
        public static Vector3 ToV3(this UVector3 v3)
        {
            return new Vector3 { X = v3.x, Y = v3.y,Z =v3.z};
        }

        public static Vector3 ToV3(this Layout.Vector3 v3)
        {
            return new Vector3 { X = v3.x, Y = v3.y, Z = v3.z };
        }

        public static UVector3 ToUv3(this Proto.Vector3 v)
        {
            return new UVector3(v.X, v.Y, v.Z);
        }


    }
}

