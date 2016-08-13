using System;
using System.Collections.Generic;
using System.IO;
using ExcelConfig;
using Layout;
using Layout.AITree;
using Layout.LayoutElements;
using org.vxwo.csharp.json;
using ServerUtility;
using XNet.Libs.Utility;

namespace MapServer
{
    public class ResourcesLoader:XSingleton<ResourcesLoader>, ExcelConfig.IConfigLoader
    {
        public ResourcesLoader()
        {
            
        }


        SyncDictionary<string, MagicData> _magicData;

        SyncDictionary<string, TimeLine> _timeLines;
        SyncDictionary<string, TreeNode> _aiTree;


        private string ConfigRoot;

        public void LoadAllConfig(string configRoot)
        {

            this.ConfigRoot = configRoot.StartsWith("" + Path.DirectorySeparatorChar, StringComparison.Ordinal)
                ? configRoot : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configRoot);

            _magicData = new SyncDictionary<string, Layout.MagicData>();
            _timeLines = new SyncDictionary<string, TimeLine>();
            _aiTree = new SyncDictionary<string, TreeNode>();
            var magics = Directory.GetFiles(Path.Combine(ConfigRoot, "Magics"), "*.xml");

            foreach (var i in magics)
            {
                var xml = File.ReadAllText(i, XmlParser.UTF8);
                var m = XmlParser.DeSerialize<Layout.MagicData>(xml);
                _magicData.Add(m.key, m);
            }

            var timeLines = Directory.GetFiles(Path.Combine(ConfigRoot, "Layouts"), "*.xml");
            foreach (var i in timeLines)
            {
                var xml = File.ReadAllText(i, XmlParser.UTF8);
                var line = XmlParser.DeSerialize<TimeLine>(xml);
                _timeLines.Add("Layouts/" + Path.GetFileName(i), line);
            }

            var aitrees = Directory.GetFiles(Path.Combine(ConfigRoot, "AI"), "*.xml");
            foreach (var i in aitrees)
            {
                var xml = File.ReadAllText(i, XmlParser.UTF8);
                var note = XmlParser.DeSerialize<TreeNode>(xml);
                _aiTree.Add("AI/" + Path.GetFileName(i), note);
            }

            var Manager = new ExcelToJSONConfigManager(this);

            var assemblyTypes = Manager.GetType().Assembly.GetTypes();
            var mothed = Manager.GetType().GetMethod("GetConfigByID");
            foreach (var i in assemblyTypes)
            {
                if (i.IsSubclassOf(typeof(JSONConfigBase)))
                {
                    var m = mothed.MakeGenericMethod(i);
                    m.Invoke(Manager, new object[] { 0 });
                }
            }

        }

        public MagicData GetMagicByKey(string key)
        {
            return _magicData[key];
        }

        public bool HaveMagicKey(string key)
        {
            return _magicData.HaveKey(key);
        }

        public TimeLine GetTimeLineByPath(string path)
        {
            return _timeLines[path];
        }

        public TreeNode GetAITree(string pathTree)
        {
            return _aiTree[pathTree];
        }

        public List<T> Deserialize<T>() where T : JSONConfigBase
        {
            var name = ExcelConfig.ExcelToJSONConfigManager.GetFileName<T>();
            var path = Path.Combine(ConfigRoot, "Configs/" + name);
            var json = File.ReadAllText(path);
            if (json == null)
                return null;
            return JsonTool.Deserialize<List<T>>(json);
        }
    }
}

