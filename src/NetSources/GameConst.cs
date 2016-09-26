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
    /// 每个版本号最大 99
    /// </summary>
    public enum GameVersion
    {
        /// <summary>
        /// 主要版本
        /// </summary>
        Master=1,
        /// <summary>
        /// 更新版本
        /// </summary>
        Major=0,
        /// <summary>
        /// 开发版本
        /// </summary>
        Develop=3,

    }
   /// <summary>
    /// 错误码 考虑平台问题 不要尝试串码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 协议版本异常
        /// </summary>
        VersionError=-1,
        /// <summary>
        /// 通用错误
        /// </summary>
        Error=0,
        /// <summary>
        /// 处理成功
        /// </summary>
        OK=1,
        /// <summary>
        /// 登陆失败
        /// </summary>
        LoginFailure=2,
        /// <summary>
        /// 用户名重复
        /// </summary>
        RegExistUserName=3,
        /// <summary>
        /// 输入为空
        /// </summary>
        RegInputEmptyOrNull=4,
        /// <summary>
        /// 没有游戏角色信息
        /// </summary>
        NoGamePlayerData=5,
        /// <summary>
        /// 英雄数据异常
        /// </summary>
        NoHeroInfo=6,
        /// <summary>
        /// 没有对应的serverID
        /// </summary>
        NOFoundServerID=7,
        /// <summary>
        /// 没有空闲的战斗服务器
        /// </summary>
        NOFreeBattleServer=8,
        /// <summary>
        /// 玩家已经在战斗中
        /// </summary>
        PlayerIsInBattle=9,
        /// <summary>
        /// 战斗服务器已经断开连接
        /// </summary>
        BattleServerHasDisconnect=10,
        /// <summary>
        /// 没有申请战斗服务器
        /// </summary>
        NOFoundUserOnBattleServer=11,
        /// <summary>
        /// 没有战斗服务器
        /// </summary>
        NOFoundUserBattleServer=12,
        /// <summary>
        /// 没有道具
        /// </summary>
        NOFoundItem=13,
        /// <summary>
        /// 道具数量不足
        /// </summary>
        NOEnoughItem=14,
        /// <summary>
        /// 穿戴中
        /// </summary>
        IsWearOnHero=15,
        /// <summary>
        /// 金币不足
        /// </summary>
        NoEnoughtGold=16,
        /// <summary>
        /// 没有空闲网关服务器
        /// </summary>
        NoFreeGateServer=17,

    }
}