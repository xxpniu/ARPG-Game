using System;
using System.Collections.Generic;

namespace Proto
{

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public class IndexAttribute:Attribute
    {
         public IndexAttribute(int index,Type tOm) 
         {
            this.Index = index;
            this.TypeOfMessage = tOm;
         }

        public int Index { set; get; }

        public Type TypeOfMessage { set; get; }
    }

    [Index(10001,typeof(Notify_CharacterAlpha))]
    [Index(10002,typeof(Notify_CharacterPosition))]
    [Index(10003,typeof(Notify_CreateBattleCharacter))]
    [Index(10004,typeof(Notify_CreateMissile))]
    [Index(10005,typeof(Notify_CreateReleaser))]
    [Index(10006,typeof(Notify_DamageResult))]
    [Index(10007,typeof(Notify_Drop))]
    [Index(10008,typeof(Notify_ElementExitState))]
    [Index(10009,typeof(Notify_ElementJoinState))]
    [Index(10010,typeof(Notify_HPChange))]
    [Index(10011,typeof(Notify_LayoutPlayMotion))]
    [Index(10012,typeof(Notify_LayoutPlayParticle))]
    [Index(10013,typeof(Notify_LookAtCharacter))]
    [Index(10014,typeof(Notify_MPChange))]
    [Index(10015,typeof(Notify_PlayerJoinState))]
    [Index(10016,typeof(Notify_PropertyValue))]
    [Index(10017,typeof(Notify_ReleaseMagic))]
    [Index(10018,typeof(Action_ClickMapGround))]
    [Index(10019,typeof(Action_ClickSkillIndex))]
    [Index(10020,typeof(Action_AutoFindTarget))]
    [Index(10021,typeof(Action_MoveDir))]
    [Index(10022,typeof(C2B_ExitBattle))]
    [Index(10023,typeof(B2C_ExitBattle))]
    [Index(10024,typeof(C2B_ExitGame))]
    [Index(10025,typeof(B2C_ExitGame))]
    [Index(10026,typeof(C2B_JoinBattle))]
    [Index(10027,typeof(B2C_JoinBattle))]
    [Index(10028,typeof(C2L_Login))]
    [Index(10029,typeof(L2C_Login))]
    [Index(10030,typeof(C2L_Reg))]
    [Index(10031,typeof(L2C_Reg))]
    [Index(10032,typeof(B2L_RegBattleServer))]
    [Index(10033,typeof(L2B_RegBattleServer))]
    [Index(10034,typeof(B2L_EndBattle))]
    [Index(10035,typeof(L2B_EndBattle))]
    [Index(10036,typeof(B2L_CheckSession))]
    [Index(10037,typeof(L2B_CheckSession))]
    [Index(10038,typeof(G2L_GateServerReg))]
    [Index(10039,typeof(L2G_GateServerReg))]
    [Index(10040,typeof(G2L_GateCheckSession))]
    [Index(10041,typeof(L2G_GateCheckSession))]
    [Index(10042,typeof(G2L_BeginBattle))]
    [Index(10043,typeof(L2G_BeginBattle))]
    [Index(10044,typeof(G2L_GetLastBattle))]
    [Index(10045,typeof(L2G_GetLastBattle))]
    [Index(10046,typeof(Task_L2B_ExitUser))]
    [Index(10047,typeof(C2G_Login))]
    [Index(10048,typeof(G2C_Login))]
    [Index(10049,typeof(C2G_CreateHero))]
    [Index(10050,typeof(G2C_CreateHero))]
    [Index(10051,typeof(C2G_BeginGame))]
    [Index(10052,typeof(G2C_BeginGame))]
    [Index(10053,typeof(C2G_GetLastBattle))]
    [Index(10054,typeof(G2C_GetLastBattle))]
    [Index(10055,typeof(C2G_OperatorEquip))]
    [Index(10056,typeof(G2C_OperatorEquip))]
    [Index(10057,typeof(C2G_SaleItem))]
    [Index(10058,typeof(G2C_SaleItem))]
    [Index(10059,typeof(C2G_EquipmentLevelUp))]
    [Index(10060,typeof(G2C_EquipmentLevelUp))]
    [Index(10061,typeof(C2G_GMTool))]
    [Index(10062,typeof(G2C_GMTool))]
    [Index(10063,typeof(Task_G2C_SyncPackage))]
    [Index(10064,typeof(Task_G2C_SyncHero))]
    [Index(10065,typeof(Task_G2C_JoinBattle))]
    [Index(10066,typeof(B2G_GetPlayerInfo))]
    [Index(10067,typeof(G2B_GetPlayerInfo))]
    [Index(10068,typeof(B2G_BattleReward))]
    [Index(10069,typeof(G2B_BattleReward))]

    public static class MessageTypeIndexs
    {
        private static readonly Dictionary<int, Type> types = new Dictionary<int, Type>();

        private static readonly Dictionary<Type, int> indexs = new Dictionary<Type, int>();
        
        static MessageTypeIndexs()
        {
            var tys = typeof(MessageTypeIndexs).GetCustomAttributes(typeof(IndexAttribute), false) as IndexAttribute[];

            foreach(var t in tys)
            {
                types.Add(t.Index, t.TypeOfMessage);
                indexs.Add(t.TypeOfMessage, t.Index);
            }
        }

        /// <summary>
        /// Tries the index of the get.
        /// </summary>
        /// <returns><c>true</c>, if get index was tryed, <c>false</c> otherwise.</returns>
        /// <param name="type">Type.</param>
        /// <param name="index">Index.</param>
        public static bool TryGetIndex(Type type,out int index)
        {
            return indexs.TryGetValue(type, out index);
        }
        /// <summary>
        /// Tries the type of the get.
        /// </summary>
        /// <returns><c>true</c>, if get type was tryed, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        /// <param name="type">Type.</param>
        public static bool TryGetType(int index,out Type type)
        {
            return types.TryGetValue(index, out type);
        }
    }
}
