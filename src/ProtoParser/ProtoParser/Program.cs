using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = string.Empty;
            var file = string.Empty;
            var fileSave = string.Empty;
            foreach(var i in args)
            {
                if (i.StartsWith("dir:"))
                {
                    root = i.Replace("dir:", "");
                }

                if (i.StartsWith("file:"))
                {
                    file = i.Replace("file:", "");
                }

                if (i.StartsWith("saveto:"))
                {
                    fileSave = i.Replace("saveto:", "");
                }
            }

            Console.WriteLine(string.Format("dir:{0} file:{1} saveto:{2}", root, file, fileSave));
            var proto = new ParserProto(root);
            proto.CompieFile(file);
            proto.TOcsFile(fileSave);
        }
    }
}
