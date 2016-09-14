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
    [MessageHandle(typeof(Notify_HPChange),1625804424)]
    [MessageHandle(typeof(Notify_MPChange),355866351)]
    [MessageHandle(typeof(Notify_DamageResult),-1022173276)]
    [MessageHandle(typeof(Notify_Drop),-165546567)]
    [MessageHandle(typeof(Notify_PlayerJoinState),-1802543358)]
    [MessageHandle(typeof(Notify_ReleaseMagic),-162101436)]
    [MessageHandle(typeof(C2L_Login),-1026118699)]
    [MessageHandle(typeof(L2C_Login),-1022666443)]
    [MessageHandle(typeof(C2L_Reg),-1856254592)]
    [MessageHandle(typeof(L2C_Reg),-1856149984)]
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
    [MessageHandle(typeof(C2B_ExitBattle),708095254)]
    [MessageHandle(typeof(B2C_ExitBattle),1395564790)]
    [MessageHandle(typeof(C2G_GetLastBattle),613536927)]
    [MessageHandle(typeof(G2C_GetLastBattle),61301791)]
    [MessageHandle(typeof(Action_ClickMapGround),-627978412)]
    [MessageHandle(typeof(Action_ClickSkillIndex),-753000692)]
    [MessageHandle(typeof(Action_AutoFindTarget),-114336352)]
    [MessageHandle(typeof(Task_G2C_SyncPackage),-968313908)]
    [MessageHandle(typeof(Task_G2C_SyncHero),-1719286116)]
    [MessageHandle(typeof(C2G_OperatorEquip),1046861353)]
    [MessageHandle(typeof(G2C_OperatorEquip),494626217)]
    [MessageHandle(typeof(C2G_SaleItem),582797435)]
    [MessageHandle(typeof(G2C_SaleItem),430999547)]
    [MessageHandle(typeof(C2G_EquipmentLevelUp),1534855086)]
    [MessageHandle(typeof(G2C_EquipmentLevelUp),490964782)]
    [MessageHandle(typeof(C2G_GMTool),1327314459)]
    [MessageHandle(typeof(G2C_GMTool),1322714523)]
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
