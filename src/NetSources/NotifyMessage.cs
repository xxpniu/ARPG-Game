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
    /// 
    /// </summary>
    public enum MoveType
    {
        /// <summary>
        /// 普通移动
        /// </summary>
        NormalMove=1,
        /// <summary>
        /// 后退
        /// </summary>
        Back=2,
        /// <summary>
        /// 跳跃
        /// </summary>
        Jump=3,

    }
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
        public int Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int ReleaserIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int TargetIndex { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string MagicKey { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            ReleaserIndex = reader.ReadInt32();
            TargetIndex = reader.ReadInt32();
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
            Magics = new List<HeroMagicData>();

        }
        /// <summary>
        /// 
        /// </summary>
        public int Index { set; get; }
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
        /// <summary>
        /// 
        /// </summary>
        public List<HeroMagicData> Magics { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
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
            int Magics_Len = reader.ReadInt32();
            while(Magics_Len-->0)
            {
                HeroMagicData Magics_Temp = new HeroMagicData();
                Magics_Temp = new HeroMagicData();Magics_Temp.ParseFormBinary(reader);
                Magics.Add(Magics_Temp );
            }
             
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
            writer.Write(Magics.Count);
            foreach(var i in Magics)
            {
                i.ToBinary(writer);               
            }
            
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
        public int Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int ReleaserIndex { set; get; }
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
            Index = reader.ReadInt32();
            ReleaserIndex = reader.ReadInt32();
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
            TargetPosition = new Vector3();

        }
        /// <summary>
        /// 
        /// </summary>
        public int Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 LastPosition { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 TargetPosition { set; get; }
        /// <summary>
        /// 移动类型
        /// </summary>
        public MoveType Type { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            LastPosition = new Vector3();LastPosition.ParseFormBinary(reader);
            TargetPosition = new Vector3();TargetPosition.ParseFormBinary(reader);
            Type = (MoveType)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            LastPosition.ToBinary(writer);
            TargetPosition.ToBinary(writer);
            writer.Write((int)Type);
            
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
        public int Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Motion { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
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
        public int Own { set; get; }
        /// <summary>
        /// 目标
        /// </summary>
        public int Target { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Own = reader.ReadInt32();
            Target = reader.ReadInt32();
             
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
        public int ReleaseIndex { set; get; }
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
            ReleaseIndex = reader.ReadInt32();
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
        public int Index { set; get; }
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
            Index = reader.ReadInt32();
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
    public class Notify_HPChange : Proto.ISerializerable
    {
        public Notify_HPChange()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int Index { set; get; }
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
            Index = reader.ReadInt32();
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
        public int Index { set; get; }
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
            Index = reader.ReadInt32();
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
        public int Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int TargetIndex { set; get; }
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
            Index = reader.ReadInt32();
            TargetIndex = reader.ReadInt32();
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
    /// 
    /// </summary>
    public class Notify_Drop : Proto.ISerializerable
    {
        public Notify_Drop()
        {
            Items = new List<PlayerItem>();

        }
        /// <summary>
        /// 
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<PlayerItem> Items { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Gold = reader.ReadInt32();
            int Items_Len = reader.ReadInt32();
            while(Items_Len-->0)
            {
                PlayerItem Items_Temp = new PlayerItem();
                Items_Temp = new PlayerItem();Items_Temp.ParseFormBinary(reader);
                Items.Add(Items_Temp );
            }
            UserID = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Gold);
            writer.Write(Items.Count);
            foreach(var i in Items)
            {
                i.ToBinary(writer);               
            }
            writer.Write(UserID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_PlayerJoinState : Proto.ISerializerable
    {
        public Notify_PlayerJoinState()
        {
            Package = new PlayerPackage();

        }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public float TimeNow { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Gold { set; get; }
        /// <summary>
        /// 道具列表
        /// </summary>
        public PlayerPackage Package { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt64();
            TimeNow = reader.ReadSingle();
            Gold = reader.ReadInt32();
            Package = new PlayerPackage();Package.ParseFormBinary(reader);
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            writer.Write(TimeNow);
            writer.Write(Gold);
            Package.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_ReleaseMagic : Proto.ISerializerable
    {
        public Notify_ReleaseMagic()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public float CdCompletedTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int MagicID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            CdCompletedTime = reader.ReadSingle();
            MagicID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(CdCompletedTime);
            writer.Write(MagicID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notify_CharacterAlpha : Proto.ISerializerable
    {
        public Notify_CharacterAlpha()
        {

        }
        /// <summary>
        /// 角色
        /// </summary>
        public int Index { set; get; }
        /// <summary>
        /// 最终的Alpha
        /// </summary>
        public float Alpha { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Alpha = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Alpha);
            
        }

    }
}