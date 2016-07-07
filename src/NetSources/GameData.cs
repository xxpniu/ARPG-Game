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
    /// 阵营
    /// </summary>
    public enum ArmyCamp
    {
        /// <summary>
        /// 玩家
        /// </summary>
        Player=1,
        /// <summary>
        /// 怪物
        /// </summary>
        Monster=2,

    }
    /// <summary>
    /// 
    /// </summary>
    public class Session : Proto.ISerializerable
    {
        public Session()
        {
			            UserName = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Time { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserName = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Time = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var UserName_bytes = Encoding.UTF8.GetBytes(UserName);writer.Write(UserName_bytes.Length);writer.Write(UserName_bytes);
            writer.Write(Time);
            
        }

    }
    /// <summary>
    /// 道具
    /// </summary>
    public class Item : Proto.ISerializerable
    {
        public Item()
        {
			
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { set; get; }
        /// <summary>
        /// 配表id
        /// </summary>
        public int Entry { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Diff { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Num = reader.ReadInt32();
            Entry = reader.ReadInt32();
            Diff = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Num);
            writer.Write(Entry);
            writer.Write(Diff);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class ItemPackage : Proto.ISerializerable
    {
        public ItemPackage()
        {
			            Items = new List<Item>();

        }
        /// <summary>
        /// 背包上限
        /// </summary>
        public int CountMax { set; get; }
        /// <summary>
        /// 当前所有道具
        /// </summary>
        public List<Item> Items { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            CountMax = reader.ReadInt32();
            int Items_Len = reader.ReadInt32();
            while(Items_Len-->0)
            {
                Item Items_Temp = new Item();
                Items_Temp = new Item();Items_Temp.ParseFormBinary(reader);
                Items.Add(Items_Temp );
            }
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(CountMax);
            writer.Write(Items.Count);
            foreach(var i in Items)
            {
                i.ToBinary(writer);               
            }
            
        }

    }
    /// <summary>
    /// 士兵
    /// </summary>
    public class Soldier : Proto.ISerializerable
    {
        public Soldier()
        {
			
        }
        /// <summary>
        /// 配表id
        /// </summary>
        public int ConfigID { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            ConfigID = reader.ReadInt32();
            Num = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(ConfigID);
            writer.Write(Num);
            
        }

    }
    /// <summary>
    /// 军队
    /// </summary>
    public class Army : Proto.ISerializerable
    {
        public Army()
        {
			            Soldiers = new List<Soldier>();

        }
        /// <summary>
        /// 士兵
        /// </summary>
        public List<Soldier> Soldiers { set; get; }
        /// <summary>
        /// 阵营
        /// </summary>
        public ArmyCamp Camp { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            int Soldiers_Len = reader.ReadInt32();
            while(Soldiers_Len-->0)
            {
                Soldier Soldiers_Temp = new Soldier();
                Soldiers_Temp = new Soldier();Soldiers_Temp.ParseFormBinary(reader);
                Soldiers.Add(Soldiers_Temp );
            }
            Camp = (ArmyCamp)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Soldiers.Count);
            foreach(var i in Soldiers)
            {
                i.ToBinary(writer);               
            }
            writer.Write((int)Camp);
            
        }

    }
}