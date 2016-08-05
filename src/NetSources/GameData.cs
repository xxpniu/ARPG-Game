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
    public class Vector3 : Proto.ISerializerable
    {
        public Vector3()
        {
			
        }
        /// <summary>
        /// 
        /// </summary>
        public float x { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public float y { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public float z { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
            
        }

    }
}