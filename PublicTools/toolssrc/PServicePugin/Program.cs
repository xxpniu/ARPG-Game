using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PServicePugin
{

    public struct RPCCall
    {
        public string API;
        public string Request;
        public string Response;
        public string Url;
    }

    class Program
    {
 
        static Regex regex;
        static Program()
        {
            var str = @"rpc[ ]+([^ \(]+)[ \(]+([^\)]+)\)[ ]*returns[ \(]+([^\)]+)\)";
            regex = new Regex(str); //1 api name , 2 request 3 response
        }

        private static int Index_OF_API = 10000;
        private static HashSet<string> types = new HashSet<string>();

        static void Main(string[] args)
        {
            //message class1,class 2
            //args
            var root = string.Empty;
            var file = string.Empty;
            var fileSave = string.Empty;
            var indexFileName = "MessageIndex.cs";
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

                if (i.StartsWith("index:", StringComparison.CurrentCultureIgnoreCase))
                {
                    indexFileName = i.Replace("index:", "");
                }
            }

            Console.WriteLine($"dir:{root} file:{file} saveto:{fileSave} index:{indexFileName}");
            StringBuilder sb = null;
            var paths = Directory.GetFiles(root, file);

            string comment = string.Empty;
            string serives = string.Empty;
            string nameSpace = string.Empty;

            Stack<RPCCall> calls =null;

            foreach (var path in paths)
            {
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        line = line.Trim();
                        //message class1 class2
                        var strs = line.Split("\t/ ".ToArray());
                        if (strs.Length == 0) continue;
                        //Console.WriteLine(line);
                        switch (strs[0])
                        {
                            case "package":
                                nameSpace = strs[1].Replace(";","");
                                //[NAMESPACE]
                                Console.WriteLine("nameSpace:" + nameSpace);
                                break;
                            case "service":
                                serives = strs[1];
                                sb = new StringBuilder();
                                Console.WriteLine("service:" + serives);
                                calls = new Stack<RPCCall>();
                                break;
                            case "}":
                                if (sb != null)
                                {

                                    var callInvokes = new StringBuilder();
                                    while (calls.Count > 0)
                                    {
                                        var c = calls.Pop();
                                        //@"        [API([URL])][Response] [API]([Request] req);"
                                        callInvokes.AppendLine(IRPCCall
                                            .Replace("[Response]", c.Response)
                                            .Replace("[Request]", c.Request)
                                            .Replace("[API]", c.API)
                                            .Replace("[URL]", c.Url));
                                    }

                                    var icall = IRPCService.Replace("[SERVICE]", serives)
                                        .Replace("[RPC]", callInvokes.ToString());

                                    sb.AppendLine(icall);

                                    var result = FileTemplate.Replace("[CLASSES]", sb.ToString()).Replace("[SERVICE]", serives)
                                        .Replace("[NAMESPACE]", nameSpace);
                                    File.WriteAllText(Path.Combine(root, $"{fileSave}/{serives}.cs"), result);
                                    sb = null;
                                    serives = string.Empty;
                                }
                                break;
                            case "rpc":
                                if (sb == null) throw new Exception("Rpc must in services");
                                //regex 
                                //1 api name , 2 request 3 response api url t[1]
                                Index_OF_API++;
                                ProcessRPC(line, out string api, out string re, out string res, out string url);
                                url = $"{Index_OF_API}";
                                Console.WriteLine($"{url}->rpc {api} ( {re} )return( {res} )");
                                AddType(re, res);
                                var code = MessageTemplate.Replace("[Name]", api)
                                    .Replace("[Request]", re).Replace("[Response]", res)
                                    .Replace("[NOTE]", url)
                                    .Replace("[API]", url);
                                sb.AppendLine(code);
                                calls?.Push(new RPCCall { API = api, Request = re, Response = res, Url = url });
                                break;

                        }
                    }
                }

            }

            var index_sb = new StringBuilder();

            short startIndex = 10000;
            foreach (var i in types)
            {
                startIndex++;
                var str = $"    [Index({startIndex},typeof({i}))]";
                index_sb.AppendLine(str);
            }

            var index_cs = Temple_Index
            .Replace("[NAMESPACE]", nameSpace.Replace(";",""))
                .Replace("[ATTRIBUTES]",index_sb.ToString());

            File.WriteAllText(Path.Combine(root, $"{fileSave}/{indexFileName}"), index_cs);

        }

        private static void AddType(params string[] tys)
        {

            foreach (var t in tys)
            {
               var  ty = t.Trim();
                if (types.Contains(ty)) continue;
                types.Add(ty);
            }
        }



        private static bool ProcessRPC(string line, out string api, out string request, out string response, out string apiurl)
        {
            line = line.Trim();
            var match = regex.Match(line);
            if (match.Groups.Count >= 4)
            {
                api = match.Groups[1].Value;
                request = match.Groups[2].Value;
                response = match.Groups[3].Value;

                var google = "google.protobuf.";
                request = request.Replace(google, "");
                response = response.Replace(google, "");
            }
            else throw new Exception($"error:{line}");
            var temp = line.Replace(@"//", "\n");
            var t = temp.Split('\n');
            apiurl = t[1];
            return match.Success;
        }

        public static string FileTemplate = @"
using [NAMESPACE];
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace [NAMESPACE].[SERVICE]
{
[CLASSES]
}";

        public static string IRPCService = @"
    public interface I[SERVICE]
    {
[RPC]
    }
   ";
        public static string IRPCCall = @"        [API([URL])][Response] [API]([Request] req);";

        public static string MessageTemplate = @"
    /// <summary>
    /// [NOTE]
    /// </summary>    
    [API([API])]
    public class [Name]:APIBase<[Request], [Response]> 
    {
        private [Name]() : base() { }
        public  static [Name] CreateQuery(){ return new [Name]();}
    }
    ";

        private static string Temple_Index = @"using System;
using System.Collections.Generic;

namespace [NAMESPACE]
{

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public class IndexAttribute:Attribute
    {
         public IndexAttribute(int index,Type tOm) 
         {
            this.Index = index;
            this.TypeOfMessage = tOm;
         }

        public int Index { set; get; }

        public Type TypeOfMessage { set; get; }
    }

[ATTRIBUTES]
    public static class MessageTypeIndexs
    {
        private static readonly Dictionary<int, Type> types = new Dictionary<int, Type>();

        private static readonly Dictionary<Type, int> indexs = new Dictionary<Type, int>();
        
        static MessageTypeIndexs()
        {
            var tys = typeof(MessageTypeIndexs).GetCustomAttributes(typeof(IndexAttribute), false) as IndexAttribute[];

            foreach(var t in tys)
            {
                types.Add(t.Index, t.TypeOfMessage);
                indexs.Add(t.TypeOfMessage, t.Index);
            }
        }

        /// <summary>
        /// Tries the index of the get.
        /// </summary>
        /// <returns><c>true</c>, if get index was tryed, <c>false</c> otherwise.</returns>
        /// <param name=""type"">Type.</param>
        /// <param name=""index"">Index.</param>
        public static bool TryGetIndex(Type type,out int index)
        {
            return indexs.TryGetValue(type, out index);
        }
        /// <summary>
        /// Tries the type of the get.
        /// </summary>
        /// <returns><c>true</c>, if get type was tryed, <c>false</c> otherwise.</returns>
        /// <param name=""index"">Index.</param>
        /// <param name=""type"">Type.</param>
        public static bool TryGetType(int index,out Type type)
        {
            return types.TryGetValue(index, out type);
        }
    }
}
";

    }

    
}
