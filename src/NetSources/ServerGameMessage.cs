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
    /// 派发启动战斗副本任务
    /// </summary>
    public class Task_L2B_StartBattle : Proto.ISerializerable
    {
        public Task_L2B_StartBattle()
        {
            Users = new List<PlayerServerInfo>();

        }
        /// <summary>
        /// 启动userID
        /// </summary>
        public List<PlayerServerInfo> Users { set; get; }
        /// <summary>
        /// 启动地图
        /// </summary>
        public int MapID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            int Users_Len = reader.ReadInt32();
            while(Users_Len-->0)
            {
                PlayerServerInfo Users_Temp = new PlayerServerInfo();
                Users_Temp = new PlayerServerInfo();Users_Temp.ParseFormBinary(reader);
                Users.Add(Users_Temp );
            }
            MapID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Users.Count);
            foreach(var i in Users)
            {
                i.ToBinary(writer);               
            }
            writer.Write(MapID);
            
        }

    }
    /// <summary>
    /// 游戏服务器注册
    /// </summary>
    public class G2L_Reg : Proto.ISerializerable
    {
        public G2L_Reg()
        {
            Host = string.Empty;
            ServiceHost = string.Empty;

        }
        /// <summary>
        /// 当前版本
        /// </summary>
        public int Version { set; get; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string Host { set; get; }
        /// <summary>
        /// 服务器端口 公开
        /// </summary>
        public int Port { set; get; }
        /// <summary>
        /// 内部访问IP
        /// </summary>
        public string ServiceHost { set; get; }
        /// <summary>
        /// 内部服务器端口
        /// </summary>
        public int ServicesProt { set; get; }
        /// <summary>
        /// 服务器ID
        /// </summary>
        public int ServerID { set; get; }
        /// <summary>
        /// 服务器最大玩家数 数据库最大支持玩家
        /// </summary>
        public int MaxPlayer { set; get; }
        /// <summary>
        /// 当前玩家数
        /// </summary>
        public int CurrentPlayer { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            Host = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Port = reader.ReadInt32();
            ServiceHost = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            ServicesProt = reader.ReadInt32();
            ServerID = reader.ReadInt32();
            MaxPlayer = reader.ReadInt32();
            CurrentPlayer = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Version);
            var Host_bytes = Encoding.UTF8.GetBytes(Host==null?string.Empty:Host);writer.Write(Host_bytes.Length);writer.Write(Host_bytes);
            writer.Write(Port);
            var ServiceHost_bytes = Encoding.UTF8.GetBytes(ServiceHost==null?string.Empty:ServiceHost);writer.Write(ServiceHost_bytes.Length);writer.Write(ServiceHost_bytes);
            writer.Write(ServicesProt);
            writer.Write(ServerID);
            writer.Write(MaxPlayer);
            writer.Write(CurrentPlayer);
            
        }

    }
    /// <summary>
    /// 游戏服务器返回
    /// </summary>
    public class L2G_Reg : Proto.ISerializerable
    {
        public L2G_Reg()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            
        }

    }
    /// <summary>
    /// 检查session是否有效
    /// </summary>
    public class G2L_CheckUserSession : Proto.ISerializerable
    {
        public G2L_CheckUserSession()
        {
            Session = string.Empty;

        }
        /// <summary>
        /// 玩家ID
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 当前登陆信息
        /// </summary>
        public string Session { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
            Session = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            var Session_bytes = Encoding.UTF8.GetBytes(Session==null?string.Empty:Session);writer.Write(Session_bytes.Length);writer.Write(Session_bytes);
            
        }

    }
    /// <summary>
    /// 返回检查session结果
    /// </summary>
    public class L2G_CheckUserSession : Proto.ISerializerable
    {
        public L2G_CheckUserSession()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            
        }

    }
    /// <summary>
    /// 启动一个战斗仿真
    /// </summary>
    public class G2L_BeginBattle : Proto.ISerializerable
    {
        public G2L_BeginBattle()
        {

        }
        /// <summary>
        /// 发起请求的用户
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 仿真地图MapID
        /// </summary>
        public int MapID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
            MapID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            writer.Write(MapID);
            
        }

    }
    /// <summary>
    /// 启动返回
    /// </summary>
    public class L2G_BeginBattle : Proto.ISerializerable
    {
        public L2G_BeginBattle()
        {
            BattleServer = new GameServerInfo();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 当前战斗服务器
        /// </summary>
        public GameServerInfo BattleServer { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            BattleServer = new GameServerInfo();BattleServer.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            BattleServer.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class G2L_GetLastBattle : Proto.ISerializerable
    {
        public G2L_GetLastBattle()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class L2G_GetLastBattle : Proto.ISerializerable
    {
        public L2G_GetLastBattle()
        {
            BattleServer = new GameServerInfo();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 当前战斗服务器
        /// </summary>
        public GameServerInfo BattleServer { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            BattleServer = new GameServerInfo();BattleServer.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            BattleServer.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 注册一个战斗服务器
    /// </summary>
    public class B2L_RegBattleServer : Proto.ISerializerable
    {
        public B2L_RegBattleServer()
        {
            ServiceHost = string.Empty;

        }
        /// <summary>
        /// 当前版本
        /// </summary>
        public int Version { set; get; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServicePort { set; get; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServiceHost { set; get; }
        /// <summary>
        /// 最大战斗仿真上限
        /// </summary>
        public int MaxBattleCount { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            ServicePort = reader.ReadInt32();
            ServiceHost = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            MaxBattleCount = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(ServicePort);
            var ServiceHost_bytes = Encoding.UTF8.GetBytes(ServiceHost==null?string.Empty:ServiceHost);writer.Write(ServiceHost_bytes.Length);writer.Write(ServiceHost_bytes);
            writer.Write(MaxBattleCount);
            
        }

    }
    /// <summary>
    /// 注册返回
    /// </summary>
    public class L2B_RegBattleServer : Proto.ISerializerable
    {
        public L2B_RegBattleServer()
        {

        }
        /// <summary>
        /// 返回码
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 中心服务器（登陆服务器）给的当前服务器ID
        /// </summary>
        public int ServiceServerID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            ServiceServerID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            writer.Write(ServiceServerID);
            
        }

    }
    /// <summary>
    /// 玩家完成战斗
    /// </summary>
    public class B2L_EndBattle : Proto.ISerializerable
    {
        public B2L_EndBattle()
        {

        }
        /// <summary>
        /// 玩家ID
        /// </summary>
        public long UserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            
        }

    }
    /// <summary>
    /// 玩家完成战斗
    /// </summary>
    public class L2B_EndBattle : Proto.ISerializerable
    {
        public L2B_EndBattle()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class B2L_CheckSession : Proto.ISerializerable
    {
        public B2L_CheckSession()
        {
            SessionKey = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public string SessionKey { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            SessionKey = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            UserID = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var SessionKey_bytes = Encoding.UTF8.GetBytes(SessionKey==null?string.Empty:SessionKey);writer.Write(SessionKey_bytes.Length);writer.Write(SessionKey_bytes);
            writer.Write(UserID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class L2B_CheckSession : Proto.ISerializerable
    {
        public L2B_CheckSession()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            
        }

    }
    /// <summary>
    /// 请求获得玩家信息
    /// </summary>
    public class B2G_GetPlayerInfo : Proto.ISerializerable
    {
        public B2G_GetPlayerInfo()
        {

        }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 当前战斗服务器ID
        /// </summary>
        public int ServiceServerID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
            ServiceServerID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            writer.Write(ServiceServerID);
            
        }

    }
    /// <summary>
    /// 请求玩家信息返回
    /// </summary>
    public class G2B_GetPlayerInfo : Proto.ISerializerable
    {
        public G2B_GetPlayerInfo()
        {
            Hero = new DHero();
            Package = new PlayerPackage();

        }
        /// <summary>
        /// 返回信息
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 英雄数据
        /// </summary>
        public DHero Hero { set; get; }
        /// <summary>
        /// 道具列表
        /// </summary>
        public PlayerPackage Package { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            Hero = new DHero();Hero.ParseFormBinary(reader);
            Package = new PlayerPackage();Package.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            Hero.ToBinary(writer);
            Package.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 返回战斗数据
    /// </summary>
    public class B2G_BattleReward : Proto.ISerializerable
    {
        public B2G_BattleReward()
        {
            DropItems = new List<PlayerItem>();
            ConsumeItems = new List<PlayerItem>();

        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 用户金币
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 当前战斗地图
        /// </summary>
        public int MapID { set; get; }
        /// <summary>
        /// 用户击杀怪物数
        /// </summary>
        public int KillMonsterCount { set; get; }
        /// <summary>
        /// 伤害数
        /// </summary>
        public long DamageTotal { set; get; }
        /// <summary>
        /// 掉落物品
        /// </summary>
        public List<PlayerItem> DropItems { set; get; }
        /// <summary>
        /// 消耗道具
        /// </summary>
        public List<PlayerItem> ConsumeItems { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
            Gold = reader.ReadInt32();
            MapID = reader.ReadInt32();
            KillMonsterCount = reader.ReadInt32();
            DamageTotal = reader.ReadInt64();
            int DropItems_Len = reader.ReadInt32();
            while(DropItems_Len-->0)
            {
                PlayerItem DropItems_Temp = new PlayerItem();
                DropItems_Temp = new PlayerItem();DropItems_Temp.ParseFormBinary(reader);
                DropItems.Add(DropItems_Temp );
            }
            int ConsumeItems_Len = reader.ReadInt32();
            while(ConsumeItems_Len-->0)
            {
                PlayerItem ConsumeItems_Temp = new PlayerItem();
                ConsumeItems_Temp = new PlayerItem();ConsumeItems_Temp.ParseFormBinary(reader);
                ConsumeItems.Add(ConsumeItems_Temp );
            }
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            writer.Write(Gold);
            writer.Write(MapID);
            writer.Write(KillMonsterCount);
            writer.Write(DamageTotal);
            writer.Write(DropItems.Count);
            foreach(var i in DropItems)
            {
                i.ToBinary(writer);               
            }
            writer.Write(ConsumeItems.Count);
            foreach(var i in ConsumeItems)
            {
                i.ToBinary(writer);               
            }
            
        }

    }
    /// <summary>
    /// 返回战斗数据
    /// </summary>
    public class G2B_BattleReward : Proto.ISerializerable
    {
        public G2B_BattleReward()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            
        }

    }
}