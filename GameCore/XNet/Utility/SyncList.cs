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

        private List<T> _list = new List<T>();
        public List<T> ToList()
        {
            lock (syncRoot)
                return _list.ToList();
        }

        public void Add(T item)
        {
            lock (syncRoot)
                _list.Add(item);
        }

        public void Remove(T item)
        {
            lock (syncRoot)
                _list.Remove(item);
        }


        public void Clear()
        {

            lock (syncRoot)
                _list.Clear();
        }


        public int Count
        {
            get
            {
                lock (syncRoot) return _list.Count;
            }
        }

    }

    public class SyncDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {


        private object syncRoot = new object();

        private Dictionary<K, V> _data = new Dictionary<K, V>();

        public int Count
        {
            get
            {
                lock (syncRoot) return _data.Count;
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
            lock (syncRoot) _data.Clear();
        }

        public bool Add(K k, V v)
        {
            lock (syncRoot)
            {
                if (_data.ContainsKey(k)) return false;
                _data.Add(k, v);
                return true;
            }
        }
        public bool Remove(K k)
        {
            lock (syncRoot)
            {
                return _data.Remove(k);
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
