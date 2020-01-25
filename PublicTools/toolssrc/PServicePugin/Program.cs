using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PServicePugin
{
    class Program
    {
 
        static Regex regex;
        static Program()
        {
            var str = @"rpc[ ]+([^\(]+)\(([^\)]+)\)[ ]*returns[ \(]+([^\)]+)\)";
            regex = new Regex(str); //1 api name , 2 request 3 response
        }
        static void Main(string[] args)
        {
            //message class1,class 2
            //args
            var root = string.Empty;
            var file = string.Empty;
            var fileSave = string.Empty;
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
            }

            Console.WriteLine(string.Format("dir:{0} file:{1} saveto:{2}", root, file, fileSave));
            StringBuilder sb = null;
            var path = Path.Combine(root, file);
            string comment = string.Empty;
            string serives = string.Empty;
            string nameSpace = string.Empty;
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
                            nameSpace = strs[1];
                            //[NAMESPACE]
                            Console.WriteLine("nameSpace:" + nameSpace);
                            break;
                        case "service":
                            serives = strs[1];
                            sb = new StringBuilder();
                            Console.WriteLine("service:"+ serives);
                            break;
                        case "}":
                            if (sb != null)
                            {
                                var result = FileTemplate.Replace("[CLASSES]", sb.ToString()).Replace("[SERVICE]", serives)
                                    .Replace("[NAMESPACE]",nameSpace);
                                File.WriteAllText(Path.Combine(root, $"{fileSave}/{serives}.cs"), result);
                                sb = null;
                                serives = string.Empty;
                            }
                            break;
                        case "rpc":
                            if (sb == null) throw new Exception("Rpc must in services");
                            //regex 
                            //1 api name , 2 request 3 response api url t[1]
                            ProcessRPC(line, out string api, out string re, out string res, out string url);
                            var note = string.Empty;
                            var sbT = new StringBuilder();
                            note = sbT.ToString();
                            var code = MessageTemplate.Replace("[Name]", api)
                                .Replace("[Request]", re ).Replace("[Response]", res)
                                .Replace("[NOTE]", url)
                                .Replace("[API]",url);
                            sb.AppendLine(code);
                            break;

                    }
                }
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
using [NAMESPACE]
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.[SERVICE]
{
[CLASSES]
}";

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

    /// <summary>
    /// [NOTE]
    /// </summary>    
    [API([API])]
    partial class [Name]Handler:APIHandler<[Request],[Response]>
    {
     
    }
    ";


    }

    
}
