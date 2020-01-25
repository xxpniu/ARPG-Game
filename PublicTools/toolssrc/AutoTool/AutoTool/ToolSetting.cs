using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool
{
    public class ToolSetting
    {
        public string Path;
        public string HttpHost;
        public string JsonPath;
        public string User;
        public string Version;
        public bool Force;
        public override string ToString()
        {
            return "Path:" + Path
                + "\nHost:" + HttpHost
                + "\nJsonPath:" + JsonPath
                + "\nUser:" + User
                + "\nForce:" + Force
                + "\nversion:" + Version;
        }
    }
}
