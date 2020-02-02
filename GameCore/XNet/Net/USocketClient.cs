using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using XNet.Libs.Utility;

namespace XNet.Libs.Net
{
    public class USocketClient
    {
        #region delegate

        public delegate void PingCallBack(object sender, PingCompletedArgs args);
        public delegate void ConnectCallBack(object sender, ConnectCommpletedArgs args);
        public delegate void DisconnectCallBack(object sender, EventArgs args);

        #endregion

        #region Fields 

        private string ip;

        private int port;

        private long LastPingTime = DateTime.Now.Ticks;

        private Dictionary<MessageClass, ServerMessageHandler> handlers;

        private Thread sendThread;

        private Thread receiveThread;

        private readonly ManualResetEvent sendMEvent = new ManualResetEvent(false);

        protected SyncList<Action> syncCall = new SyncList<Action>();

        private readonly MessageQueue<Message> receiveQueue = new MessageQueue<Message>();
        private readonly MessageQueue<Message> sendQueue = new MessageQueue<Message>();
        private readonly byte[] ReceiveBuffer = new byte[1024];
        private readonly MessageStream stream = new MessageStream();
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

        #endregion

        public USocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            PingDurtion = 3000;
            handlers = new Dictionary<MessageClass, ServerMessageHandler>();
        }

        #region public mothed

        public long PingDurtion { set; get; }
        public long Delay { private set; get; }
        public int ReceiveSize { private set; get; }
        public int SendSize { private set; get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:XNet.Libs.Net.USocketClient"/> is connected.
        /// </summary>
        /// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
        public bool IsConnect { get; private set; } = false;

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void Connect() 
        {
            TryToConnect();
        }

        /// <summary>
        /// 发送message
        /// </summary>
        /// <param name="msg">Message.</param>
        public void SendMessage(Message msg) 
        { 
            sendQueue.AddMessage(msg);
            sendMEvent.Set();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect() 
        {
            TryDisconnect();
        }

        /// <summary>
        /// 注册一个消息处理者
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void RegisterHandler(MessageClass type, ServerMessageHandler handler)
        {
            if (handlers.ContainsKey(type))
            {
                throw new ExistHandlerException(type);
            }

            handlers.Add(type, handler);
        }

        protected virtual void OnClose() { }

        /// <summary>
        /// Update
        /// </summary>
        public void Update() 
        {
            UpdateHandles();
        }

        public void Ping()
        {
            long tick = DateTime.Now.Ticks;
            byte[] bytes;
            using (MemoryStream mem = new MemoryStream())
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

        #endregion

        #region private 

        private void ConnectCompleted(bool success)
        {
            OnConnectCompleted?.Invoke(this, new ConnectCommpletedArgs() { Success = success });
        }

        private void TryToSend() 
        {
            try
            {
                sendMEvent.WaitOne();
                var send = sendQueue.GetMessage();
                while (send.Count > 0)
                {
                    var m = send.Dequeue();
                    var buff = m.ToBytes();
                    _socket.Send(buff);
                    SendSize += buff.Length;
                }
                sendMEvent.Reset();
            }
            catch (Exception ex)
            {
                HandleException(ex); 
            }
        }

        private void TryToReceive()
        {
            try
            {
                int count = _socket.Receive(ReceiveBuffer);
                if (count == 0)
                {
                    throw new Exception("Received no data");
                }
                stream.Write(ReceiveBuffer,0,count);
                ReceiveSize += count;
                while (stream.Read(out Message message))
                {
                    OnReceived(message);
                }
            }
            catch (Exception ex) 
            { 
                HandleException(ex);
            }
        }

        private void TryToConnect()
        {
            try
            {
                var dns = Dns.GetHostAddresses(ip);
                _socket = new Socket(dns[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    Blocking = true,
                    SendTimeout = 9999,
                    ReceiveTimeout = 9999
                };
                _socket.Connect(dns[0],port);

                IsConnect = true;
                syncCall.Add(
                    () =>
                    {
                    ConnectCompleted(IsConnect);
                    });
                receiveThread = new Thread(() =>
                {
                    while (IsConnect)
                    {
                        TryToReceive();
                    }
                })
                {
                    IsBackground = true
                };
                receiveThread.Start();

                sendThread = new Thread(() =>
                {
                    while (IsConnect)
                    {
                        TryToSend();
                    }
                })
                {
                    IsBackground = false
                };
                sendThread.Start();
            }
            catch (Exception ex)
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
                IsConnect = false;
                syncCall.Add(
                    () =>
                    {
                    ConnectCompleted(IsConnect);
                    });
                HandleException(ex);
            }
        }

        private void UpdateHandles() 
        {
            if (syncCall.Count > 0) 
            {
                var list = syncCall.ToList();
                syncCall.Clear();
                foreach (var i in list) 
                {
                    i();
                }
            }

            foreach (var i in handlers) {
                i.Value.Update();
            }

            if (LastPingTime + PingDurtion * TimeSpan.TicksPerMillisecond < DateTime.Now.Ticks)
            {
                LastPingTime = DateTime.Now.Ticks;
                Ping();
            }

            var received = receiveQueue.GetMessage();
            while (received.Count>0) 
            {
                var msg = received.Dequeue();
                if (handlers.TryGetValue(msg.Class, out ServerMessageHandler handler))
                {
                    handler.Handle(msg);
                }
            }
        }

        private void HandleException(Exception ex) 
        {
            Debuger.LogError(ex.Message);
            Debuger.LogWaring(ex.ToString());
            TryDisconnect();
        }

        private void TryDisconnect()
        {
            if (!IsConnect) return;
            Debuger.Log("Try disconnect");
            IsConnect = false;
            try
            {
                sendMEvent.Set();
                sendThread.Join(1000);
                receiveThread.Join(1000);
            }
            finally
            {
                sendThread = null;
                receiveThread = null;
            }
            try
            {
                //_socket.Disconnect(false);
                _socket.Shutdown( SocketShutdown.Both);
            }
            catch(Exception ex)
            {
                Debuger.LogError(ex);
            }
            _socket.Close();
            _socket = null;

            OnClose();

            syncCall.Add(() => 
            {
                if (this.OnDisconnect == null) return;
                OnDisconnect(this, new EventArgs());
            });
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="message"></param>
        private  void OnReceived(Message message)
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
                syncCall.Add(() =>
                {
                    OnPingCompleted?.Invoke(this, new PingCompletedArgs { DelayTicks = Delay });
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
                receiveQueue.AddMessage(message);
            }
        }


        #endregion
    }
}
