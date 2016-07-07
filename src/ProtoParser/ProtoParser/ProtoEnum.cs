using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoParser
{

    public class ProtoEnumField
    {
        public string Name{set;get;}
        public string Value{set;get;}
        public string Note { get; set; }
    }
    public class ProtoEnum
    {
        public string Name { set; get; }

        public string Node { set; get; }
        public List<ProtoEnumField> Fields { set; get; }
        public ProtoEnum(string name,string node)
        {
            Name = name;
            Node = node;
            Fields = new List<ProtoEnumField>();
        }

        public void AddField(ProtoEnumField field)
        {
            Fields.Add(field);
        }
    }
}
