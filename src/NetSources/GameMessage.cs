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
    /// 元素进入场景
    /// </summary>
    public class Notify_ElementJoinState : Proto.ISerializerable
    {
        public Notify_ElementJoinState()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            
        }

    }
    /// <summary>
    /// 元素退出场景
    /// </summary>
    public class Notify_ElementExitState : Proto.ISerializerable
    {
        public Notify_ElementExitState()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            
        }

    }
    /// <summary>
    /// 创建一个释放者
    /// </summary>
    public class Notify_CreateReleaser : Proto.ISerializerable
    {
        public Notify_CreateReleaser()
        {
            MagicKey = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long ReleaserIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long TargetIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string MagicKey { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            ReleaserIndex = reader.ReadInt64();
            TargetIndex = reader.ReadInt64();
            MagicKey = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(ReleaserIndex);
            writer.Write(TargetIndex);
            var MagicKey_bytes = Encoding.UTF8.GetBytes(MagicKey==null?string.Empty:MagicKey);writer.Write(MagicKey_bytes.Length);writer.Write(MagicKey_bytes);
            
        }

    }
    /// <summary>
    /// 战斗中的角色
    /// </summary>
    public class Notify_CreateBattleCharacter : Proto.ISerializerable
    {
        public Notify_CreateBattleCharacter()
        {
            Position = new Vector3();
            Forward = new Vector3();
            Properties = new List<HeroProperty>();
            Name = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 配表ID
        /// </summary>
        public int ConfigID { set; get; }
        /// <summary>
        /// 阵营ID
        /// </summary>
        public int TeamIndex { set; get; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { set; get; }
        /// <summary>
        /// 朝向
        /// </summary>
        public Vector3 Forward { set; get; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int HP { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int MP { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<HeroProperty> Properties { set; get; }
        /// <summary>
        /// 伤害类型
        /// </summary>
        public DamageType TDamage { set; get; }
        /// <summary>
        /// 防御类型
        /// </summary>
        public DefanceType TDefance { set; get; }
        /// <summary>
        /// 英雄类型
        /// </summary>
        public HeroCategory Category { set; get; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 移动速度
        /// </summary>
        public float Speed { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            UserID = reader.ReadInt64();
            ConfigID = reader.ReadInt32();
            TeamIndex = reader.ReadInt32();
            Position = new Vector3();Position.ParseFormBinary(reader);
            Forward = new Vector3();Forward.ParseFormBinary(reader);
            Level = reader.ReadInt32();
            HP = reader.ReadInt32();
            MP = reader.ReadInt32();
            int Properties_Len = reader.ReadInt32();
            while(Properties_Len-->0)
            {
                HeroProperty Properties_Temp = new HeroProperty();
                Properties_Temp = new HeroProperty();Properties_Temp.ParseFormBinary(reader);
                Properties.Add(Properties_Temp );
            }
            TDamage = (DamageType)reader.ReadInt32();
            TDefance = (DefanceType)reader.ReadInt32();
            Category = (HeroCategory)reader.ReadInt32();
            Name = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Speed = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(UserID);
            writer.Write(ConfigID);
            writer.Write(TeamIndex);
            Position.ToBinary(writer);
            Forward.ToBinary(writer);
            writer.Write(Level);
            writer.Write(HP);
            writer.Write(MP);
            writer.Write(Properties.Count);
            foreach(var i in Properties)
            {
                i.ToBinary(writer);               
            }
            writer.Write((int)TDamage);
            writer.Write((int)TDefance);
            writer.Write((int)Category);
            var Name_bytes = Encoding.UTF8.GetBytes(Name==null?string.Empty:Name);writer.Write(Name_bytes.Length);writer.Write(Name_bytes);
            writer.Write(Speed);
            
        }

    }
    /// <summary>
    /// 创建一个飞行物
    /// </summary>
    public class Notify_CreateMissile : Proto.ISerializerable
    {
        public Notify_CreateMissile()
        {
            ResourcesPath = string.Empty;
            Position = new Vector3();
            formBone = string.Empty;
            toBone = string.Empty;
            offset = new Vector3();

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long ReleaserIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ResourcesPath { set; get; }
        /// <summary>
        /// 速度
        /// </summary>
        public float Speed { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 Position { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string formBone { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string toBone { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 offset { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            ReleaserIndex = reader.ReadInt64();
            ResourcesPath = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Speed = reader.ReadSingle();
            Position = new Vector3();Position.ParseFormBinary(reader);
            formBone = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            toBone = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            offset = new Vector3();offset.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(ReleaserIndex);
            var ResourcesPath_bytes = Encoding.UTF8.GetBytes(ResourcesPath==null?string.Empty:ResourcesPath);writer.Write(ResourcesPath_bytes.Length);writer.Write(ResourcesPath_bytes);
            writer.Write(Speed);
            Position.ToBinary(writer);
            var formBone_bytes = Encoding.UTF8.GetBytes(formBone==null?string.Empty:formBone);writer.Write(formBone_bytes.Length);writer.Write(formBone_bytes);
            var toBone_bytes = Encoding.UTF8.GetBytes(toBone==null?string.Empty:toBone);writer.Write(toBone_bytes.Length);writer.Write(toBone_bytes);
            offset.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_CharacterPosition : Proto.ISerializerable
    {
        public Notify_CharacterPosition()
        {
            LastPosition = new Vector3();
            StartForward = new Vector3();
            TargetPosition = new Vector3();

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 LastPosition { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 StartForward { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 TargetPosition { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public float Speed { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            LastPosition = new Vector3();LastPosition.ParseFormBinary(reader);
            StartForward = new Vector3();StartForward.ParseFormBinary(reader);
            TargetPosition = new Vector3();TargetPosition.ParseFormBinary(reader);
            Speed = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            LastPosition.ToBinary(writer);
            StartForward.ToBinary(writer);
            TargetPosition.ToBinary(writer);
            writer.Write(Speed);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_LayoutPlayMotion : Proto.ISerializerable
    {
        public Notify_LayoutPlayMotion()
        {
            Motion = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Motion { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            Motion = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            var Motion_bytes = Encoding.UTF8.GetBytes(Motion==null?string.Empty:Motion);writer.Write(Motion_bytes.Length);writer.Write(Motion_bytes);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_LookAtCharacter : Proto.ISerializerable
    {
        public Notify_LookAtCharacter()
        {

        }
        /// <summary>
        /// 源
        /// </summary>
        public long Own { set; get; }
        /// <summary>
        /// 目标
        /// </summary>
        public long Target { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Own = reader.ReadInt64();
            Target = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Own);
            writer.Write(Target);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_LayoutPlayParticle : Proto.ISerializerable
    {
        public Notify_LayoutPlayParticle()
        {
            FromBoneName = string.Empty;
            ToBoneName = string.Empty;
            Path = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public long ReleaseIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int FromTarget { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int ToTarget { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string FromBoneName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ToBoneName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Bind { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int DestoryType { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public float DestoryTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            ReleaseIndex = reader.ReadInt64();
            FromTarget = reader.ReadInt32();
            ToTarget = reader.ReadInt32();
            FromBoneName = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            ToBoneName = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Bind = reader.ReadBoolean();
            DestoryType = reader.ReadInt32();
            DestoryTime = reader.ReadSingle();
            Path = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(ReleaseIndex);
            writer.Write(FromTarget);
            writer.Write(ToTarget);
            var FromBoneName_bytes = Encoding.UTF8.GetBytes(FromBoneName==null?string.Empty:FromBoneName);writer.Write(FromBoneName_bytes.Length);writer.Write(FromBoneName_bytes);
            var ToBoneName_bytes = Encoding.UTF8.GetBytes(ToBoneName==null?string.Empty:ToBoneName);writer.Write(ToBoneName_bytes.Length);writer.Write(ToBoneName_bytes);
            writer.Write(Bind);
            writer.Write(DestoryType);
            writer.Write(DestoryTime);
            var Path_bytes = Encoding.UTF8.GetBytes(Path==null?string.Empty:Path);writer.Write(Path_bytes.Length);writer.Write(Path_bytes);
            
        }

    }
    /// <summary>
    /// 属性修改
    /// </summary>
    public class Notify_PropertyValue : Proto.ISerializerable
    {
        public Notify_PropertyValue()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public HeroPropertyType Type { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int FinallyValue { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            Type = (HeroPropertyType)reader.ReadInt32();
            FinallyValue = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write((int)Type);
            writer.Write(FinallyValue);
            
        }

    }
    /// <summary>
    /// 广播血量变化
    /// </summary>
    public class Notity_HPChange : Proto.ISerializerable
    {
        public Notity_HPChange()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 最终HP
        /// </summary>
        public int TargetHP { set; get; }
        /// <summary>
        /// HP变化值
        /// </summary>
        public int HP { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Max { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            TargetHP = reader.ReadInt32();
            HP = reader.ReadInt32();
            Max = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(TargetHP);
            writer.Write(HP);
            writer.Write(Max);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_MPChange : Proto.ISerializerable
    {
        public Notify_MPChange()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 最终MP
        /// </summary>
        public int TargetMP { set; get; }
        /// <summary>
        /// MP变化值
        /// </summary>
        public int MP { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Max { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            TargetMP = reader.ReadInt32();
            MP = reader.ReadInt32();
            Max = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(TargetMP);
            writer.Write(MP);
            writer.Write(Max);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_DamageResult : Proto.ISerializerable
    {
        public Notify_DamageResult()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long TargetIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsMissed { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Damage { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            TargetIndex = reader.ReadInt64();
            IsMissed = reader.ReadBoolean();
            Damage = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(TargetIndex);
            writer.Write(IsMissed);
            writer.Write(Damage);
            
        }

    }
    /// <summary>
    /// 登陆
    /// </summary>
    public class C2S_Login : Proto.ISerializerable
    {
        public C2S_Login()
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
    public class S2C_Login : Proto.ISerializerable
    {
        public S2C_Login()
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
    public class C2S_Reg : Proto.ISerializerable
    {
        public C2S_Reg()
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
    public class S2C_Reg : Proto.ISerializerable
    {
        public S2C_Reg()
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
            Heros = new List<DHero>();
            Package = new PlayerPackage();

        }
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code { set; get; }
        /// <summary>
        /// 当前角色
        /// </summary>
        public List<DHero> Heros { set; get; }
        /// <summary>
        /// 背包
        /// </summary>
        public PlayerPackage Package { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Code = (ErrorCode)reader.ReadInt32();
            int Heros_Len = reader.ReadInt32();
            while(Heros_Len-->0)
            {
                DHero Heros_Temp = new DHero();
                Heros_Temp = new DHero();Heros_Temp.ParseFormBinary(reader);
                Heros.Add(Heros_Temp );
            }
            Package = new PlayerPackage();Package.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write((int)Code);
            writer.Write(Heros.Count);
            foreach(var i in Heros)
            {
                i.ToBinary(writer);               
            }
            Package.ToBinary(writer);
            
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
    public class Task_B2C_ExitBattle : Proto.ISerializerable
    {
        public Task_B2C_ExitBattle()
        {

        }

        public void ParseFormBinary(BinaryReader reader)
        {
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            
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

        }
        /// <summary>
        /// 技能对应位置
        /// </summary>
        public int Index { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            
        }

    }
}