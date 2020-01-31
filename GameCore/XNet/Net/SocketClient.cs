#define USETHREAD
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            ServerMessageHandler handler;
            Handlers.TryGetValue(type, out handler);
            return handler;
        }

        private byte[] buffer = new byte[1024];
        private bool UsedThread = false;

        public SocketClient(int port, string ip, bool userThread)
        {
            UsedThread = userThread;
            Port = port;
            IP = ip;
            Stream = new MessageStream();
            Handlers = new Dictionary<MessageClass, ServerMessageHandler>();
            BufferMessage = new MessageQueue<Message>();
            ReceiveBufferMessage = new MessageQueue<Message>();
            Delay = 999;
        }

        public SocketClient(int port, string ip) : this(port, ip, true) { }

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            if (isConnect)
            {
                throw new Exception("Is connecting!");
            }
            IPAddress[] dsn;
            try
            {
                dsn = Dns.GetHostAddresses(IP);
            }
            catch
            {
                OnConnect(false);
                return;
            }
            {
                var address = dsn[0];
                _socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.SendTimeout = 2000;
                _socket.ReceiveTimeout = 2000;
                _socket.NoDelay = true;
                _socket.Blocking = true;
                _socket.BeginConnect(address, Port, _connectCallBack, _socket);
            }
        }

        private void _connectCallBack(IAsyncResult result)
        {
            try
            {
                _socket.EndConnect(result);
                OnConnect(true);

                _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, _receivedCallBack, _socket);
                //处理多线程  可以不使用多线程发送 服务器一般使用
                if (UsedThread)
                {
                    ProcessThread = new Thread(this.UpdateProcess);
                    ProcessThread.IsBackground = true;
                    ProcessThread.Start();
                }
            }
            catch (Exception ex)
            {
                Debuger.DebugLog(ex.Message);
                OnConnect(false);
                if (UsedThread)
                    Update();
            }
        }
        private void _receivedCallBack(IAsyncResult result)
        {
            //_socket.Blocking = false;
            int count = 0;
            SocketError errorCode;
            try
            {
                if (!this.IsConnect) return;

                count = _socket.EndReceive(result, out errorCode);
                ReceiveBuffTotalSize += count;
                if (errorCode == SocketError.Success)
                {
                    if (count > 0)
                    {
                        Stream.Write(buffer, 0, count);
                    }
                    else
                    {
                        throw new Exception("Client receive No data!");
                    }
                    Message message;
                    while (Stream.Read(out message))
                    {
                        this.OnReceived(message);
                    }
                    if (_socket != null)
                    {
                        _socket.BeginReceive(buffer, 0,
                                buffer.Length, SocketFlags.None, _receivedCallBack, _socket);

                    }
                }
                else
                {
                    throw new Exception("Error Code:" + errorCode);
                }
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex.Message);
                Disconnect();
            }
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
            SyncCall.Add(() =>
                {
                    if (OnConnectCompleted != null)
                    {
                        OnConnectCompleted(this, new ConnectCommpletedArgs { Success = isSuccess });
                    }
                });

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
                SyncCall.Add(() =>
                {
                    if (OnPingCompleted != null)
                    {
                        OnPingCompleted(this, new PingCompletedArgs { DelayTicks = Delay });
                    }
                });
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
            else {
                ReceivedBufferMessage(message);
            }
            if (UsedThread)
                SendEvent.Set();
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
                Utility.Debuger.DebugLog("No handle Message!");
            }
        }



        /// <summary>
        /// 刷新
        /// </summary>
        public void Update()
        {
            OnUpdate();
            UpdateHandle();
            if (!UsedThread)
                DoWork();
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
            BufferMessage.AddMessage(message);
            if (UsedThread)
            {
                SendEvent.Set();
            }
        }

        private bool DoSend(Message message)
        {
            if (!isConnect) return false;
            byte[] sendBuffer = message.ToBytes();
            try
            {
                if (_socket.Connected)
                {
                    SendBuffTotalSize += sendBuffer.Length;
                    _socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, _sendCallBack, _socket);
                    return true;
                }
                else
                {
                    HandleException(new Exception("Disconnected"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private void _sendCallBack(IAsyncResult result)
        {
            try
            {
                _socket.EndSend(result);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        /// <summary>
        /// 处理错误
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            Disconnect();
        }

        protected SyncList<Action> SyncCall = new SyncList<Action>();

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
                SyncCall.Add(() =>
                {
                    if (OnDisconnect == null) return;
                    OnDisconnect(this, new EventArgs());
                });
                try
                {
                    this._socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Debuger.Log(e.ToString());
                }
                isConnect = false;
                if (this.ProcessThread != null && this.ProcessThread.IsAlive)
                {
                    SendEvent.Set();
                    ProcessThread.Join(1000);
                }
                Close();
            }
        }

        private void Close()
        {
            //this._socket.Disconnect(true);

            this._socket.Close();
            this._socket = null;
        }

        private ManualResetEvent SendEvent = new ManualResetEvent(false);

        private void UpdateProcess()
        {
            while (true)
            {
                SendEvent.Reset();
                if (UseSendThreadUpdate)
                    Update();
                //发送数据
                DoWork();
                if (!isConnect) break;
                SendEvent.WaitOne();
            }
            if (UseSendThreadUpdate)
                Update();
        }

        private void DoWork()
        {
            var messages = BufferMessage.GetMessage();
            while (messages != null && messages.Count > 0)
            {
                var m = messages.Dequeue();
                DoSend(m);
            }
        }

        private void UpdateHandle()
        {
            //处理Handle
            var received = ReceiveBufferMessage.GetMessage();
            if (received != null && received.Count > 0)
            {
                while (received.Count > 0)
                {
                    HandleMessage(received.Dequeue());
                }
            }
            //time out 
            if (LastPingTime + PingDurtion * TimeSpan.TicksPerMillisecond < DateTime.Now.Ticks)
            {
                LastPingTime = DateTime.Now.Ticks;
                Ping();
            }
            foreach (var handler in Handlers.Values)
                handler.Update();

            if (SyncCall.Count > 0)
            {
                var list = SyncCall.ToList();
                SyncCall.Clear();
                foreach (var i in list)
                {
                    i();
                }
            }
        }

        public bool UseSendThreadUpdate = false;

        private MessageQueue<Message> BufferMessage { set; get; }
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
            byte[] bytes;
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(tick);
                }
                bytes = mem.ToArray();
            }
            var message = new Message(MessageClass.Ping, 0,0, bytes);
            SendMessage(message);
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
