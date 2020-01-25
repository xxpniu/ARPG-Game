using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PServicePugin
{
    public enum ProtoType
    {
        Int32,
        String,
        Float,
        ProtoStruct,
        Service,
        RPC,
        NameSpace
    }

    public class Option
    {
        public string Name { set; get; }

        public string Value { set; get; }
    }

    public class ProtoStruct
    {
        public string Name { set; get; }

        public ProtoType Type { set; get; }

        public List<ProtoStruct> Structs  { set; get; }
    }

    public class ProtoService : ProtoStruct
    {
         
    }

    public class ProtoRpc : ProtoStruct
    {
        public ProtoStruct Request { set; get; }

        public ProtoStruct Response { set; get; }

    }


}
