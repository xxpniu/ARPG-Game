using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ServerUtility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MonitorAttribute : Attribute
    {
        public MonitorAttribute() { }
    }

    public interface IMonitor
    {
        void OnShowState();
        void OnTick();
        void OnExit();
        void OnStart();
    }

    public class MonitorPool:XSingleton<MonitorPool>
    {
        public void Init(Assembly assemley)
        {
            monitores.Clear();
            var types = assemley.GetTypes();
            foreach (var i in types)
            {
                var attrs = i.GetCustomAttributes<MonitorAttribute>();
                if (attrs.Count() > 0)
                {
                    monitores.Add(Activator.CreateInstance(i) as IMonitor);
                }
            }
        }

        private readonly List<IMonitor> monitores = new List<IMonitor>();

        public void Start() {
            foreach (var i in monitores) i.OnStart();
        }
        public void Tick() { foreach (var i in monitores) i.OnTick(); }
        public void Exit() { foreach (var i in monitores) i.OnExit(); }
        public void ShowState() { foreach (var i in monitores) i.OnShowState(); }

        public T GetMointor<T>() where T : class,IMonitor
        {
            foreach (var i in monitores)
            {
                if (i is T) return i as T;
            }
            return null;
        }

        public T Get<T>() where T : class, IMonitor
        {
            return GetMointor<T>();
        }

    }
}

