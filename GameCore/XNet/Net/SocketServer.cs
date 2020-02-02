using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XNet.Libs.Utility;

namespace XNet.Libs.Net
{
    /// <summary>
    /// Tcp 服务器
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public class SocketServer
    {
        /// <summary>
        /// 服务端连接
        /// </summary>
        /// <param name="client"></param>
        public virtual void OnConnect(Client client) { }
        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public virtual void OnReceivedMessag(Client client, Message msg)
        {
            client.LastMessageTime = DateTime.UtcNow;
            if (msg.Class == MessageClass.Ping)
            {
                SendMessage(client, msg);
            }
            else if (msg.Class == MessageClass.Package)
            {
                var package = MessageBufferPackage.ParseFromMessage(msg);
                foreach (var i in package.Messages)
                {
                    OnReceivedMessag(client, i);
                }
            }
            else if (msg.Class == MessageClass.Action)
            {
                client.SetActionMessage(msg);
            }
            else if (this.HandlerManager != null)
            {
                this.HandlerManager.Handle(msg, client);
            }
            else
            {
                Debuger.DebugLog("Server No Handler!");
            }
        }
        /// <summary>
        /// 关闭一个连接
        /// </summary>
        /// <param name="client"></param>
        public virtual void OnCloseConnection(Client client) { }
        public virtual void OnStart() { }
        /// <summary>
        /// 当前连接管理
        /// </summary>
        public ConnectionManager CurrentConnectionManager { set; get; }
        public int Port { set; get; }
        /// <summary>
        /// 消息处理管理
        /// </summary>
        public MessageHandlerManager HandlerManager { set; get; }
        public SocketServer(ConnectionManager cManager, int port)
        {
            CurrentConnectionManager = cManager;
            Port = port;
            MaxClient = 6000;
            BufferQueue = new MessageQueue<SendMessageBuffer>();
        }
        /// <summary>
        /// 消息最大字节数
        /// </summary>
        public static int MaxReceiveSize = 1024 * 1024;
        private ManualResetEvent MREvent = new ManualResetEvent(false);
        private ManualResetEvent SendMREvent = new ManualResetEvent(false);
        private Socket _socket;

        public void Start()
        {
            OnStart();
            IsWorking = true;
            CurrentConnectionManager.Clear();
            _socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //_socket.SendTimeout = 999;
            //_socket.ReceiveTimeout = 999;
            _socket.LingerState = new LingerOption(true, 10);
            _socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _socket.Listen(2000);

            AcceptThread = new Thread(new ThreadStart(ThreadRun));
            AcceptThread.IsBackground = true;
            AcceptThread.Start();

            SendThread = new Thread(new ThreadStart(SendMessage));
            SendThread.IsBackground = true;
            SendThread.Start();

            Debuger.DebugLog("Server Listen at port:"+Port);
        }

        private Thread SendThread;

        private Thread AcceptThread;

        private void ThreadRun()
        {
            while (IsWorking)
            {
                MREvent.Reset();
                _socket.BeginAccept(new AsyncCallback(OnAccpet), _socket);
                MREvent.WaitOne();
            }
        }

        public int MaxClient { get; private set; }

        private void OnAccpet(IAsyncResult ar)
        {
            MREvent.Set();
            if (!IsWorking) return; //server closed 
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);
            if (client == null) return; //client is empty

            client.NoDelay = true;
            //limit client 
            if (CurrentConnectionManager.Count >= MaxClient)
            {
                Debuger.LogWaring($"Client count  limit {CurrentConnectionManager.Count }/{MaxClient}");
                try
                {
                    client.Close();
                }
                finally { }
                return;
            }

            var nClient = CurrentConnectionManager.CreateClient(client, this);
            try
            {
                nClient.Socket.BeginReceive(nClient.Buffer, 0,
                    nClient.Buffer.Length,
                    SocketFlags.None,
                    new AsyncCallback(OnDataReceived), nClient);
                OnConnect(nClient);
            }
            catch (Exception ex)
            {
                HandleException(nClient, ex);
            }

        }

