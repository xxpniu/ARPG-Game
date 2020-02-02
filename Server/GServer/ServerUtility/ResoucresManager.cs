using System.Collections.Generic;
using System.IO;
using ExcelConfig;
using org.vxwo.csharp.json;

namespace ServerUtility
{
    public class ResourcesLoader : XSingleton<ResourcesLoader>, IConfigLoader
    {


        private string ConfigRoot;

        public void LoadAllConfig(string configRoot)
        {
            this.ConfigRoot = configRoot;
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

        public List<T> Deserialize<T>() where T : JSONConfigBase
        {
            var name = ExcelToJSONConfigManager.GetFileName<T>();
            var path = Path.Combine(ConfigRoot, "Configs/" + name);
            var json = File.ReadAllText(path);
            if (json == null) return null;
            return JsonTool.Deserialize<List<T>>(json);
        }
    }


}

