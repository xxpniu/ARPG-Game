using System;
using System.Reflection;
using System.Threading.Tasks;
using Google.Protobuf;
using Proto;
using XNet.Libs.Net;

namespace ServerUtility
{

    [AttributeUsage(AttributeTargets.Class)]
    public class HandleAttribute : Attribute
    {
        public HandleAttribute(Type rTy)
        {
            RType = rTy;
        }
        public Type RType { set; get; }

       

    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class IgnoreAdmissionAttribute : Attribute
    {

    }

    public class Responser
    {
        public Responser(Client requestClient)
        {
            Client = requestClient;
        }

        /// <summary>
        /// The clent
        /// </summary>
        public Client Client { private set; get; }
    }
}

