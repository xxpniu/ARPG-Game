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
    [MessageHandle(typeof(Notify_CreateMissile),1729374952)]
    [MessageHandle(typeof(Notify_CharacterBeginMove),924544451)]
    [MessageHandle(typeof(Notify_CharacterStopMove),-105613934)]
    [MessageHandle(typeof(Notify_LayoutPlayMotion),187257874)]
    [MessageHandle(typeof(Notify_LookAtCharacter),-445529701)]
    [MessageHandle(typeof(Notify_LayoutPlayParticle),-1563665098)]
    [MessageHandle(typeof(Notify_PropertyValue),2023471666)]
    [MessageHandle(typeof(Notity_EffectSubHP),586760837)]
    [MessageHandle(typeof(Notity_EffectAddHP),-2048462950)]
    [MessageHandle(typeof(Notify_EffectSubMP),1096701168)]
    [MessageHandle(typeof(Notity_EffectAddMP),-2048462955)]
    [MessageHandle(typeof(Notify_DamageResult),-1022173276)]
    [MessageHandle(typeof(C2S_Login),-1025718538)]
    [MessageHandle(typeof(S2C_Login),-1007318794)]
    [MessageHandle(typeof(C2S_Reg),-1856243423)]
    [MessageHandle(typeof(S2C_Reg),-1855685855)]
    [MessageHandle(typeof(C2G_Login),-1026428822)]
    [MessageHandle(typeof(G2C_Login),-1031028758)]
    [MessageHandle(typeof(C2G_CreateHero),872482605)]
    [MessageHandle(typeof(G2C_CreateHero),158119597)]
    [MessageHandle(typeof(C2G_BeginGame),272313428)]
    [MessageHandle(typeof(G2C_BeginGame),-442049580)]
    [MessageHandle(typeof(C2G_ExitGame),-1580854745)]
    [MessageHandle(typeof(G2C_ExitGame),-1732652633)]
    [MessageHandle(typeof(Task_G2C_JoinBattle),1212697435)]
    [MessageHandle(typeof(Task_L2B_StartBattle),1944232767)]
    [MessageHandle(typeof(Task_G2B_EndBattle),1337194827)]
    [MessageHandle(typeof(Task_L2B_JoinBattle),1251549209)]
    [MessageHandle(typeof(G2L_Reg),-1856389636)]
    [MessageHandle(typeof(L2G_Reg),-1856145636)]
    [MessageHandle(typeof(G2L_UnReg),1792431837)]
    [MessageHandle(typeof(L2G_UnReg),1800486013)]
    [MessageHandle(typeof(G2L_CheckUserSession),-1274860641)]
    [MessageHandle(typeof(L2G_CheckUserSession),-1451555649)]
    [MessageHandle(typeof(G2L_BeginBattle),-542792341)]
    [MessageHandle(typeof(L2G_BeginBattle),1066090379)]
    [MessageHandle(typeof(B2L_RegBattleServer),-77139858)]
    [MessageHandle(typeof(L2B_RegBattleServer),716002094)]
    [MessageHandle(typeof(B2G_GetPlayerInfo),1494462755)]
    [MessageHandle(typeof(G2B_GetPlayerInfo),-459223933)]
    [MessageHandle(typeof(B2G_BattleReward),-428989659)]
    [MessageHandle(typeof(G2B_BattleReward),249211141)]

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
