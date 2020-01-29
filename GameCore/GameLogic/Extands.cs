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

        public static Vector3ShortIndex ToSV3(this UVector3 v3)
        {
            return new Vector3ShortIndex() { X = (byte)v3.x, Y = (byte)v3.y, Z = (byte)v3.z };
        }

        public static IMessage ParseFromBytes(this Type type, byte[] bytes)
        {
            var parser = type.GetProperty("Parser").GetValue(null, null) as MessageParser;
            return parser.ParseFrom(bytes);
        }
    }
}

