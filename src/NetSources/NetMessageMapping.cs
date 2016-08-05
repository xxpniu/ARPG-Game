using System;
using System.Collections.Generic;

namespace Proto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class MessageHandleAttribute : Attribute
    {        
        public MessageHandleAttribute(Type type, int handleId)
        {
            Type = type;
            HandleID = handleId;
        }

        public Type Type { set; get; }

        public int HandleID { set; get; }
    }


    [MessageHandle(typeof(C2S_Login),-1025718538)]
    [MessageHandle(typeof(S2C_login),-1007277546)]

    public sealed class MessageHandleTypes
    {
        public MessageHandleTypes()
        {
            
        }

        public static Type GetTypeByIndex(int index)
        {
            var type = typeof(MessageHandleTypes);
            var handles = type.GetCustomAttributes(typeof(MessageHandleAttribute), false) as MessageHandleAttribute[];
            foreach (var i in handles)
            {
                if (i.HandleID == index) return i.Type;
            }
            return null;
        }

        public static bool GetTypeIndex(Type t,out int index)
        { 
            var type = typeof(MessageHandleTypes);
            index = -1;
            var handles = type.GetCustomAttributes(typeof(MessageHandleAttribute), false) as MessageHandleAttribute[];
            foreach (var i in handles)
            {
                if (i.Type.Name == t.Name)
                {
                    index = i.HandleID;
                    return true;
                }
            }
            return false;
        }
    }
}
