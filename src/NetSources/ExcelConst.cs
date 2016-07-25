/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
   /// <summary>
    /// 攻击类型
    /// </summary>
    public enum AttackType
    {
        /// <summary>
        /// 进程
        /// </summary>
        Normal=0,
        /// <summary>
        /// 远程
        /// </summary>
        Remote=1,

    }
   /// <summary>
    /// 防御类型
    /// </summary>
    public enum DefanceType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal=0,
        /// <summary>
        /// 盾牌
        /// </summary>
        Shield=1,

    }
   /// <summary>
    /// 种族类型
    /// </summary>
    public enum BodyType
    {
        /// <summary>
        /// 人类
        /// </summary>
        Human=0,
        /// <summary>
        /// 骷髅
        /// </summary>
        Skeleton=1,

    }
   /// <summary>
    /// 伤害类型
    /// </summary>
    public enum DamageType
    {
        /// <summary>
        /// 混乱
        /// </summary>
        Confusion=0,
        /// <summary>
        /// 物理
        /// </summary>
        Physical=1,
        /// <summary>
        /// 魔法
        /// </summary>
        Magic=2,

    }
   /// <summary>
    /// 解锁条件
    /// </summary>
    public enum LevelUnlockType
    {
        /// <summary>
        /// 无需解锁
        /// </summary>
        None=0,
        /// <summary>
        /// 前置关卡
        /// </summary>
        NeedCompleteLevel=1,

    }
}