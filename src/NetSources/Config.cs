
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
    /// 角色数据表jue se
    /// </summary>
    [ConfigFile("CharacterData.json","CharacterData")]
    public class CharacterData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 资源目录zi yuanmu lu
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String ResourcesPath { set; get; }
        
        /// <summary>
        /// AI路径lu jing
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String AIResourcePath { set; get; }
        
        /// <summary>
        /// 消耗xiao hao
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int Cost { set; get; }
        
        /// <summary>
        /// 攻击速度(间隔秒)gong jisu dujian ge
        /// </summary>
        [ExcelConfigColIndex(5)]
        public float AttackSpeed { set; get; }
        
        /// <summary>
        /// 移动速度（m/s）yi dongsu du
        /// </summary>
        [ExcelConfigColIndex(6)]
        public float MoveSpeed { set; get; }
        
        /// <summary>
        /// 避让优先级bi rangyou xian ji
        /// </summary>
        [ExcelConfigColIndex(7)]
        public float PriorityMove { set; get; }
        
        /// <summary>
        /// 血量xue liang
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int HPMax { set; get; }
        
        /// <summary>
        /// 伤害小shang haixiao
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int DamageMin { set; get; }
        
        /// <summary>
        /// 伤害大shang haida
        /// </summary>
        [ExcelConfigColIndex(10)]
        public int DamageMax { set; get; }
        
        /// <summary>
        /// 攻击力gong ji li
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int Attack { set; get; }
        
        /// <summary>
        /// 防御力fang yu li
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int Defance { set; get; }
        
        /// <summary>
        /// 等级deng ji
        /// </summary>
        [ExcelConfigColIndex(13)]
        public int Level { set; get; }
        
        /// <summary>
        /// 种族zhong zu
        /// </summary>
        [ExcelConfigColIndex(14)]
        public int BodyType { set; get; }
        
        /// <summary>
        /// 防御类型fang yulei xing
        /// </summary>
        [ExcelConfigColIndex(15)]
        public int DefanceType { set; get; }
        
        /// <summary>
        /// 攻击类型gong jilei xing
        /// </summary>
        [ExcelConfigColIndex(16)]
        public int AttackType { set; get; }
        
        /// <summary>
        /// 攻击类型gong jilei xing
        /// </summary>
        [ExcelConfigColIndex(17)]
        public int DamageType { set; get; }

    }

    /// <summary>
    /// 角色技能jue seji neng
    /// </summary>
    [ConfigFile("CharacterMagicData.json","CharacterMagicData")]
    public class CharacterMagicData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int CharacterID { set; get; }
        
        /// <summary>
        /// 魔法Keymo fa
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String MagicKey { set; get; }
        
        /// <summary>
        /// 释放最小距离shi fangzui daxiaoju li
        /// </summary>
        [ExcelConfigColIndex(3)]
        public float ReleaseRangeMin { set; get; }
        
        /// <summary>
        /// 释放距离（释放最大距离）shi fangju lishi fangzui daju li
        /// </summary>
        [ExcelConfigColIndex(4)]
        public float ReleaseRangeMax { set; get; }
        
        /// <summary>
        /// 释放类型shi fanglei xing
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int ReleaseType { set; get; }
        
        /// <summary>
        /// 释放参数shi fangcan shu
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String ReleaseParams { set; get; }
        
        /// <summary>
        /// CoolDown时间秒shi jianmiao
        /// </summary>
        [ExcelConfigColIndex(7)]
        public float TickTime { set; get; }

    }

    /// <summary>
    /// 关卡数据表guan kashu jubiao
    /// </summary>
    [ConfigFile("LevelData.json","LevelData")]
    public class LevelData:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 关卡名称guan kaming cheng
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String LevelResouceName { set; get; }
        
        /// <summary>
        /// 单位时间增加能量值dan weishi jianzeng jianeng liangzhi
        /// </summary>
        [ExcelConfigColIndex(3)]
        public float PointAddSpeed { set; get; }
        
        /// <summary>
        /// 限制时间秒xian zhishi jianmiao
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int LimitTime { set; get; }
        
        /// <summary>
        /// 出兵逻辑chu bingluo ji
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String LevelAI { set; get; }
        
        /// <summary>
        /// 箭塔jian ta
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int TowerID { set; get; }

    }

 }
