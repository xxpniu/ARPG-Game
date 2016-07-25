
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
        /// AI路径
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String AIResourcePath { set; get; }
        
        /// <summary>
        /// 伤害参考值
        /// </summary>
        [ExcelConfigColIndex(4)]
        public float DPS { set; get; }
        
        /// <summary>
        /// 生存能力参考值
        /// </summary>
        [ExcelConfigColIndex(5)]
        public float IV { set; get; }
        
        /// <summary>
        /// 消耗比值
        /// </summary>
        [ExcelConfigColIndex(6)]
        public float EV { set; get; }
        
        /// <summary>
        /// 消耗
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int Cost { set; get; }
        
        /// <summary>
        /// 攻击速度(间隔秒)
        /// </summary>
        [ExcelConfigColIndex(8)]
        public float AttackSpeed { set; get; }
        
        /// <summary>
        /// 移动速度（m/s）
        /// </summary>
        [ExcelConfigColIndex(9)]
        public float MoveSpeed { set; get; }
        
        /// <summary>
        /// 避让优先级
        /// </summary>
        [ExcelConfigColIndex(10)]
        public float PriorityMove { set; get; }
        
        /// <summary>
        /// 血量
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int HPMax { set; get; }
        
        /// <summary>
        /// 伤害小
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int DamageMin { set; get; }
        
        /// <summary>
        /// 伤害大
        /// </summary>
        [ExcelConfigColIndex(13)]
        public int DamageMax { set; get; }
        
        /// <summary>
        /// 攻击力
        /// </summary>
        [ExcelConfigColIndex(14)]
        public int Attack { set; get; }
        
        /// <summary>
        /// 防御力
        /// </summary>
        [ExcelConfigColIndex(15)]
        public int Defance { set; get; }
        
        /// <summary>
        /// 等级
        /// </summary>
        [ExcelConfigColIndex(16)]
        public int Level { set; get; }
        
        /// <summary>
        /// 种族
        /// </summary>
        [ExcelConfigColIndex(17)]
        public int BodyType { set; get; }
        
        /// <summary>
        /// 防御类型
        /// </summary>
        [ExcelConfigColIndex(18)]
        public int DefanceType { set; get; }
        
        /// <summary>
        /// 攻击类型
        /// </summary>
        [ExcelConfigColIndex(19)]
        public int AttackType { set; get; }
        
        /// <summary>
        /// 攻击类型
        /// </summary>
        [ExcelConfigColIndex(20)]
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
        /// 释放最小距离
        /// </summary>
        [ExcelConfigColIndex(3)]
        public float ReleaseRangeMin { set; get; }
        
        /// <summary>
        /// 释放距离（释放最大距离）
        /// </summary>
        [ExcelConfigColIndex(4)]
        public float ReleaseRangeMax { set; get; }
        
        /// <summary>
        /// 释放类型
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int ReleaseType { set; get; }
        
        /// <summary>
        /// 释放参数
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String ReleaseParams { set; get; }
        
        /// <summary>
        /// CoolDown时间秒
        /// </summary>
        [ExcelConfigColIndex(7)]
        public float TickTime { set; get; }

    }

    /// <summary>
    /// 道具表
    /// </summary>
    [ConfigFile("ItemData.json","ItemData")]
    public class ItemData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 类别
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int ItemType { set; get; }
        
        /// <summary>
        /// 图标
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Icon { set; get; }

    }

    /// <summary>
    /// 关卡数据表
    /// </summary>
    [ConfigFile("LevelData.json","LevelData")]
    public class LevelData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 解锁条件
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int UnlockType { set; get; }
        
        /// <summary>
        /// 解锁参数
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String UnlockParams { set; get; }
        
        /// <summary>
        /// 关卡名称
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String LevelResouceName { set; get; }
        
        /// <summary>
        /// 最大
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int MaxPoint { set; get; }
        
        /// <summary>
        /// 初始能量
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int PointBegin { set; get; }
        
        /// <summary>
        /// 难度系数
        /// </summary>
        [ExcelConfigColIndex(7)]
        public float DV { set; get; }
        
        /// <summary>
        /// 怪物初始化能量
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int MonsterPoint { set; get; }
        
        /// <summary>
        /// 能量获取比例
        /// </summary>
        [ExcelConfigColIndex(9)]
        public float PointGetRate { set; get; }
        
        /// <summary>
        /// 怪物获取能量比
        /// </summary>
        [ExcelConfigColIndex(10)]
        public float MonsterGetRate { set; get; }
        
        /// <summary>
        /// 玩家单位时间获取能量
        /// </summary>
        [ExcelConfigColIndex(11)]
        public float PointLeftAdd { set; get; }
        
        /// <summary>
        /// 单位时间增加能量值（每秒）
        /// </summary>
        [ExcelConfigColIndex(12)]
        public float PointRightAdd { set; get; }
        
        /// <summary>
        /// 限制时间秒
        /// </summary>
        [ExcelConfigColIndex(13)]
        public int LimitTime { set; get; }
        
        /// <summary>
        /// AI出兵间隔时间
        /// </summary>
        [ExcelConfigColIndex(14)]
        public float Limit { set; get; }
        
        /// <summary>
        /// 出兵逻辑概率和兵种
        /// </summary>
        [ExcelConfigColIndex(15)]
        public String AILogic { set; get; }
        
        /// <summary>
        /// 箭塔
        /// </summary>
        [ExcelConfigColIndex(16)]
        public int TowerID { set; get; }

    }

 }
