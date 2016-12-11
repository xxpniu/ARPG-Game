using System;
namespace GameLogic.Utility
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple =  false) ]
    public class NeedNotifyAttribute : Attribute
    {
        public string IndexFieldName { private set; get; }
        public Type NotifyType{ private set; get; }
        public string[] FieldNames { private set; get; }

        public NeedNotifyAttribute(Type notifyType, string indexFieldName, params string[] pars)
        {
            NotifyType = notifyType;
            IndexFieldName = indexFieldName;
            FieldNames = pars;
        }

        public NeedNotifyAttribute(Type notifyType, params string[] pars):this(notifyType,"Index", pars)
        {
            
        }
    }
}
