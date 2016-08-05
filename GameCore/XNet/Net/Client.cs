using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using XNet.Libs.Utility;

namespace XNet.Libs.Net
{
	/// <summary>
	/// 连接
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public class Client
	{

		/// <summary>
		/// 连接ID
		/// </summary>
		public int ID { get; set; }
		/// <summary>
		/// 缓存大小
		/// </summary>
		public static int BUFFER_SIZE = 1024;
		/// <summary>
		/// 是否已经关闭/断开连接
		/// </summary>
		public volatile bool IsClose;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		/// <param name="client"></param>
		/// <param name="id"></param>
		public Client (SocketServer server, Socket client, int id)
		{
			ID = id;
			Buffer = new byte[BUFFER_SIZE];
			Stream = new MessageStream ();
			IsClose = false;
			Server = server;
			Socket = client;
			HaveAdmission = false;
		}
		/// <summary>
		/// 当前连接的socket
		/// </summary>
		public Socket Socket { private set; get; }
		/// <summary>
		/// 缓存
		/// </summary>
		public byte[] Buffer { set; get; }
		/// <summary>
		/// 消息缓存
		/// </summary>
		public MessageStream Stream { set; get; }
		/// <summary>
		/// 关闭
		/// </summary>
		public void Close ()
		{
			if (IsClose)
				return;
			IsClose = true;
			if (Socket != null) {
				try{
					Socket.Shutdown (SocketShutdown.Both);
					Socket.Close ();
				}catch(Exception ex){
					Debuger.Log(ex.Message);
				}
				Socket = null;
			}
			if (OnDisconnect != null) {
				OnDisconnect (this, new EventArgs ());
			}
		}
		/// <summary>
		/// 当前服务器
		/// </summary>
		public SocketServer Server { set; get; }
		/// <summary>
		/// 发送一个消息
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage (Message message)
		{
			if (Server.IsWorking) {
				if (!IsClose)
					Server.SendMessage (this, message);
			}
		}
		/// <summary>
		/// 连接断开事件 
		/// </summary>
		public event EventHandler<EventArgs> OnDisconnect;
		
		public bool HaveAdmission {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the state of the user.
		/// </summary>
		/// <value>The state of the user.</value>
		public object UserState{ set; get; }

		/// <summary>
		/// Gets or sets the last message time.
		/// </summary>
		/// <value>The last message time.</value>
		public DateTime LastMessageTime{ set; get; }
	}

}
