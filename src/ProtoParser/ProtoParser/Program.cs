using System;
using System.Collections.Generic;
using System.IO;
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
            var type = string.Empty;
            foreach (var i in args)
            {
                if (i.StartsWith("dir:", StringComparison.CurrentCultureIgnoreCase))
                {
                    root = i.Replace("dir:", "");
                }

                if (i.StartsWith("file:", StringComparison.CurrentCultureIgnoreCase))
                {
                    file = i.Replace("file:", "");
                }

                if (i.StartsWith("saveto:", StringComparison.CurrentCultureIgnoreCase))
                {
                    fileSave = i.Replace("saveto:", "");
                }
                if (i.StartsWith("type:", StringComparison.CurrentCultureIgnoreCase))
                {
                    type = i.Replace("type:", "");
                }
            }

            Console.WriteLine(string.Format("dir:{0} file:{1} saveto:{2}", root, file, fileSave));
            switch (type)
            {
                case "handle":
                    {
                        var messages = new HashSet<string>();
                        var files = file.Split(',');
                        foreach (var i in files)
                        {
                            var proto = new ParserProto(root);
                            proto.CompieFile(i);
                            foreach (var s in proto.Structs)
                            {
                                if (messages.Contains(s.Name)) continue;
                                messages.Add(s.Name);
                            }
                        }

                        //var temp = @"   []"
                        var sb = new StringBuilder();
                        foreach (var i in messages)
                        {
                            sb.AppendLine(@"    [MessageHandle(typeof(" + i + ")," + i.GetHashCode() + ")]");
                        }

                        var result = HandleTypes.Replace("[HANDLETYPES]", sb.ToString());
                        File.WriteAllText(Path.Combine(root, fileSave), result);
                    }
                    break;
                default:
                    {
                        //Console.WriteLine(string.Format("dir:{0} file:{1} saveto:{2}", root, file, fileSave));
                        var files = file.Split(',');
                        foreach (var i in files)
                        {
                            var proto = new ParserProto(root);
                            proto.CompieFile(i);
                            proto.TOcsFile(fileSave);
                        }
                    }
                    break;
            }
        }


        private static string HandleTypes =@"using System;
using System.Collections.Generic;

namespace Proto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class MessageHandleAttribute : Attribute
    {        
        public MessageHandleAttribute(Type type, int handleId)
        {
            Type = type;
            HandleID = handleId;
        }

        public Type Type { set; get; }

        public int HandleID { set; get; }
    }


[HANDLETYPES]
    public sealed class MessageHandleTypes
    {
        static MessageHandleTypes()
        {
            var handles = typeof(MessageHandleTypes)
                .GetCustomAttributes(typeof(MessageHandleAttribute), false) 
                as MessageHandleAttribute[];
            foreach (var i in handles)
            {
                _typeToIndex.Add(i.Type,i.HandleID);
                _indexToType.Add(i.HandleID,i.Type);
            }

        }

        private static Dictionary<int, Type> _indexToType = new Dictionary<int, Type>();
        private static Dictionary<Type, int> _typeToIndex = new Dictionary<Type, int>();

        public static Type GetTypeByIndex(int index)
        {
            Type type;
            if (_indexToType.TryGetValue(index, out type)) return type;
            return null;
        }

        public static bool GetTypeIndex(Type t,out int index)
        {
            return _typeToIndex.TryGetValue(t, out index);
        }
    }
}
";

    }
}
