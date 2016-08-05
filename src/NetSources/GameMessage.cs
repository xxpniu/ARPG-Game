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