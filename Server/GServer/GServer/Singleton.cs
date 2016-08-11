using System;
namespace GServer
{

    public class XSingleton<T> where T : class, new()
    {
        protected static T _instance;
        public static T Singleton
        {
            get
            {
                if (_instance == null)
                    _instance = new T();

                return _instance;
            }
        }
    }
}

