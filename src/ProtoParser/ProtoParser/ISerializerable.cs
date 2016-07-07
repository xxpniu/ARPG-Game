using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Proto
{
    public interface ISerializerable
    {
        void ParseFormBinary(BinaryReader reader);
        byte[] ToBinary(BinaryWriter writer);
    }
}