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


    [MessageHandle(typeof(Notify_ElementJoinState),-208268113)]
    [MessageHandle(typeof(Notify_ElementExitState),-323688811)]
    [MessageHandle(typeof(Notify_CreateReleaser),1761963755)]
    [MessageHandle(typeof(Notify_CreateBattleCharacter),561773071)]
    [MessageHandle(typeof(Notity_CreateMissile),829996726)]
    [MessageHandle(typeof(Notity_CharacterBeginMove),31078321)]
    [MessageHandle(typeof(Notity_CharacterStopMove),1169687876)]
    [MessageHandle(typeof(Notity_LayoutPlayMotion),-1048880412)]
    [MessageHandle(typeof(Notity_LookAtCharacter),979620905)]
    [MessageHandle(typeof(Notity_LayoutPlayParticle),593561508)]
    [MessageHandle(typeof(Notity_EffectSubHP),586760837)]
    [MessageHandle(typeof(Notity_EffectAddHP),-2048462950)]
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
