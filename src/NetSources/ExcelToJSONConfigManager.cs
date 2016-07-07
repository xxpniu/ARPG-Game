using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExcelConfig
{
    public class ExcelToJSONConfigManager
    {
        public ExcelToJSONConfigManager(IConfigLoader loader)
        {
            Loader = loader;
            configs = new Dictionary<Type, JSONConfigBase[]>();
            Current = this;
        }


        private IConfigLoader Loader { set; get; }
        public static ExcelToJSONConfigManager Current { private set; get; }

        public Dictionary<Type, JSONConfigBase[]> configs;

        private void TryToLoad<T>() where T : JSONConfigBase
        { 
            var ty = typeof(T);
            if (configs.ContainsKey(ty))
            {
                return;
            }

			var data = Loader.Deserialize<T>();
			
			if (this.configs.ContainsKey(ty))
			{
				this.configs[ty] = data.ToArray();
			}
			else
			{
				this.configs.Add(ty, data.ToArray());
			}

        }

        public T GetConfigByID<T>(int id) where T : JSONConfigBase
        {
            TryToLoad<T>();
            var ty = typeof(T);
            T[] list;
            if (configs.ContainsKey(ty))
            {
                list = configs[ty] as T[];

                return list.FirstOrDefault(t => t.ID == id);
            }

            return default(T);
        }

        public T[] GetConfigs<T>() where T : JSONConfigBase
        {
            TryToLoad<T>();
            var ty = typeof(T);
            T[] list;
            if (configs.ContainsKey(ty))
            {
                list = configs[ty] as T[];
                return list;
            }
            return new T[0];
        }

        public T[] GetConfigs<T>(FindCondition<T> condtion) where T : JSONConfigBase
        {
            TryToLoad<T>();
            var ty = typeof(T);
            T[] list;
            if (configs.ContainsKey(ty))
            {
                list = configs[ty] as T[];
                return list.Where(t => condtion(t)).ToArray();
            }
            return new T[0];
        }

        public static string GetFileName<T>() where T : JSONConfigBase
        {
            var type = typeof(T);
            var atts = type.GetCustomAttributes(typeof(ConfigFileAttribute), false) as ConfigFileAttribute[];

            if (atts.Length > 0)
            {
                return atts[0].FileName;
            }
            return string.Empty;
        }

        public delegate bool FindCondition<T>(T item) where T : JSONConfigBase;

        public T FirstConfig<T>(FindCondition<T> conditon) where T : JSONConfigBase
        {

            TryToLoad<T>();
            var ty = typeof(T);
            T[] list;
            if (configs.ContainsKey(ty))
            {
                list = configs[ty] as T[];
                return list.FirstOrDefault(t => conditon(t));
            }
            return default(T);
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigFileAttribute : Attribute
    {
        public ConfigFileAttribute(string fileName,string tableName)
        {
            this.FileName = fileName;
            this.TableName = tableName;
        }

        public string TableName { set; get; }

        public string FileName { set; get; }
    }

    public class JSONConfigBase
    {
        public int ID { set; get; }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelConfigColIndexAttribute : Attribute {
        public ExcelConfigColIndexAttribute(int index)
        {
            Index = index;
        }
        public int Index { set; get; }
    }

    public interface IConfigLoader
    {
		List<T> Deserialize<T>() where T: JSONConfigBase; 
    }
}
