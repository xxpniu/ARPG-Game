using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PServicePugin
{
    class ProtoProcess
    {
        public static void Process(string path, HashSet<string> types)
        {
            var file = File.ReadAllText(path);
        }
    }
}
