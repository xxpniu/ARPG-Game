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
    /// 登陆
    /// </summary>
    public class C2L_Login : Proto.ISerializerable
    {
        public C2L_Login()
        {
            UserName = string.Empty;
            Password = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public int Version { set; get; }
        /// <summary>
        /// 登陆token
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            UserName = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Password = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Version);
            var UserName_bytes = Encoding.UTF8.GetBytes(UserName==null?string.Empty:UserName);writer.Write(UserName_bytes.Length);writer.Write(UserName_bytes);
            var Password_bytes = Encoding.UTF8.GetBytes(Password==null?string.Empty:Password);writer.Write(Password_bytes.Length);writer.Write(Password_bytes);
            
        }

    }
    /// <summary>
    /// 登陆返回
    /// </summary>
    public class L2C_Login : Proto.ISerializerable
    {
        public L2C_Login()
        {
            Session = string.Empty;
            Server = new GameServerInfo();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// session
        /// </summary>
        public string Session { set; get; }
        /// <summary>
        /// 所属服务器
        /// </summary>
        public GameServerInfo Server { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            UserID = reader.ReadInt64();
            Session = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Server = new GameServerInfo();Server.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            writer.Write(UserID);
            var Session_bytes = Encoding.UTF8.GetBytes(Session==null?string.Empty:Session);writer.Write(Session_bytes.Length);writer.Write(Session_bytes);
            Server.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 注册用户
    /// </summary>
    public class C2L_Reg : Proto.ISerializerable
    {
        public C2L_Reg()
        {
            UserName = string.Empty;
            Password = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public int Version { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            UserName = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Password = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Version);
            var UserName_bytes = Encoding.UTF8.GetBytes(UserName==null?string.Empty:UserName);writer.Write(UserName_bytes.Length);writer.Write(UserName_bytes);
            var Password_bytes = Encoding.UTF8.GetBytes(Password==null?string.Empty:Password);writer.Write(Password_bytes.Length);writer.Write(Password_bytes);
            
        }

    }
    /// <summary>
    /// 注册返回
    /// </summary>
    public class L2C_Reg : Proto.ISerializerable
    {
        public L2C_Reg()
        {
            Session = string.Empty;
            Server = new GameServerInfo();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Session { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public GameServerInfo Server { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            Session = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            UserID = reader.ReadInt64();
            Server = new GameServerInfo();Server.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            var Session_bytes = Encoding.UTF8.GetBytes(Session==null?string.Empty:Session);writer.Write(Session_bytes.Length);writer.Write(Session_bytes);
            writer.Write(UserID);
            Server.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class C2G_Login : Proto.ISerializerable
    {
        public C2G_Login()
        {
            Session = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public int Version { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Session { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            Session = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            UserID = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Version);
            var Session_bytes = Encoding.UTF8.GetBytes(Session==null?string.Empty:Session);writer.Write(Session_bytes.Length);writer.Write(Session_bytes);
            writer.Write(UserID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class G2C_Login : Proto.ISerializerable
    {
        public G2C_Login()
        {
            Hero = new DHero();
            Package = new PlayerPackage();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 当前角色
        /// </summary>
        public DHero Hero { set; get; }
        /// <summary>
        /// 背包
        /// </summary>
        public PlayerPackage Package { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Coin { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            Hero = new DHero();Hero.ParseFormBinary(reader);
            Package = new PlayerPackage();Package.ParseFormBinary(reader);
            Gold = reader.ReadInt32();
            Coin = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            Hero.ToBinary(writer);
            Package.ToBinary(writer);
            writer.Write(Gold);
            writer.Write(Coin);
            
        }

    }
    /// <summary>
    /// 创建角色
    /// </summary>
    public class C2G_CreateHero : Proto.ISerializerable
    {
        public C2G_CreateHero()
        {

        }
        /// <summary>
        /// 选择的英雄ID
        /// </summary>
        public int HeroID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            HeroID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(HeroID);
            
        }

    }
    /// <summary>
    /// 创建角色
    /// </summary>
    public class G2C_CreateHero : Proto.ISerializerable
    {
        public G2C_CreateHero()
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
    /// 开始启动游戏
    /// </summary>
    public class C2G_BeginGame : Proto.ISerializerable
    {
        public C2G_BeginGame()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int MapID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            MapID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(MapID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class G2C_BeginGame : Proto.ISerializerable
    {
        public G2C_BeginGame()
        {
            ServerInfo = new GameServerInfo();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public GameServerInfo ServerInfo { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            ServerInfo = new GameServerInfo();ServerInfo.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            ServerInfo.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 退出
    /// </summary>
    public class C2B_ExitGame : Proto.ISerializerable
    {
        public C2B_ExitGame()
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
    public class B2C_ExitGame : Proto.ISerializerable
    {
        public B2C_ExitGame()
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
    public class Task_G2C_JoinBattle : Proto.ISerializerable
    {
        public Task_G2C_JoinBattle()
        {
            Server = new GameServerInfo();

        }
        /// <summary>
        /// 
        /// </summary>
        public GameServerInfo Server { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Server = new GameServerInfo();Server.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            Server.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class C2B_JoinBattle : Proto.ISerializerable
    {
        public C2B_JoinBattle()
        {
            Session = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public int MapID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Session { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Version { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            MapID = reader.ReadInt32();
            Session = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            UserID = reader.ReadInt64();
            Version = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(MapID);
            var Session_bytes = Encoding.UTF8.GetBytes(Session==null?string.Empty:Session);writer.Write(Session_bytes.Length);writer.Write(Session_bytes);
            writer.Write(UserID);
            writer.Write(Version);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class B2C_JoinBattle : Proto.ISerializerable
    {
        public B2C_JoinBattle()
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
    public class C2B_ExitBattle : Proto.ISerializerable
    {
        public C2B_ExitBattle()
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
    public class B2C_ExitBattle : Proto.ISerializerable
    {
        public B2C_ExitBattle()
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
    /// 获取最后一个战斗服务器
    /// </summary>
    public class C2G_GetLastBattle : Proto.ISerializerable
    {
        public C2G_GetLastBattle()
        {

        }
        /// <summary>
        /// 用户ID
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
    public class G2C_GetLastBattle : Proto.ISerializerable
    {
        public G2C_GetLastBattle()
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
        /// <summary>
        /// 地图
        /// </summary>
        public int MapID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            BattleServer = new GameServerInfo();BattleServer.ParseFormBinary(reader);
            MapID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            BattleServer.ToBinary(writer);
            writer.Write(MapID);
            
        }

    }
    /// <summary>
    /// 点击移动
    /// </summary>
    public class Action_ClickMapGround : Proto.ISerializerable
    {
        public Action_ClickMapGround()
        {
            TargetPosition = new Vector3();

        }
        /// <summary>
        /// 点击的位置
        /// </summary>
        public Vector3 TargetPosition { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            TargetPosition = new Vector3();TargetPosition.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            TargetPosition.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 点击释放技能
    /// </summary>
    public class Action_ClickSkillIndex : Proto.ISerializerable
    {
        public Action_ClickSkillIndex()
        {
            MagicKey = string.Empty;

        }
        /// <summary>
        /// 技能Key
        /// </summary>
        public string MagicKey { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            MagicKey = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var MagicKey_bytes = Encoding.UTF8.GetBytes(MagicKey==null?string.Empty:MagicKey);writer.Write(MagicKey_bytes.Length);writer.Write(MagicKey_bytes);
            
        }

    }
    /// <summary>
    /// 自动寻敌
    /// </summary>
    public class Action_AutoFindTarget : Proto.ISerializerable
    {
        public Action_AutoFindTarget()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public bool Auto { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Auto = reader.ReadBoolean();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Auto);
            
        }

    }
    /// <summary>
    /// 同步包裹
    /// </summary>
    public class Task_G2C_SyncPackage : Proto.ISerializerable
    {
        public Task_G2C_SyncPackage()
        {
            Package = new PlayerPackage();

        }
        /// <summary>
        /// 
        /// </summary>
        public PlayerPackage Package { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Coin { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Package = new PlayerPackage();Package.ParseFormBinary(reader);
            Gold = reader.ReadInt32();
            Coin = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            Package.ToBinary(writer);
            writer.Write(Gold);
            writer.Write(Coin);
            
        }

    }
    /// <summary>
    /// 同步角色
    /// </summary>
    public class Task_G2C_SyncHero : Proto.ISerializerable
    {
        public Task_G2C_SyncHero()
        {
            Hero = new DHero();

        }
        /// <summary>
        /// 
        /// </summary>
        public DHero Hero { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Hero = new DHero();Hero.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            Hero.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 处理装备穿戴
    /// </summary>
    public class C2G_OperatorEquip : Proto.ISerializerable
    {
        public C2G_OperatorEquip()
        {
            Guid = string.Empty;

        }
        /// <summary>
        /// 英雄ID
        /// </summary>
        public long HeroID { set; get; }
        /// <summary>
        /// 装备guid
        /// </summary>
        public string Guid { set; get; }
        /// <summary>
        /// 部位
        /// </summary>
        public EquipmentType Part { set; get; }
        /// <summary>
        /// 是否是穿戴
        /// </summary>
        public bool IsWear { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            HeroID = reader.ReadInt64();
            Guid = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Part = (EquipmentType)reader.ReadInt32();
            IsWear = reader.ReadBoolean();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(HeroID);
            var Guid_bytes = Encoding.UTF8.GetBytes(Guid==null?string.Empty:Guid);writer.Write(Guid_bytes.Length);writer.Write(Guid_bytes);
            writer.Write((int)Part);
            writer.Write(IsWear);
            
        }

    }
    /// <summary>
    /// 处理穿戴装备
    /// </summary>
    public class G2C_OperatorEquip : Proto.ISerializerable
    {
        public G2C_OperatorEquip()
        {
            Hero = new DHero();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DHero Hero { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            Hero = new DHero();Hero.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            Hero.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class C2G_SaleItem : Proto.ISerializerable
    {
        public C2G_SaleItem()
        {
            Items = new List<SaleItem>();

        }
        /// <summary>
        /// 需要出售的道具
        /// </summary>
        public List<SaleItem> Items { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            int Items_Len = reader.ReadInt32();
            while(Items_Len-->0)
            {
                SaleItem Items_Temp = new SaleItem();
                Items_Temp = new SaleItem();Items_Temp.ParseFormBinary(reader);
                Items.Add(Items_Temp );
            }
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Items.Count);
            foreach(var i in Items)
            {
                i.ToBinary(writer);               
            }
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class G2C_SaleItem : Proto.ISerializerable
    {
        public G2C_SaleItem()
        {
            Diff = new List<PlayerItem>();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 道具变更信息
        /// </summary>
        public List<PlayerItem> Diff { set; get; }
        /// <summary>
        /// 金币最终值
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 钻石最终值
        /// </summary>
        public int Coin { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            int Diff_Len = reader.ReadInt32();
            while(Diff_Len-->0)
            {
                PlayerItem Diff_Temp = new PlayerItem();
                Diff_Temp = new PlayerItem();Diff_Temp.ParseFormBinary(reader);
                Diff.Add(Diff_Temp );
            }
            Gold = reader.ReadInt32();
            Coin = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            writer.Write(Diff.Count);
            foreach(var i in Diff)
            {
                i.ToBinary(writer);               
            }
            writer.Write(Gold);
            writer.Write(Coin);
            
        }

    }
    /// <summary>
    /// 装备升级 ＋
    /// </summary>
    public class C2G_EquipmentLevelUp : Proto.ISerializerable
    {
        public C2G_EquipmentLevelUp()
        {
            Guid = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public string Guid { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Level { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Guid = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Level = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var Guid_bytes = Encoding.UTF8.GetBytes(Guid==null?string.Empty:Guid);writer.Write(Guid_bytes.Length);writer.Write(Guid_bytes);
            writer.Write(Level);
            
        }

    }
    /// <summary>
    /// 装备升级
    /// </summary>
    public class G2C_EquipmentLevelUp : Proto.ISerializerable
    {
        public G2C_EquipmentLevelUp()
        {
            ResultEquip = new Equip();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool LevelUp { set; get; }
        /// <summary>
        /// 金币
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 钻石
        /// </summary>
        public int Coin { set; get; }
        /// <summary>
        /// 装备刷新
        /// </summary>
        public Equip ResultEquip { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            LevelUp = reader.ReadBoolean();
            Gold = reader.ReadInt32();
            Coin = reader.ReadInt32();
            ResultEquip = new Equip();ResultEquip.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            writer.Write(LevelUp);
            writer.Write(Gold);
            writer.Write(Coin);
            ResultEquip.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class C2G_GMTool : Proto.ISerializerable
    {
        public C2G_GMTool()
        {
            GMCommand = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public string GMCommand { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            GMCommand = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var GMCommand_bytes = Encoding.UTF8.GetBytes(GMCommand==null?string.Empty:GMCommand);writer.Write(GMCommand_bytes.Length);writer.Write(GMCommand_bytes);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class G2C_GMTool : Proto.ISerializerable
    {
        public G2C_GMTool()
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