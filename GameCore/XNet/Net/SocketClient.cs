#define USETHREAD
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using XNet.Libs.Utility;
namespace XNet.Libs.Net
{
    public delegate void PingCallBack(long ticks);
    public delegate void ConnectCallBack(bool result);
    public delegate void DisconnectCallBack();

    /// <summary>
    /// 连接客户端
    /// @author:xxp
    /// @date:2020/02/03
    /// </summary>
    public class SocketClient
    {
        private Socket _socket;
        private readonly byte[] buffer = new byte[1024];
        private readonly ManualResetEvent updateEvent = new ManualResetEvent(false);
        private Dictionary<MessageClass, IServerMessageHandler> Handlers { set; get; }
        private readonly ConcurrentQueue<Action> Actions = new ConcurrentQueue<Action>();
        private long LastPingTime = 0;
        private MessageStream Stream { set; get; }

        private volatile bool isConnect = false;
        private volatile int SendBuffTotalSize;
        private volatile int ReceiveBuffTotalSize;
        private volatile bool IsWorking = false;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        private MessageQueue<Message> ReceiveBufferMessage { set; get; }
        private bool AutoUpdate = true;

        /// <summary>
        /// 网络延迟
        /// </summary>
        public long Delay { private set; get; }
        /// <summary>
        /// Receive and send 
        /// </summary>
        public int TotalSize { get { return ReceiveBuffTotalSize + SendBuffTotalSize; } }
        /// <summary>
        /// send size
        /// </summary>
        public int SendSize { get { return SendBuffTotalSize; } }
        /// <summary>
        /// receivesize
        /// </summary>
        public int ReceiveSize { get { return ReceiveBuffTotalSize; } }
        /// <summary>
        /// User state 
        /// </summary>
        public object UserState { set; get; }
        /// <summary>
        /// Ping 的间隔时间 毫秒
        /// </summary>
        public int PingDurtion = 3000; //
        /// <summary>
        /// 当前连接状态
        /// </summary>
        public bool IsConnect { get { return isConnect; } }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { private set; get; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { private set; get; }
        /// <summary>
        /// call connect 
        /// </summary>
        public ConnectCallBack OnConnectCompleted;
        /// <summary>
        /// Call after finish ping
        /// </summary>
        public PingCallBack OnPingCompleted;
        /// <summary>
        /// call after disconnect
        /// </summary>
        public DisconnectCallBack OnDisconnect;

        public SocketClient(int port, string ip, bool autoUpdate = true)
        {
            Port = port;
            IP = ip;
            Stream = new MessageStream();
            Handlers = new Dictionary<MessageClass, IServerMessageHandler>();
            ReceiveBufferMessage = new MessageQueue<Message>();
            Delay = 999;
            AutoUpdate = autoUpdate;
        }

        /// <summary>
        /// Reg handler
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void RegisterHandler(MessageClass type, IServerMessageHandler handler)
        {
            if (Handlers.ContainsKey(type)) throw new ExistHandlerException(type);
            Handlers.Add(type, handler);
        }

        #region Connect

        /// <summary>
        /// Connect begin async  call
        /// no wait
        /// same as { _ = ConnectAsync();}
        /// </summary>
        public void Connect()
        {
            _ = ConnectAsync();
        }

        /// <summary>
        /// Use async 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = 2000,
                ReceiveTimeout = 2000,
                NoDelay = true,
                Blocking = true
            };

            var address = await Dns.GetHostAddressesAsync(IP);
            IsWorking = true;
            if (AutoUpdate)
            {
                _ = Task.Factory.StartNew(() =>
                 {
                     while (IsWorking)
                     {
                         updateEvent.Reset();
                         Update();
                         updateEvent.WaitOne();
                     }
                     Update();
                 }, tokenSource.Token);
            }
            await Task.Factory.FromAsync(BeginConnect, EndConnect, address, Port, null);
            return IsConnect;
        }

