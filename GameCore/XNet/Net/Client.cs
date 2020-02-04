using System;
using System.Net.Sockets;

namespace XNet.Libs.Net
{
    public delegate void OnClientDisconnect(Client client);

	/// <summary>
	/// 连接
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public class Client
	{
		/// <summary>
		/// 缓存大小
		/// </summary>
		public static int BUFFER_SIZE = 1024;

        private volatile bool IsClose;
		private Message _actionMessage;

		/// <summary>
		/// 连接ID
		/// </summary>
		public int ID { get; private set; }
		/// <summary>
		/// 是否关闭
		/// </summary>
		public bool Enable { get { return !IsClose; } }
		/// <summary>
		/// 连接断开事件 
		/// </summary>
		public event OnClientDisconnect OnDisconnect;
        /// <summary>
        /// have admission
        /// </summary>
		public bool HaveAdmission { get; set; }
		/// <summary>
		/// Gets or sets the last message time.
		/// </summary>
		/// <value>The last message time.</value>
		public DateTime LastMessageTime { set; get; }
        /// <summary>
        /// user state
        /// always use save sessionkey
        /// </summary>
		public object UserState { set; get; }
        /// <summary>
        /// Buffer
        /// </summary>
		public byte[] Buffer { private set; get; }
        /// <summary>
        /// Stream
        /// </summary>
		public MessageStream Stream { private set; get; }
        /// <summary>
        /// Server
        /// </summary>
		public SocketServer Server { private set; get; }

        /// <summary>
        /// socket
        /// </summary>
		public Socket Socket { private set; get; }

        /// <summary>
        /// create .ctor
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
		/// 关闭
		/// </summary>
		public void Close()
		{
			if (IsClose) return;
			OnDisconnect?.Invoke(this);
			IsClose = true;
			try { Socket?.Close(); }
			catch { }
			Socket = null;
		}

		/// <summary>
		/// 发送一个消息
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage(Message message)
		{
			if (IsClose) return;
			Server.SendMessage(this, message);
		}

        /// <summary>
        /// get last action message 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
		public bool TryGetActionMessage(out Message message)
		{
			message = _actionMessage;
			_actionMessage = null;
			return message != null;
		}

        /// <summary>
        /// save action
        /// </summary>
        /// <param name="action"></param>
        public void SetActionMessage(Message action)
        {
            _actionMessage = action;
        }
	}

}
