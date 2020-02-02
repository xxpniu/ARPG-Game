#define USETHREAD
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNet.Libs.Utility;

#pragma warning disable XS0001
namespace XNet.Libs.Net
{
    /// <summary>
    /// 连接客户端
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public class SocketClient
    {
        public delegate void PingCallBack(object sender, PingCompletedArgs args);
        public delegate void ConnectCallBack(object sender, ConnectCommpletedArgs args);
        public delegate void DisconnectCallBack(object sender, EventArgs args);

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { set; get; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string IP { set; get; }

        private Socket _socket;
        /// <summary>
        /// 连接完成
        /// </summary>
        public ConnectCallBack OnConnectCompleted;
        /// <summary>
        /// Ping 完成事件
        /// </summary>
        public PingCallBack OnPingCompleted;
        /// <summary>
        /// 断开连接
        /// </summary>
        public DisconnectCallBack OnDisconnect;


        private Dictionary<MessageClass, ServerMessageHandler> Handlers { set; get; }
        /// <summary>
        /// 注册一个消息处理者
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void RegisterHandler(MessageClass type, ServerMessageHandler handler)
        {
            if (Handlers.ContainsKey(type))
            {
                throw new ExistHandlerException(type);
            }
            Handlers.Add(type, handler);
            handler.Connection = this;
        }


        /// <summary>
        /// 获取处理者
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ServerMessageHandler GetHandler(MessageClass type)
        {
            Handlers.TryGetValue(type, out ServerMessageHandler handler);
            return handler;
        }

        private readonly byte[] buffer = new byte[1024];
        private readonly bool UsedThread = false;

        public SocketClient(int port, string ip, bool userThread)
        {
            UsedThread = userThread;
            Port = port;
            IP = ip;
            Stream = new MessageStream();
            Handlers = new Dictionary<MessageClass, ServerMessageHandler>();
            ReceiveBufferMessage = new MessageQueue<Message>();
            Delay = 999;
        }

        public SocketClient(int port, string ip) : this(port, ip, true) { }


       
        private IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            var point = new DnsEndPoint(host, port);
            IAsyncResult result = _socket.BeginConnect(point, requestCallback, state);        
            return result;
        }

        private Task ReciveTask;

        private void EndConnect(IAsyncResult asyncResult)
        {
            var success = true;
            try
            {
                _socket.EndConnect(asyncResult);
                Debuger.DebugLog($"EndConnect async");
                ReciveTask = Task.Factory.FromAsync(BeginReceive, EndRecevie, null);
                if (UsedThread) {
                    ProcessThread = new Thread(() => { UpdateProcess(); });
                }
            }
            catch (Exception ex)
            {
                success = false;
                HandleException(ex);
            }

            OnConnect(success);
            updateEvent.Set();
        }

        private IAsyncResult BeginReceive(AsyncCallback receiveCallback, object state)
        {
            return _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, receiveCallback, state);
        }

        private void EndRecevie(IAsyncResult asyncResult)
        {
            var count = _socket.EndReceive(asyncResult, out SocketError errorCode);
            ReceiveBuffTotalSize += count;
            if (errorCode == SocketError.Success)
            {
                if (count > 0)
                {
                    Stream.Write(buffer, 0, count);
                    while (Stream.Read(out Message message))
                    {
                        this.OnReceived(message);
                        updateEvent.Set();
                    }
                }
                ReciveTask = Task.Factory.FromAsync(BeginReceive, EndRecevie, null);
                //ReciveTask.Start();
            }
            else
            {
                throw new Exception("Error Code:" + errorCode);
            }
        }

        public void Connect()
        {
            var task = ConnectAsync();
            task.Wait();
        }

        private async Task<bool> ConnectAsync()
        {

            Debuger.DebugLog($"connect async");
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = 2000,
                ReceiveTimeout = 2000,
                NoDelay = true,
                Blocking = true
            };
           
