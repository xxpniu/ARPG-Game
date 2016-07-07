using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ProtoParser
{
    public enum TestE
    {
        /// <summary>
        /// 
        /// </summary>
        E = 1,
    }
    public class Test : Proto.ISerializerable
    {
        public int ID { set; get; }
        public List<Test> IDS { set; get; }

        public Test TestID { set; get; }
        public Test()
        {
            ID = 0;
        }
        public string Str { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            ID = reader.ReadInt32();

            int IDS_Len = reader.ReadInt32();
            while(IDS_Len-->0)
            {
                var IDS_Temp = new Test();
                IDS_Temp.ParseFormBinary(reader);
                IDS.Add(IDS_Temp);
            }
            ID = reader.ReadInt16();
            var TestID_Temp = new Test();
            TestID_Temp.ParseFormBinary(reader);
            TestID = TestID_Temp;
            int xxLen = reader.ReadInt32();
            Str = Encoding.UTF8.GetString(reader.ReadBytes(xxLen));
            TestID = new Test();
            TestID.ParseFormBinary(reader);
        }

        public byte[] ToBinary(BinaryWriter writer)
        {
            writer.Write(IDS.Count);
            foreach(var i in IDS)
            {
                
            }
            return null;
        }
    }
}
