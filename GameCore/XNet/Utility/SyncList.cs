using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.Utility
{
    public class SyncList<T>
    {
        private object syncRoot = new object();
        private int count = 0;

        private List<T> _list = new List<T>();
        public List<T> ToList()
        {
            lock (syncRoot)
                return _list.ToList();
        }

        public void Add(T item)
        {
            lock (syncRoot)
            {
                _list.Add(item);
                count = _list.Count;
            }
        }

        public void Remove(T item)
        {
            lock (syncRoot)
            {
                _list.Remove(item);
                count = _list.Count;
            }
        }


        public void Clear()
        {

            lock (syncRoot)
            {
                _list.Clear();
                count = 0;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

    }

    public class SyncDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        public SyncDictionary() : this(4) { }
        public SyncDictionary(int capity)
        {
            _data = new Dictionary<K, V>(capity);
        }
        private int count = 0;

        private object syncRoot = new object();

        private Dictionary<K, V> _data;

        public int Count
        {
            get
            {
                return count;
            }
        }


        public List<V> Values
        {
            get
            {
                lock (syncRoot) return _data.Values.ToList();
            }
        }

        public List<K> Keys
        {
            get
            {
                lock (syncRoot) return _data.Keys.ToList();
            }
        }


        public void Clear()
        {
            lock (syncRoot) { count = 0; _data.Clear();}
        }

        public bool Add(K k, V v)
        {
            lock (syncRoot)
            {
                if (_data.ContainsKey(k)) return false;
                _data.Add(k, v);
                count = _data.Count;
                return true;
            }
        }
        public bool Remove(K k)
        {
            lock (syncRoot)
            {
                var res = _data.Remove(k);
                count = _data.Count;
                return res;
            }
        }

        public bool HaveKey(K k)
        {
            lock (syncRoot)
            {
                return _data.ContainsKey(k);
            }
        }

        public bool TryToGetValue(K k, out V v)
        {
            lock (syncRoot)
            {
                return _data.TryGetValue(k, out v);
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            lock (syncRoot)
            {
                return _data.ToList().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (syncRoot)
            {
                return _data.ToList().GetEnumerator();
            }
        }

        public V this[K k]
        {
            get
            {
                lock (syncRoot)
                {
                    return _data[k];
                }
            }

            set
            {
                lock (syncRoot)
                {
                    _data[k] = value;
                }
            }
        }
    }
}