            await Task.Factory.FromAsync(BeginConnect, EndConnect, IP, Port, null);
            return IsConnect;
        }

        private Thread ProcessThread;

        private MessageStream Stream { set; get; }

        private volatile bool isConnect = false;
        /// <summary>
        /// 当前连接状态
        /// </summary>
        public bool IsConnect { get { return isConnect; } }
        /// <summary>
        /// 连接成功后调用
        /// </summary>
        /// <param name="isSuccess"></param>
        public virtual void OnConnect(bool isSuccess)
        {
            isConnect = isSuccess;
            Task.Factory.StartNew(() => OnConnectCompleted?.Invoke(this, new ConnectCommpletedArgs { Success = isSuccess }));
        }


        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="message"></param>
        public virtual void OnReceived(Message message)
        {
            if (message.Class == MessageClass.Ping)
            {
                #region Ping
                var tickNow = DateTime.Now.Ticks;
                long tickSend = 0;
                using (var mem = new MemoryStream(message.Content))
                {
                    using (var br = new BinaryReader(mem))
                    {
                        tickSend = br.ReadInt64();
                    }
                }
                Delay = (tickNow - tickSend);
                Task.Factory.StartNew(() =>OnPingCompleted?.Invoke(this, new PingCompletedArgs { DelayTicks = Delay }));
                #endregion
            }
            else if (message.Class == MessageClass.Package)
            {
                var bufferPackage = MessageBufferPackage.ParseFromMessage(message);
                foreach (var m in bufferPackage.Messages)
                {
                    OnReceived(m);
                }
            }
            else if (message.Class == MessageClass.Close)
            {
                this.Disconnect();
            }
            else
            {
                ReceivedBufferMessage(message);
            }
        }

        private void ReceivedBufferMessage(Message message)
        {
            ReceiveBufferMessage.AddMessage(message);
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message"></param>
        private void HandleMessage(Message message)
        {
            if (Handlers.ContainsKey(message.Class))
            {
                Handlers[message.Class].Handle(message);
            }
            else {
                Debuger.DebugLog("No handle Message!");
            }
        }

        private ManualResetEvent updateEvent = new ManualResetEvent(false);
        /// <summary>
        /// 刷新
        /// </summary>
        public void Update()
        {
            OnUpdate();
            UpdateHandle();
        }

        public virtual void OnUpdate() { }

        private long LastPingTime = 0;

        /// <summary>
        /// Ping 的间隔时间 毫秒
        /// </summary>
        public int PingDurtion = 3000; //

        /// <summary>
        /// 发送一个消息
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            _ = DoSend(message);
            updateEvent.Set();
        }

        private async Task<bool> DoSend(Message message)
        {
            var t = Task.Factory.FromAsync(BeginSend, EndSend, message, null);
            await t;
            return !t.IsFaulted;
        }

        private IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            //if (!_socket.Connected) { callback?.Invoke( )return; };
            byte[] sendBuffer = message.ToBytes();
            SendBuffTotalSize += sendBuffer.Length;
            Debuger.DebugLog($"{message.Class} count {sendBuffer.Length}");
            return _socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, callback, state);
        }

        private void EndSend(IAsyncResult result)
        {
            _socket.EndSend(result);
        }
        /// <summary>
        /// 处理错误
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            Debuger.DebugLog(ex.ToString());
            Disconnect();
        }

        public virtual void OnClosed()
        { 
            
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (isConnect)
            {
                OnClosed();
                Task.Factory.StartNew(() => OnDisconnect?.Invoke(this, null));
                try
                {
                    _socket?.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Debuger.Log(e.ToString());
                }
                isConnect = false;
                ReciveTask?.Dispose();
                if (this.ProcessThread?.IsAlive == true)
                {
                    this.ProcessThread.Join(0);
                }

                this._socket?.Close();
                this._socket = null;
            }
        }


        private void UpdateProcess()
        {
            while (true)
            {
                updateEvent.Reset();
                Update();
                updateEvent.WaitOne();
            }
        }


        private void UpdateHandle()
        {
            //处理Handle
            var received = ReceiveBufferMessage.GetMessage();
            while (received != null && received.Count > 0)
            {
                HandleMessage(received.Dequeue());
            }
            //time out 
            if (LastPingTime + PingDurtion * TimeSpan.TicksPerMillisecond < DateTime.Now.Ticks)
            {
                LastPingTime = DateTime.Now.Ticks;
                Ping();
            }
            foreach (var handler in Handlers.Values) handler.Update();

        }

        public bool UseSendThreadUpdate = false;
        /// <summary>
        /// 网络延迟
        /// </summary>
        public long Delay { private set; get; }

        /// <summary>
        /// Ping 服务器
        /// </summary>
        public void Ping()
        {
            long tick = DateTime.Now.Ticks;
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(tick);
                }
                var message = new Message(MessageClass.Ping, 0, 0, mem.ToArray());
                SendMessage(message);
            }
        }


        private volatile int SendBuffTotalSize;
        private volatile int ReceiveBuffTotalSize;

        public int TotalSize { get { return ReceiveBuffTotalSize + SendBuffTotalSize; } }

        public int SendSize { get { return SendBuffTotalSize; } }

        public int ReceiveSize { get { return ReceiveBuffTotalSize; } }

        private MessageQueue<Message> ReceiveBufferMessage { set; get; }

        public object UserState { set; get; }


    }
}