        private void OnDataReceived(IAsyncResult ar)
        {
            var nClient = ar.AsyncState as Client;
            try
            {
                if (!nClient.Enable) throw new Exception("Client had closed");
                int count = nClient.Socket.EndReceive(ar, out SocketError errorCode);
                if (errorCode != SocketError.Success) throw new Exception("Error Code:" + errorCode);
                if (count <= 0) throw new Exception("Clent receive No data!");

                nClient.Stream.Write(nClient.Buffer, 0, count);

                while (nClient.Stream.Read(out Message message))
                {
                    if (MaxReceiveSize == 0) OnReceivedMessag(nClient, message);
                }
                nClient?.Socket?.BeginReceive(nClient.Buffer, 0,
                        nClient.Buffer.Length,
                        SocketFlags.None,
                        new AsyncCallback(OnDataReceived), nClient);
            }
            catch (Exception ex)
            {
                HandleException(nClient, ex);
            }
        }

        private void OnSentData(IAsyncResult ar)
        {
            var client = ar.AsyncState as Client;
            if (client?.Enable != true) return;
            try
            {
                client.Socket.EndSend(ar);
            }
            catch (Exception ex)
            {
                HandleException(client, ex);
            }
        }

        /// <summary>
        /// 服务器是否启动
        /// </summary>
        public volatile bool IsWorking;
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (IsWorking)
            {
                IsWorking = false;

                Utility.Debuger.DebugLog("Socket Server:Stoping....");
                CurrentConnectionManager.Each((client) =>
                {
                    //服务器关闭
                    DisConnectClient(client, 0);
                    return false;
                });
                CurrentConnectionManager.Clear();

                MREvent.Set();
                SendMREvent.Set();
                AcceptThread.Join();
                SendThread.Join();
                SendThread = null;
                AcceptThread = null;
                _socket?.Close();
                _socket = null;

            }
        }

        private void SendMessage(Client client, byte[] msg)
        {
            if (client?.Enable!=true) return;
            try
            {
                client.Socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(OnSentData), client);
            }
            catch (Exception ex)
            {
                HandleException(client, ex);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void SendMessage(Client client, Message msg)
        {
            BufferQueue.AddMessage(new SendMessageBuffer(client, msg));
            SendMREvent.Set();
        }

        private void HandleException(Client client, Exception ex)
        {
            Debuger.DebugLog(ex.Message);
            RemoveClient(client);
        }

        private void RemoveClient(Client client)
        {
            client.Close();
            CurrentConnectionManager.RemoveClient(client);
            OnCloseConnection(client);
        }

        private void SendMessage()
        {
            while (IsWorking)
            {
                SendMREvent.Reset();
                var queue = (BufferQueue.GetMessage());
                while (queue != null && queue.Count > 0)
                {
                    var de = queue.Dequeue();
                    SendMessage(de.Client, de.Message.ToBytes());
                }
                SendMREvent.WaitOne();
            }
        }

        /// <summary>
        /// 发送缓存
        /// </summary>
        private MessageQueue<SendMessageBuffer> BufferQueue { set; get; }

        /// <summary>
        /// 当前消息数量
        /// </summary>
        public volatile int lastBufferSize;

        private bool HaveClient(Client client)
        {
            return CurrentConnectionManager.Exisit(client);
        }

        /// <summary>
        /// 关闭一个会话
        /// </summary>
        /// <param name="client"></param>
        /// <param name="code">关闭的时候显示的错误代码</param>
        public void DisConnectClient(Client client, byte code)
        {
            if (!HaveClient(client)) return;
            try
            {
                //发送一个关闭信息
                if (client.Socket.Connected)
                    this.SendMessage(client, new Message(MessageClass.Close,0,0,new byte[] { code } ).ToBytes());
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
            RemoveClient(client);
        }
    }


    /// <summary>
    /// 发送消息的缓存
    /// </summary>
    public class SendMessageBuffer
    {
        public SendMessageBuffer(Net.Client client, Net.Message msg)
        {
            this.Client = client;
            this.Message = msg;
        }
        public Client Client { set; get; }
        public Message Message { set; get; }
    }

}
