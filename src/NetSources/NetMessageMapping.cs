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
    [MessageHandle(typeof(Notify_CharacterPosition),1265737176)]
    [MessageHandle(typeof(Notify_LayoutPlayMotion),187257874)]
    [MessageHandle(typeof(Notify_LookAtCharacter),-445529701)]
    [MessageHandle(typeof(Notify_LayoutPlayParticle),-1563665098)]
    [MessageHandle(typeof(Notify_PropertyValue),2023471666)]
    [MessageHandle(typeof(Notity_HPChange),958027706)]
    [MessageHandle(typeof(Notify_MPChange),355866351)]
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
    [MessageHandle(typeof(C2B_ExitGame),-1582110174)]
    [MessageHandle(typeof(B2C_ExitGame),-1691428286)]
    [MessageHandle(typeof(Task_G2C_JoinBattle),1212697435)]
    [MessageHandle(typeof(C2B_JoinBattle),1752308250)]
    [MessageHandle(typeof(B2C_JoinBattle),-1845989638)]
    [MessageHandle(typeof(Task_B2C_ExitBattle),970004100)]
    [MessageHandle(typeof(C2B_ExitBattle),708095254)]
    [MessageHandle(typeof(B2C_ExitBattle),1395564790)]
    [MessageHandle(typeof(C2G_GetLastBattle),613536927)]
    [MessageHandle(typeof(G2C_GetLastBattle),61301791)]
    [MessageHandle(typeof(Action_ClickMapGround),-627978412)]
    [MessageHandle(typeof(Action_ClickSkillIndex),-753000692)]
    [MessageHandle(typeof(Task_L2B_StartBattle),1944232767)]
    [MessageHandle(typeof(Task_L2B_ExitUser),-35661362)]
    [MessageHandle(typeof(G2L_Reg),-1856389636)]
    [MessageHandle(typeof(L2G_Reg),-1856145636)]
    [MessageHandle(typeof(G2L_CheckUserSession),-1274860641)]
    [MessageHandle(typeof(L2G_CheckUserSession),-1451555649)]
    [MessageHandle(typeof(G2L_BeginBattle),-542792341)]
    [MessageHandle(typeof(L2G_BeginBattle),1066090379)]
    [MessageHandle(typeof(G2L_GetLastBattle),402296512)]
    [MessageHandle(typeof(L2G_GetLastBattle),223488416)]
    [MessageHandle(typeof(B2L_RegBattleServer),-77139858)]
    [MessageHandle(typeof(L2B_RegBattleServer),716002094)]
    [MessageHandle(typeof(B2L_EndBattle),1998286470)]
    [MessageHandle(typeof(L2B_EndBattle),1078880454)]
    [MessageHandle(typeof(B2L_CheckSession),-656558221)]
    [MessageHandle(typeof(L2B_CheckSession),-1235510989)]
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
