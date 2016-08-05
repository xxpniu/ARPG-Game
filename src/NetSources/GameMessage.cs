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
            var MagicKey_bytes = Encoding.UTF8.GetBytes(MagicKey);writer.Write(MagicKey_bytes.Length);writer.Write(MagicKey_bytes);
            
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
            Name = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
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
        /// 最大血量
        /// </summary>
        public int MaxHP { set; get; }
        /// <summary>
        /// HP
        /// </summary>
        public int HP { set; get; }
        /// <summary>
        /// 伤害小
        /// </summary>
        public int DamageMin { set; get; }
        /// <summary>
        /// 伤害大
        /// </summary>
        public int DamageMax { set; get; }
        /// <summary>
        /// 攻击
        /// </summary>
        public int Attack { set; get; }
        /// <summary>
        /// 防御
        /// </summary>
        public int Defence { set; get; }
        /// <summary>
        /// 伤害类型
        /// </summary>
        public DamageType TDamage { set; get; }
        /// <summary>
        /// 防御类型
        /// </summary>
        public DefanceType TDefance { set; get; }
        /// <summary>
        /// 攻击类型
        /// </summary>
        public AttackType TAttack { set; get; }
        /// <summary>
        /// 种族
        /// </summary>
        public BodyType TBody { set; get; }
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
            ConfigID = reader.ReadInt32();
            TeamIndex = reader.ReadInt32();
            Position = new Vector3();Position.ParseFormBinary(reader);
            Forward = new Vector3();Forward.ParseFormBinary(reader);
            Level = reader.ReadInt32();
            MaxHP = reader.ReadInt32();
            HP = reader.ReadInt32();
            DamageMin = reader.ReadInt32();
            DamageMax = reader.ReadInt32();
            Attack = reader.ReadInt32();
            Defence = reader.ReadInt32();
            TDamage = (DamageType)reader.ReadInt32();
            TDefance = (DefanceType)reader.ReadInt32();
            TAttack = (AttackType)reader.ReadInt32();
            TBody = (BodyType)reader.ReadInt32();
            Name = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Speed = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(ConfigID);
            writer.Write(TeamIndex);
            Position.ToBinary(writer);
            Forward.ToBinary(writer);
            writer.Write(Level);
            writer.Write(MaxHP);
            writer.Write(HP);
            writer.Write(DamageMin);
            writer.Write(DamageMax);
            writer.Write(Attack);
            writer.Write(Defence);
            writer.Write((int)TDamage);
            writer.Write((int)TDefance);
            writer.Write((int)TAttack);
            writer.Write((int)TBody);
            var Name_bytes = Encoding.UTF8.GetBytes(Name);writer.Write(Name_bytes.Length);writer.Write(Name_bytes);
            writer.Write(Speed);
            
        }

    }
    /// <summary>
    /// 创建一个飞行物
    /// </summary>
    public class Notity_CreateMissile : Proto.ISerializerable
    {
        public Notity_CreateMissile()
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
            var ResourcesPath_bytes = Encoding.UTF8.GetBytes(ResourcesPath);writer.Write(ResourcesPath_bytes.Length);writer.Write(ResourcesPath_bytes);
            writer.Write(Speed);
            Position.ToBinary(writer);
            var formBone_bytes = Encoding.UTF8.GetBytes(formBone);writer.Write(formBone_bytes.Length);writer.Write(formBone_bytes);
            var toBone_bytes = Encoding.UTF8.GetBytes(toBone);writer.Write(toBone_bytes.Length);writer.Write(toBone_bytes);
            offset.ToBinary(writer);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notity_CharacterBeginMove : Proto.ISerializerable
    {
        public Notity_CharacterBeginMove()
        {
			StartPosition = new Vector3();
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
        public Vector3 StartPosition { set; get; }
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
            StartPosition = new Vector3();StartPosition.ParseFormBinary(reader);
            StartForward = new Vector3();StartForward.ParseFormBinary(reader);
            TargetPosition = new Vector3();TargetPosition.ParseFormBinary(reader);
            Speed = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            StartPosition.ToBinary(writer);
            StartForward.ToBinary(writer);
            TargetPosition.ToBinary(writer);
            writer.Write(Speed);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notity_CharacterStopMove : Proto.ISerializerable
    {
        public Notity_CharacterStopMove()
        {
			TargetForward = new Vector3();
TargetPosition = new Vector3();

        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 TargetForward { set; get; }
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
            TargetForward = new Vector3();TargetForward.ParseFormBinary(reader);
            TargetPosition = new Vector3();TargetPosition.ParseFormBinary(reader);
            Speed = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            TargetForward.ToBinary(writer);
            TargetPosition.ToBinary(writer);
            writer.Write(Speed);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notity_LayoutPlayMotion : Proto.ISerializerable
    {
        public Notity_LayoutPlayMotion()
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
            var Motion_bytes = Encoding.UTF8.GetBytes(Motion);writer.Write(Motion_bytes.Length);writer.Write(Motion_bytes);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Notity_LookAtCharacter : Proto.ISerializerable
    {
        public Notity_LookAtCharacter()
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
    public class Notity_LayoutPlayParticle : Proto.ISerializerable
    {
        public Notity_LayoutPlayParticle()
        {
			
        }
        /// <summary>
        /// 
        /// </summary>
        public long ReleaseIndex { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            ReleaseIndex = reader.ReadInt64();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(ReleaseIndex);
            
        }

    }
    /// <summary>
    /// 广播伤害
    /// </summary>
    public class Notity_EffectSubHP : Proto.ISerializerable
    {
        public Notity_EffectSubHP()
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
        /// 损失HP
        /// </summary>
        public int LostHP { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            TargetHP = reader.ReadInt32();
            LostHP = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(TargetHP);
            writer.Write(LostHP);
            
        }

    }
    /// <summary>
    /// 广播加血
    /// </summary>
    public class Notity_EffectAddHP : Proto.ISerializerable
    {
        public Notity_EffectAddHP()
        {
			
        }
        /// <summary>
        /// 
        /// </summary>
        public long Index { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int TargetHP { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int CureHP { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Index = reader.ReadInt64();
            TargetHP = reader.ReadInt32();
            CureHP = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(TargetHP);
            writer.Write(CureHP);
            
        }

    }
    /// <summary>
    /// 登陆
    /// </summary>
    public class C2S_Login : Proto.ISerializerable
    {
        public C2S_Login()
        {
			            LoginToken = string.Empty;

        }
        /// <summary>
        /// 登陆token
        /// </summary>
        public string LoginToken { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            LoginToken = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var LoginToken_bytes = Encoding.UTF8.GetBytes(LoginToken);writer.Write(LoginToken_bytes.Length);writer.Write(LoginToken_bytes);
            
        }

    }
    /// <summary>
    /// 登陆返回
    /// </summary>
    public class S2C_login : Proto.ISerializerable
    {
        public S2C_login()
        {
			            Session = string.Empty;

        }
        /// <summary>
        /// session
        /// </summary>
        public string Session { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Session = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var Session_bytes = Encoding.UTF8.GetBytes(Session);writer.Write(Session_bytes.Length);writer.Write(Session_bytes);
            
        }

    }
}