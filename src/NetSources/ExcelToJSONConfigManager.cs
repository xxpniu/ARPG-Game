using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExcelConfig
{
    public class ExcelDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        private object SyncRoot = new object();

        private Dictionary<K, V> dicts = new Dictionary<K, V>();

        public bool ContainsKey(K k)
        {
            lock (SyncRoot)
            {
                return dicts.ContainsKey(k);
            }
        }

        public bool TryGetValue(K k, out V v)
        {
            lock (SyncRoot)
            {
                return dicts.TryGetValue(k, out v);
            }
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            List<KeyValuePair<K, V>> list;
            lock (SyncRoot)
            {
                list = dicts.ToList();
            }
            return list.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            List<KeyValuePair<K, V>> list;
            lock (SyncRoot)
            {
                list = dicts.ToList();
            }
            return list.GetEnumerator();
        }

        public V this[K k]
        {
            get
            {
                V v = default(V);
                TryGetValue(k, out v);
                return v;
            }

            set
            {
                lock (SyncRoot)
                {
                    if (dicts.ContainsKey(k))
                        dicts[k] = value;
                    else
                        dicts.Add(k, value);
                }
            }
        }

        public int Count
        {
            get
            {
                lock (SyncRoot) return dicts.Count;
            }
        }

        public List<V> Values
        {
            get
            {
                lock (SyncRoot)
                {
                    return dicts.Values.ToList();
                }
            }
        }
    }

    public class ExcelToJSONConfigManager
    {
        public ExcelToJSONConfigManager(IConfigLoader loader)
        {
            Loader = loader;
            configs = new ExcelDictionary<Type, ExcelDictionary<int,JSONConfigBase>>();
            Current = this;
        }

        private IConfigLoader Loader { set; get; }
        public static ExcelToJSONConfigManager Current { private set; get; }

        private ExcelDictionary<Type, ExcelDictionary<int, JSONConfigBase>> configs;

        private void TryToLoad<T>() where T : JSONConfigBase
        {
            var ty = typeof(T);
            if (configs.ContainsKey(ty))
            {
                return;
            }
            var data = Loader.Deserialize<T>();
            var disct = new ExcelDictionary<int, JSONConfigBase>();
            foreach (var i in data)
            {
                disct[i.ID] = i;
            }
            this.configs[ty] = disct;
        }

        public T GetConfigByID<T>(int id) where T : JSONConfigBase
        {
            TryToLoad<T>();
            var ty = typeof(T);
            if (configs.ContainsKey(ty))
            {
                return configs[ty][id] as T;
            }

            return default(T);
        }

        public T[] GetConfigs<T>() where T : JSONConfigBase
        {
            TryToLoad<T>();
            var ty = typeof(T);
            var list = new List<T>();
            if (configs.ContainsKey(ty))
            {
                var values = configs[ty].Values;
                foreach (var i in values)
                {
                    list.Add(i as T);
                }
                return list.ToArray(); 
            }
            return new T[0];
        }

        public T[] GetConfigs<T>(FindCondition<T> condtion) where T : JSONConfigBase
        {
            TryToLoad<T>();
            var ty = typeof(T);
            var list = new List<T>(16);
            if (configs.ContainsKey(ty))
            {
                var values = configs[ty].Values;
                foreach (var i in values)
                {
                    list.Add(i as T);
                }
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
                list = configs[ty].Values.ToArray() as T[];
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
