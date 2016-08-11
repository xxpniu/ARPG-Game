using System;
using Proto;
using XNet.Libs.Net;

namespace ServerUtility
{

    [AttributeUsage(AttributeTargets.Class)]
    public class HandleTypeAttribute : Attribute
    {
        public HandleTypeAttribute(Type type)
        {
            HandleType = type;
        }

        public Type HandleType { set; get; }
    }

    public abstract class Responser<T,R>
        where  T:ISerializerable where R:ISerializerable
    {
        public Responser()
        {
            NeedAccess = true;
        }

        public bool NeedAccess { protected set; get; }

        public abstract R DoResponse(T request, Client client);
    }
}

