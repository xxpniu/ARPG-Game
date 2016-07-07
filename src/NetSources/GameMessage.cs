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
    /// 注册游戏服务器
    /// </summary>
    public class RegServer : Proto.ISerializerable
    {
        public RegServer()
        {
			            ListenIP = string.Empty;

        }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { set; get; }
        /// <summary>
        /// 监听ip
        /// </summary>
        public string ListenIP { set; get; }
        /// <summary>
        /// 最大访问数
        /// </summary>
        public int MaxClient { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Port = reader.ReadInt32();
            ListenIP = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            MaxClient = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Port);
            var ListenIP_bytes = Encoding.UTF8.GetBytes(ListenIP);writer.Write(ListenIP_bytes.Length);writer.Write(ListenIP_bytes);
            writer.Write(MaxClient);
            
        }

    }
    /// <summary>
    /// 上传服务器状态
    /// </summary>
    public class ReportServerStatus : Proto.ISerializerable
    {
        public ReportServerStatus()
        {
			
        }
        /// <summary>
        /// 当前访问数
        /// </summary>
        public int CurrentClient { set; get; }
        /// <summary>
        /// 最大访问数
        /// </summary>
        public int MaxClient { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            CurrentClient = reader.ReadInt32();
            MaxClient = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(CurrentClient);
            writer.Write(MaxClient);
            
        }

    }
    /// <summary>
    /// 游戏中的会话
    /// </summary>
    public class GameSession : Proto.ISerializerable
    {
        public GameSession()
        {
			            SessionKey = string.Empty;

        }
        /// <summary>
        /// 会话id
        /// </summary>
        public int SessionID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string SessionKey { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            SessionID = reader.ReadInt32();
            UserID = reader.ReadInt32();
            SessionKey = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(SessionID);
            writer.Write(UserID);
            var SessionKey_bytes = Encoding.UTF8.GetBytes(SessionKey);writer.Write(SessionKey_bytes.Length);writer.Write(SessionKey_bytes);
            
        }

    }
    /// <summary>
    /// 游戏中的玩家
    /// </summary>
    public class GameUser : Proto.ISerializerable
    {
        public GameUser()
        {
			            Name = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserID = reader.ReadInt32();
            Name = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(UserID);
            var Name_bytes = Encoding.UTF8.GetBytes(Name);writer.Write(Name_bytes.Length);writer.Write(Name_bytes);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class C2S_Login : Proto.ISerializerable
    {
        public C2S_Login()
        {
			            Token = string.Empty;

        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Token { set; get; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType Type { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Token = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Type = (UserType)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var Token_bytes = Encoding.UTF8.GetBytes(Token);writer.Write(Token_bytes.Length);writer.Write(Token_bytes);
            writer.Write((int)Type);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class S2C_Login : Proto.ISerializerable
    {
        public S2C_Login()
        {
			            SessionKey = string.Empty;
Session = new GameSession();
            User = new List<GameUser>();

        }
        /// <summary>
        /// 
        /// </summary>
        public bool Success { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string SessionKey { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public GameSession Session { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<GameUser> User { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int ActiveUserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
            SessionKey = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Session = new GameSession();Session.ParseFormBinary(reader);
            int User_Len = reader.ReadInt32();
            while(User_Len-->0)
            {
                GameUser User_Temp = new GameUser();
                User_Temp = new GameUser();User_Temp.ParseFormBinary(reader);
                User.Add(User_Temp );
            }
            ActiveUserID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Success);
            var SessionKey_bytes = Encoding.UTF8.GetBytes(SessionKey);writer.Write(SessionKey_bytes.Length);writer.Write(SessionKey_bytes);
            Session.ToBinary(writer);
            writer.Write(User.Count);
            foreach(var i in User)
            {
                i.ToBinary(writer);               
            }
            writer.Write(ActiveUserID);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class C2S_RegUser : Proto.ISerializerable
    {
        public C2S_RegUser()
        {
			Session = new GameSession();
            Name = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public GameSession Session { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Session = new GameSession();Session.ParseFormBinary(reader);
            Name = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            Session.ToBinary(writer);
            var Name_bytes = Encoding.UTF8.GetBytes(Name);writer.Write(Name_bytes.Length);writer.Write(Name_bytes);
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class S2C_RegUser : Proto.ISerializerable
    {
        public S2C_RegUser()
        {
			            Users = new List<GameUser>();

        }
        /// <summary>
        /// 
        /// </summary>
        public bool Success { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<GameUser> Users { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
            UserID = reader.ReadInt32();
            int Users_Len = reader.ReadInt32();
            while(Users_Len-->0)
            {
                GameUser Users_Temp = new GameUser();
                Users_Temp = new GameUser();Users_Temp.ParseFormBinary(reader);
                Users.Add(Users_Temp );
            }
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Success);
            writer.Write(UserID);
            writer.Write(Users.Count);
            foreach(var i in Users)
            {
                i.ToBinary(writer);               
            }
            
        }

    }
}