        private IAsyncResult BeginConnect(IPAddress[] host, int port, AsyncCallback requestCallback, object state)
        {
            IAsyncResult result = _socket.BeginConnect(host,port, requestCallback, state);        
            return result;
        }

        private void EndConnect(IAsyncResult asyncResult)
        {
            var success = true;
            try
            {
                _socket.EndConnect(asyncResult);
                Task.Factory.FromAsync(BeginReceive, EndRecevie, null);
            }
            catch (Exception ex)
            {
                success = false;
                HandleException(ex);
            }
            isConnect = success;
            InvokeAsync(() => { OnConnectCompleted?.Invoke(success); });
        }

        #endregion

        #region Receive

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
                        this.Received(message);
                        updateEvent.Set();
                    }
                }
                if (IsConnect) Task.Factory.FromAsync(BeginReceive, EndRecevie, null);
            }
            else
            {
                throw new Exception("Error Code:" + errorCode);
            }
        }

        private void Received(Message message)
        {
            if (message.Class == MessageClass.Ping)
            {
                #region Ping

                using (var mem = new MemoryStream(message.Content))
                {
                    using (var br = new BinaryReader(mem))
                    {
                        Delay= DateTime.Now.Ticks - br.ReadInt64();
                    }
                }
                InvokeAsync(() => { OnPingCompleted?.Invoke(Delay); });
                #endregion
            }
            else if (message.Class == MessageClass.Package)
            {
                var bufferPackage = MessageBufferPackage.ParseFromMessage(message);
                foreach (var m in bufferPackage.Messages)
                {
                    Received(m);
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

        private void HandleMessage(Message message)
        {
            if (Handlers.ContainsKey(message.Class))
            {
                Handlers[message.Class].Handle(message, this);
            }
            else
            {
                Debuger.DebugLog("No handle Message!");
            }
        }
        #endregion

        #region Update 
        /// <summary>
        /// 刷新
        /// </summary>
        public void Update()
        {
            UpdateHandle();
        }

        private void InvokeAsync(Action invoke)
        {
            Actions.Enqueue(invoke);
            updateEvent.Set();
        }

        private void UpdateHandle()
        {
            while (Actions.Count > 0)
            {
                if (Actions.TryDequeue(out Action a))  a?.Invoke();
            }
            //处理Handle
            var received = ReceiveBufferMessage.GetMessage();
            while (received != null && received.Count > 0)
            {
                try
                {
                    HandleMessage(received.Dequeue());
                }
                catch (Exception ex)
                {
                    Debuger.LogError(ex.ToString());
                }
            }

            if (!isConnect) return;
            //time out 
            if (LastPingTime + PingDurtion * TimeSpan.TicksPerMillisecond < DateTime.Now.Ticks)
            {
                LastPingTime = DateTime.Now.Ticks;
                Ping();
            }
        }

        #endregion

        #region Send Message
        /// <summary>
        /// 发送一个消息
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            if (!isConnect) return;
            _ = Task.Factory.FromAsync(BeginSend, EndSend, message, null);
            updateEvent.Set();
        }

        private IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            byte[] sendBuffer = message.ToBytes();
            SendBuffTotalSize += sendBuffer.Length;
            try
            {
                return _socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, callback, state);
            }
            catch (Exception ex) { HandleException(ex); throw ex; }
        }
        private void EndSend(IAsyncResult result)
        {
            _socket.EndSend(result);
        }
        #endregion


        private void HandleException(Exception ex)
        {
            //Debuger.DebugLog(ex.ToString());
            Disconnect();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (isConnect)
            {
                InvokeAsync(() => { OnDisconnect?.Invoke(); });
                try { _socket?.Close(); }
                finally { }
                isConnect = false;
                IsWorking = false;
                tokenSource.CancelAfter(100);
                this._socket?.Close();
                this._socket = null;
            }
        }

        /// <summary>
        /// Ping 服务器
        /// </summary>
        public void Ping()
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(DateTime.Now.Ticks);
                }
                var message = new Message(MessageClass.Ping, 0, 0, mem.ToArray());
                SendMessage(message);
            }
        }
    }
}
