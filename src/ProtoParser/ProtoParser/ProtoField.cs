using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoParser
{

    public class ProtoTypes
    {
        public const string INT_32 = "int32";
        public const string STRING = "string";
        public const string SHORT = "int16";
        public const string BYTE = "int8";
        public const string CHAR = "char";
        public const string FLOAT = "float";
        public const string BOOL = "bool";
        public const string INT_64 = "int64";
    }

    public class ProtoField
    {
        public bool IsArray { set; get; }
        public string Name { set;get; }

        public string Type { set; get; }

        public string Note { get; set; }
    }
}
