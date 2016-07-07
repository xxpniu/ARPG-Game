
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
    /// 钻石商城
    /// </summary>
    [ConfigFile("ChargInfoConfig.json","ChargInfoConfig")]
    public class ChargInfoConfig:JSONConfigBase    {
        
        /// <summary>
        /// 充值金额
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int ChargNum { set; get; }
        
        /// <summary>
        /// 平台钻石兑换
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int BaseDiamondCharge { set; get; }
        
        /// <summary>
        /// 游戏内折返钻石
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int IapReward { set; get; }

    }

    /// <summary>
    /// 怪物类型表
    /// </summary>
    [ConfigFile("MonsterCategoryConfig.json","MonsterCategoryConfig")]
    public class MonsterCategoryConfig:JSONConfigBase    {
        
        /// <summary>
        /// 类型说明
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 克制类别
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int AddCategory { set; get; }
        
        /// <summary>
        /// 克制效果值
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int AddRate { set; get; }
        
        /// <summary>
        /// 被克制类别
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int DefType { set; get; }
        
        /// <summary>
        /// 被克制效果
        /// </summary>
        [ExcelConfigColIndex(5)]
        public float DefRate { set; get; }
        
        /// <summary>
        /// 图标
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String IconName { set; get; }

    }

    /// <summary>
    /// 兵种升级表
    /// </summary>
    [ConfigFile("MonsterLvlUpConfig.json","MonsterLvlUpConfig")]
    public class MonsterLvlUpConfig:JSONConfigBase    {
        
        /// <summary>
        /// 进化前ID
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int OldMonster { set; get; }
        
        /// <summary>
        /// 进化后ID
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int LateMonster { set; get; }
        
        /// <summary>
        /// 消耗金币量
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int CostGold { set; get; }
        
        /// <summary>
        /// 进化消耗物品
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String CostItems { set; get; }
        
        /// <summary>
        /// 物品消耗数量
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String CostItemsNumber { set; get; }

    }

    /// <summary>
    /// 制作表
    /// </summary>
    [ConfigFile("MakeConfig.json","MakeConfig")]
    public class MakeConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 需要道具ID
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String RequireItems { set; get; }
        
        /// <summary>
        /// 需要道具数量
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String RequireItemsNumber { set; get; }
        
        /// <summary>
        /// 获得道具
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String RewardItems { set; get; }
        
        /// <summary>
        /// 获得道具数量
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String RewardItemsNumber { set; get; }
        
        /// <summary>
        /// 需要金币
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int RequireGold { set; get; }
        
        /// <summary>
        /// 解锁条件
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int UnlockType { set; get; }
        
        /// <summary>
        /// 解锁参数
        /// </summary>
        [ExcelConfigColIndex(8)]
        public String UnlockPars1 { set; get; }
        
        /// <summary>
        /// 制作上限
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int MaxProduct { set; get; }

    }

    /// <summary>
    /// 博彩表
    /// </summary>
    [ConfigFile("GambleConfig.json","GambleConfig")]
    public class GambleConfig:JSONConfigBase    {
        
        /// <summary>
        /// 博弈比例
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int GambleRatio { set; get; }
        
        /// <summary>
        /// 该比例出现概率
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int HappenRatio { set; get; }
        
        /// <summary>
        /// 概率随次数修正
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int Fixratio { set; get; }
        
        /// <summary>
        /// 钻石出现比例
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int GemAddRatio { set; get; }
        
        /// <summary>
        /// 固定钻石奖励值
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int RewardGem { set; get; }
        
        /// <summary>
        /// 每日首次不计入
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int IgnorRatio { set; get; }

    }

    /// <summary>
    /// 地图内黑店
    /// </summary>
    [ConfigFile("SecretStoreConfig.json","SecretStoreConfig")]
    public class SecretStoreConfig:JSONConfigBase    {
        
        /// <summary>
        /// 黑店名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 商店编号
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int Store_id { set; get; }
        
        /// <summary>
        /// 商店所属地图
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Belong_map { set; get; }
        
        /// <summary>
        /// 商品id
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int item_id { set; get; }
        
        /// <summary>
        /// 商品名称
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String item_name { set; get; }
        
        /// <summary>
        /// 购买货币类型
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int current_type { set; get; }
        
        /// <summary>
        /// 购买价格
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int Sold_price { set; get; }
        
        /// <summary>
        /// 购买堆叠数
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int Purchase_pile { set; get; }
        
        /// <summary>
        /// 最大购买次数
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int Max_purchase_times { set; get; }

    }

    /// <summary>
    /// 地图子表
    /// </summary>
    [ConfigFile("SubMapConfig.json","SubMapConfig")]
    public class SubMapConfig:JSONConfigBase    {
        
        /// <summary>
        /// 子地图ID
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int MapID { set; get; }
        
        /// <summary>
        /// 事件节点名称
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Name { set; get; }
        
        /// <summary>
        /// 事件类型
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int EventType { set; get; }
        
        /// <summary>
        /// 前置战斗节点编号
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int BattleID { set; get; }
        
        /// <summary>
        /// 事件坐标集合
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String Posistions { set; get; }
        
        /// <summary>
        /// 单个坐标探索值
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int PerLocationMath { set; get; }
        
        /// <summary>
        /// 事件参数1
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String Pars1 { set; get; }
        
        /// <summary>
        /// 事件参数2
        /// </summary>
        [ExcelConfigColIndex(8)]
        public String Pars2 { set; get; }
        
        /// <summary>
        /// 事件参数3
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String Pars3 { set; get; }
        
        /// <summary>
        /// 事件参数4
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String Pars4 { set; get; }

    }

    /// <summary>
    /// 地图表
    /// </summary>
    [ConfigFile("MapConfig.json","MapConfig")]
    public class MapConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 地图关卡资源名
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String MapResName { set; get; }
        
        /// <summary>
        /// 过图条件
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int OpenCondtion { set; get; }
        
        /// <summary>
        /// 条件参数
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String OpenParams { set; get; }
        
        /// <summary>
        /// 解锁方式
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int UnlockMode { set; get; }
        
        /// <summary>
        /// 解锁参数
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String UnlockParams { set; get; }
        
        /// <summary>
        /// 是否开放
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int IsOpen { set; get; }
        
        /// <summary>
        /// 地图刷新周期
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int RefreshTicks { set; get; }
        
        /// <summary>
        /// 成就点
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int Point { set; get; }
        
        /// <summary>
        /// 从属组集合
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String MapGroup { set; get; }
        
        /// <summary>
        /// 随机战斗触发概率
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int RandomPro { set; get; }
        
        /// <summary>
        /// 地图完整探索度
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int CompletePoint { set; get; }
        
        /// <summary>
        /// 背景音乐
        /// </summary>
        [ExcelConfigColIndex(13)]
        public String BackgroudMusic { set; get; }

    }

    /// <summary>
    /// 天赋表
    /// </summary>
    [ConfigFile("TalentConfig.json","TalentConfig")]
    public class TalentConfig:JSONConfigBase    {
        
        /// <summary>
        /// 天赋名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 天赋说明
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Desctription { set; get; }
        
        /// <summary>
        /// 天赋获取方式
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int TalentGet { set; get; }
        
        /// <summary>
        /// 获取参数
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int GetPars { set; get; }
        
        /// <summary>
        /// 天赋类别
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int TalentType { set; get; }
        
        /// <summary>
        /// 参数1
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Pars1 { set; get; }
        
        /// <summary>
        /// 参数2
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String Pars2 { set; get; }
        
        /// <summary>
        /// 参数3
        /// </summary>
        [ExcelConfigColIndex(8)]
        public String Pars3 { set; get; }
        
        /// <summary>
        /// 参数4
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String Pars4 { set; get; }
        
        /// <summary>
        /// 参数5
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String Pars5 { set; get; }
        
        /// <summary>
        /// 参数6
        /// </summary>
        [ExcelConfigColIndex(11)]
        public String Pars6 { set; get; }
        
        /// <summary>
        /// 参数7
        /// </summary>
        [ExcelConfigColIndex(12)]
        public String Pars7 { set; get; }

    }

    /// <summary>
    /// 常量表
    /// </summary>
    [ConfigFile("ConstAttConfig.json","ConstAttConfig")]
    public class ConstAttConfig:JSONConfigBase    {
        
        /// <summary>
        /// 计算时间单位毫秒
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int ProduceRewardTick { set; get; }
        
        /// <summary>
        /// 等级一的黄金参量
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int GoldProduceLvl1 { set; get; }
        
        /// <summary>
        /// cd时间
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int GoldProduceLvl1CD { set; get; }
        
        /// <summary>
        /// 复活道具ID
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int ReliveNeedItem { set; get; }
        
        /// <summary>
        /// 默认开放地图ID
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int DefaultMapID { set; get; }
        
        /// <summary>
        /// 默认的包裹大小
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int DefaultPackageSize { set; get; }
        
        /// <summary>
        /// 游戏中的干粮道具ID
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int FoodItemID { set; get; }
        
        /// <summary>
        /// 默认可出战英雄
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int DefaultTeamSize { set; get; }
        
        /// <summary>
        /// 回城卷轴ID
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int JoinCastleItemID { set; get; }
        
        /// <summary>
        /// 离线收益时间毫秒
        /// </summary>
        [ExcelConfigColIndex(10)]
        public int OutLineRewTime { set; get; }
        
        /// <summary>
        /// 初始可视范围
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int OrignalVisual { set; get; }
        
        /// <summary>
        /// 博弈次数
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int GamblePerTime { set; get; }
        
        /// <summary>
        /// 每日分享奖励钻石
        /// </summary>
        [ExcelConfigColIndex(13)]
        public int ShareRewardGem { set; get; }
        
        /// <summary>
        /// 初始移动格数
        /// </summary>
        [ExcelConfigColIndex(14)]
        public int OrignalMoveStep { set; get; }
        
        /// <summary>
        /// 秒 随机事件间隔时间
        /// </summary>
        [ExcelConfigColIndex(15)]
        public int RandEventTime { set; get; }
        
        /// <summary>
        /// 零干粮移动距离
        /// </summary>
        [ExcelConfigColIndex(16)]
        public int OutOffeedMoving { set; get; }

    }

    /// <summary>
    /// 建筑表
    /// </summary>
    [ConfigFile("BuildingConfig.json","BuildingConfig")]
    public class BuildingConfig:JSONConfigBase    {
        
        /// <summary>
        /// 建筑名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 建筑组标识
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int BuildingId { set; get; }
        
        /// <summary>
        /// 资源图标
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Icon { set; get; }
        
        /// <summary>
        /// 建筑等级
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int Level { set; get; }
        
        /// <summary>
        /// 升级触发事件
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String ConstructEvent { set; get; }
        
        /// <summary>
        /// 事件参数1
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Pars1 { set; get; }
        
        /// <summary>
        /// 事件参数2
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String Pars2 { set; get; }
        
        /// <summary>
        /// 金币消耗
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int CostGold { set; get; }
        
        /// <summary>
        /// 建造材料消耗
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String CostItems { set; get; }
        
        /// <summary>
        /// 材料消耗数量
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String CostItemCounts { set; get; }
        
        /// <summary>
        /// 解锁类型
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int UnlockType { set; get; }
        
        /// <summary>
        /// 解锁参数1
        /// </summary>
        [ExcelConfigColIndex(12)]
        public String UnlockParms1 { set; get; }
        
        /// <summary>
        /// 解锁参数2
        /// </summary>
        [ExcelConfigColIndex(13)]
        public String UnlockParms2 { set; get; }
        
        /// <summary>
        /// 建筑描述
        /// </summary>
        [ExcelConfigColIndex(14)]
        public String Describe { set; get; }
        
        /// <summary>
        /// 升级后提示
        /// </summary>
        [ExcelConfigColIndex(15)]
        public String BuildSuccessMessage { set; get; }

    }

    /// <summary>
    /// 怪物表
    /// </summary>
    [ConfigFile("MonsterConfig.json","MonsterConfig")]
    public class MonsterConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 资源名称
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String ResName { set; get; }
        
        /// <summary>
        /// 等级
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int Level { set; get; }
        
        /// <summary>
        /// NPC描述
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String Description { set; get; }
        
        /// <summary>
        /// 兵种类型
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int Type { set; get; }
        
        /// <summary>
        /// 兵星
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int Star { set; get; }
        
        /// <summary>
        /// 生命
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int Hp { set; get; }
        
        /// <summary>
        /// 攻击
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int Damage { set; get; }
        
        /// <summary>
        /// 命中率
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int Hit { set; get; }
        
        /// <summary>
        /// 闪避率
        /// </summary>
        [ExcelConfigColIndex(10)]
        public float Dodge { set; get; }
        
        /// <summary>
        /// 速度
        /// </summary>
        [ExcelConfigColIndex(11)]
        public float Speed { set; get; }
        
        /// <summary>
        /// 克制关系暴击
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int CrtPro { set; get; }
        
        /// <summary>
        /// 暴击倍率
        /// </summary>
        [ExcelConfigColIndex(13)]
        public int Mult { set; get; }
        
        /// <summary>
        /// 技能ID
        /// </summary>
        [ExcelConfigColIndex(14)]
        public int SkillID { set; get; }
        
        /// <summary>
        /// 掉落方案ID
        /// </summary>
        [ExcelConfigColIndex(15)]
        public int DropID { set; get; }

    }

    /// <summary>
    /// 成就
    /// </summary>
    [ConfigFile("AchievementConfig.json","AchievementConfig")]
    public class AchievementConfig:JSONConfigBase    {
        
        /// <summary>
        /// 成就名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 成就描述
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Description { set; get; }
        
        /// <summary>
        /// 成就达成条件
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int ConditionType { set; get; }
        
        /// <summary>
        /// 条件参数1
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String Pars1 { set; get; }
        
        /// <summary>
        /// 条件参数2
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String Pars2 { set; get; }
        
        /// <summary>
        /// 条件参数3
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Pars3 { set; get; }
        
        /// <summary>
        /// 条件参数4
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String Pars4 { set; get; }
        
        /// <summary>
        /// 奖励成就点
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int RewardPoint { set; get; }
        
        /// <summary>
        /// 物品奖励
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int itmeReward { set; get; }
        
        /// <summary>
        /// 奖励参数1
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String RewardPar1 { set; get; }
        
        /// <summary>
        /// 奖励参数2
        /// </summary>
        [ExcelConfigColIndex(11)]
        public String RewardPar2 { set; get; }
        
        /// <summary>
        /// 奖励参数3
        /// </summary>
        [ExcelConfigColIndex(12)]
        public String RewardPar3 { set; get; }

    }

    /// <summary>
    /// 战斗组表
    /// </summary>
    [ConfigFile("BattleGroupConfig.json","BattleGroupConfig")]
    public class BattleGroupConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 战斗文本
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String battleDiscribtion { set; get; }
        
        /// <summary>
        /// 战斗次数
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int BattleCount { set; get; }
        
        /// <summary>
        /// 战斗ID
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String BattleIds { set; get; }

    }

    /// <summary>
    /// 战斗表
    /// </summary>
    [ConfigFile("BattleConfig.json","BattleConfig")]
    public class BattleConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 战斗组事件文本
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Dialog { set; get; }
        
        /// <summary>
        /// NPCID
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int NpcID { set; get; }
        
        /// <summary>
        /// 奖励金币
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int RewardGold { set; get; }
        
        /// <summary>
        /// 附加奖励物品ID
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int AddtionRewarditem { set; get; }
        
        /// <summary>
        /// 奖励数
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Pars1 { set; get; }

    }

    /// <summary>
    /// 技能表
    /// </summary>
    [ConfigFile("SkillConfig.json","SkillConfig")]
    public class SkillConfig:JSONConfigBase    {
        
        /// <summary>
        /// 技能名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 技能描述
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Description { set; get; }
        
        /// <summary>
        /// 技能效果
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Effect { set; get; }
        
        /// <summary>
        /// 技能cd
        /// </summary>
        [ExcelConfigColIndex(4)]
        public float SkillCd { set; get; }
        
        /// <summary>
        /// 主要效果类型
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int MainEffectType { set; get; }
        
        /// <summary>
        /// 效果目标
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int MainEffectTarget { set; get; }
        
        /// <summary>
        /// 主要效果数值
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int MainEffectNumber { set; get; }
        
        /// <summary>
        /// 附加效果类型
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int StatusType { set; get; }
        
        /// <summary>
        /// 效果目标
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int StatusTarget { set; get; }
        
        /// <summary>
        /// 附加效果参数1
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String Pars1 { set; get; }
        
        /// <summary>
        /// 附加效果参数2
        /// </summary>
        [ExcelConfigColIndex(11)]
        public String Pars2 { set; get; }
        
        /// <summary>
        /// 附加效果参数3
        /// </summary>
        [ExcelConfigColIndex(12)]
        public String Pars3 { set; get; }

    }

    /// <summary>
    /// 招募表
    /// </summary>
    [ConfigFile("HeroConfig.json","HeroConfig")]
    public class HeroConfig:JSONConfigBase    {
        
        /// <summary>
        /// 招募兵种ID
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int recruit_id { set; get; }
        
        /// <summary>
        /// 招募货币类型
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int recruit_current_type { set; get; }
        
        /// <summary>
        /// 招募价格
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int recruit_price { set; get; }
        
        /// <summary>
        /// 招募条件
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int recruit_condition { set; get; }
        
        /// <summary>
        /// 参数
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String recruit_para { set; get; }
        
        /// <summary>
        /// 条件文本说明
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String recruit_des { set; get; }

    }

    /// <summary>
    /// 掉落
    /// </summary>
    [ConfigFile("DropConfig.json","DropConfig")]
    public class DropConfig:JSONConfigBase    {
        
        /// <summary>
        /// 掉落概率1
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int DropItemPro1 { set; get; }
        
        /// <summary>
        /// 掉落物品1
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int DropItem1 { set; get; }
        
        /// <summary>
        /// 掉落数量1
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int DropItemCoun1 { set; get; }
        
        /// <summary>
        /// 掉落概率2
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int DropItemPro2 { set; get; }
        
        /// <summary>
        /// 掉落物品2
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int DropItem2 { set; get; }
        
        /// <summary>
        /// 掉落数量2
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int DropItemCoun2 { set; get; }
        
        /// <summary>
        /// 掉落概率3
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int DropItemPro3 { set; get; }
        
        /// <summary>
        /// 掉落物品3
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int DropItem3 { set; get; }
        
        /// <summary>
        /// 掉落数量3
        /// </summary>
        [ExcelConfigColIndex(9)]
        public int DropItemCoun3 { set; get; }
        
        /// <summary>
        /// 掉落概率4
        /// </summary>
        [ExcelConfigColIndex(10)]
        public int DropItemPro4 { set; get; }
        
        /// <summary>
        /// 掉落物品4
        /// </summary>
        [ExcelConfigColIndex(11)]
        public int DropItem4 { set; get; }
        
        /// <summary>
        /// 掉落数量4
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int DropItemCoun4 { set; get; }
        
        /// <summary>
        /// 掉落概率5
        /// </summary>
        [ExcelConfigColIndex(13)]
        public int DropItemPro5 { set; get; }
        
        /// <summary>
        /// 掉落物品5
        /// </summary>
        [ExcelConfigColIndex(14)]
        public int DropItem5 { set; get; }
        
        /// <summary>
        /// 掉落数量5
        /// </summary>
        [ExcelConfigColIndex(15)]
        public int DropItemCoun5 { set; get; }
        
        /// <summary>
        /// 掉落概率6
        /// </summary>
        [ExcelConfigColIndex(16)]
        public int DropItemPro6 { set; get; }
        
        /// <summary>
        /// 掉落物品6
        /// </summary>
        [ExcelConfigColIndex(17)]
        public int DropItem6 { set; get; }
        
        /// <summary>
        /// 掉落数量6
        /// </summary>
        [ExcelConfigColIndex(18)]
        public int DropItemCoun6 { set; get; }

    }

    /// <summary>
    /// 货郎
    /// </summary>
    [ConfigFile("StoreDataConfig.json","StoreDataConfig")]
    public class StoreDataConfig:JSONConfigBase    {
        
        /// <summary>
        /// 物品ID
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int ItemId { set; get; }
        
        /// <summary>
        /// 物品名称
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Name { set; get; }
        
        /// <summary>
        /// 出售价格
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int Sold_price { set; get; }
        
        /// <summary>
        /// 购买最大数
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int Max_purchase { set; get; }
        
        /// <summary>
        /// 物品解锁条件类
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int Unlock_type { set; get; }
        
        /// <summary>
        /// 解锁参数1
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Unlock_para1 { set; get; }
        
        /// <summary>
        /// 解锁参数2
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String Unlock_para2 { set; get; }
        
        /// <summary>
        /// 解锁参数3
        /// </summary>
        [ExcelConfigColIndex(8)]
        public String Unlock_para3 { set; get; }
        
        /// <summary>
        /// 解锁参数4
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String Unlock_para4 { set; get; }

    }

    /// <summary>
    /// 资源产量表
    /// </summary>
    [ConfigFile("ResourcesProduceConfig.json","ResourcesProduceConfig")]
    public class ResourcesProduceConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 消耗资源ID
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String CostItems { set; get; }
        
        /// <summary>
        /// 消耗资源量
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String CostItemsNumber { set; get; }
        
        /// <summary>
        /// 收获资源ID
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String RewardItems { set; get; }
        
        /// <summary>
        /// 收获资源量
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String RewardItemsNumber { set; get; }
        
        /// <summary>
        /// 说明描述
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Description { set; get; }

    }

    /// <summary>
    /// 道具表
    /// </summary>
    [ConfigFile("ItemConfig.json","ItemConfig")]
    public class ItemConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 描述
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Desription { set; get; }
        
        /// <summary>
        /// 物品类型
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int Category { set; get; }
        
        /// <summary>
        /// 出售货币类型
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int CoinType { set; get; }
        
        /// <summary>
        /// 出售价格
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int Price { set; get; }
        
        /// <summary>
        /// 是否可以售卖
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int CanSale { set; get; }
        
        /// <summary>
        /// 参数1
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String Pars1 { set; get; }
        
        /// <summary>
        /// 参数2
        /// </summary>
        [ExcelConfigColIndex(8)]
        public String Pars2 { set; get; }
        
        /// <summary>
        /// 参数3
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String Pars3 { set; get; }
        
        /// <summary>
        /// 参数4
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String Pars4 { set; get; }
        
        /// <summary>
        /// 参数5
        /// </summary>
        [ExcelConfigColIndex(11)]
        public String Pars5 { set; get; }

    }

    /// <summary>
    /// 钻石商城
    /// </summary>
    [ConfigFile("DimondStoreConfig.json","DimondStoreConfig")]
    public class DimondStoreConfig:JSONConfigBase    {
        
        /// <summary>
        /// 物品名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 商品识别码
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String ItemKey { set; get; }
        
        /// <summary>
        /// 购买价格
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int Sold_price { set; get; }
        
        /// <summary>
        /// 是否打折
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int Is_reduce { set; get; }
        
        /// <summary>
        /// 折扣比例
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String Reduce_percent { set; get; }
        
        /// <summary>
        /// 折扣持续周期
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Last_time { set; get; }
        
        /// <summary>
        /// 最大可购买数
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int Max_purchase_times { set; get; }

    }

    /// <summary>
    /// 随机事件表
    /// </summary>
    [ConfigFile("RandomEventConfig.json","RandomEventConfig")]
    public class RandomEventConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 事件类型
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int Type { set; get; }
        
        /// <summary>
        /// 事件解锁类型
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int EventUnlockType { set; get; }
        
        /// <summary>
        /// 解锁参数
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int UnlockCondition { set; get; }
        
        /// <summary>
        /// 介绍
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String Dialog1 { set; get; }
        
        /// <summary>
        /// 取消
        /// </summary>
        [ExcelConfigColIndex(6)]
        public String Cancel { set; get; }
        
        /// <summary>
        /// 确定
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String OK { set; get; }
        
        /// <summary>
        /// 消耗类型
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int CostType { set; get; }
        
        /// <summary>
        /// 需要消耗材料
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String NeedItem { set; get; }
        
        /// <summary>
        /// 确定
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String DialogOK { set; get; }
        
        /// <summary>
        /// 取消
        /// </summary>
        [ExcelConfigColIndex(11)]
        public String DialogFaiuler { set; get; }
        
        /// <summary>
        /// 奖励类别
        /// </summary>
        [ExcelConfigColIndex(12)]
        public int RewardType { set; get; }
        
        /// <summary>
        /// 参数
        /// </summary>
        [ExcelConfigColIndex(13)]
        public String RewardConditon { set; get; }

    }

 }
