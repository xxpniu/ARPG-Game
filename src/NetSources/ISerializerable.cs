using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Proto
{
    public interface ISerializerable
    {
        void ParseFormBinary(BinaryReader reader);
        void ToBinary(BinaryWriter writer);
    }

    public sealed class ProtoTool
    {
        public static int GetVersion()
        {
            var v = (byte)GameVersion.Master * 100 * 100 +
                                     (byte)GameVersion.Major * 100  +
                                       (byte)GameVersion.Develop ;
            return v;
        }

        public static bool CompareVersion(int version)
        {
            return version == GetVersion();
        }
    }
}