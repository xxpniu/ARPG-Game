using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Utility;
using static AutoTool.JsonPath;

namespace AutoTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "--json", "sss" };
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var jsonPath = string.Empty;//"mapping.json";
            var httpHost = string.Empty;// "https://data-api-testfulllink.gkid.com";
            var cookie = string.Empty;// @"user=2|1:0|10:1559293195|4:user|20:SG9uZ0NoZW5QaWFvS2U=|37df841d2c8b0fabb3e6634b2cb7e03281a6353bf7a282dd76a76096d66ad36e";
            var force = false;
            var ignore = string.Empty;
            var log = false;

            foreach (var i in args)
            {
                Console.WriteLine(i);
            }

            for (var i = 0; i < args.Length; i++)
            {
                var command = args[i];
                
                if (command.StartsWith("--", StringComparison.OrdinalIgnoreCase))
                {
                    switch (command)
                    {
                        case "--json":
                            jsonPath = args[++i];
                            break;
                        case "--path":
                            path = args[++i];
                            break;
                        case "--host":
                            httpHost = args[++i];
                            break;
                        case "--cookie":
                            cookie = args[++i];
                            break;
                        case "--force":
                            force = args[++i] == "true";
                            break;
                        case "--ignore":
                            ignore = args[++i];
                            break;
                        case "--log":
                            log = args[++i] == "true";
                            break;
                    }
                }
            }

            var json = File.ReadAllText(Path.Combine(path, jsonPath));
            var list = Tool.ToObject<Root>(json);

            var web = new WebRequestManager(new ToolSetting
            {
                HttpHost = httpHost,
                JsonPath = jsonPath,
                Path = path,
                User = cookie,
                Version = DateTime.Now.ToString("yyMMdd-HHmmss"),
                Force = force
            })
            {
                LOG = log
            };
            web.Setting.ToString().Print(ConsoleColor.Green);


            foreach (var i in list.files)
            {
                if (checkIgnore(ignore, i)) continue;
                web.UploadFile(i).Wait();
            }

        }

       

        private static bool checkIgnore(string ignore, FilesItem i)
        {
            if (string.IsNullOrEmpty(ignore)) return false;
            foreach (var ig in ignore.Split(','))
            {
                if (i.path.StartsWith(ig,StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }
       
    }
}
