using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.Utility
{
    public class SyncList<T> 
    {
        private object syncRoot = new object();

        private List<T> _list = new List<T>();
        public List<T> ToList() {
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
    }

    
}
