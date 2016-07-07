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
}