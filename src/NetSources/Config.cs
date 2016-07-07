
/*
#############################################
       
       *此代码为工具自动生成
       *请勿单独修改该文件

#############################################
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;
namespace ExcelConfig
{

    /// <summary>
    /// 角色数据表
    /// </summary>
    [ConfigFile("CharacterData.json","CharacterData")]
    public class CharacterData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 资源目录
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String ResourcesPath { set; get; }
        
        /// <summary>
        /// 血量
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int HPMax { set; get; }
        
        /// <summary>
        /// 伤害小
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int DamageMin { set; get; }
        
        /// <summary>
        /// 伤害大
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int DamageMax { set; get; }
        
        /// <summary>
        /// 攻击力
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int Attack { set; get; }
        
        /// <summary>
        /// 防御力
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int Defance { set; get; }
        
        /// <summary>
        /// 等级
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int Level { set; get; }
        
        /// <summary>
        /// 种族
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int BodyType { set; get; }
        
        /// <summary>
        /// 防御类型
        /// </summary>
        [ExcelConfigColIndex(10)]
        public int DefanceType { set; get; }
        
        /// <summary>
        /// 攻击类型
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int AttackType { set; get; }
        
        /// <summary>
        /// 攻击类型
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int DamageType { set; get; }

    }

    /// <summary>
    /// 角色技能
    /// </summary>
    [ConfigFile("CharacterMagicData.json","CharacterMagicData")]
    public class CharacterMagicData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int CharacterID { set; get; }
        
        /// <summary>
        /// 魔法Key
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String MagicKey { set; get; }
        
        /// <summary>
        /// CoolDown时间秒
        /// </summary>
        [ExcelConfigColIndex(3)]
        public float TickTime { set; get; }

    }

 }
