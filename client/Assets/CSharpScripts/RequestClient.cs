using System;
using XNet.Libs.Net;
using System.Collections.Generic;
using Proto;
using System.IO;
using System.Text;
using org.vxwo.csharp.json;
using XNet.Libs.Utility;
using UnityEngine;
using System.Reflection;

public class ServerTaskAttribute : Attribute
{
    public ServerTaskAttribute(Type t)
    {
        TaskType = t;
    }

    public Type TaskType { set; get; }
}

public abstract class TaskHandler<T> where T:class, Proto.ISerializerable, new()
{
    public abstract void DoTask(T task);
}

public class BattleNotifyHandler:ServerMessageHandler
{
    public BattleNotifyHandler(BattleGate gate)
    {
        Gate = gate;
    }
    private BattleGate Gate; 

    #region implemented abstract members of ServerMessageHandler

    public override void Handle(Message message)
    {
        Type notifyType = MessageHandleTypes.GetTypeByIndex(message.Flag);
        ISerializerable notify;
        using (var mem = new MemoryStream(message.Content))
        {
            using (var br = new BinaryReader(mem))
            {
                notify=Activator.CreateInstance(notifyType) as Proto.ISerializerable;
                notify.ParseFormBinary(br);
                #if NETDEBUG
                Debug.Log(notify.GetType()+"-->"+JsonTool.Serialize(notify));
                #endif
            }
        }
        Gate.ProcessNotify(notify);
    }

    #endregion
}


public class RequestClient:SocketClient
{
    public interface IHandler
    {
        void OnHandle(bool success,ISerializerable message);

        void OnTimeOut();

        bool IsTimeOut{ get; }
    }

    public class Request<S, R> :IHandler 
        where S : class,ISerializerable, new() 
        where R :class,ISerializerable , new()
    {

        private volatile bool completed =false;
        private float start =0f;

        public void SendRequest()
        {
            start = Time.time;
            UUIManager.S.MaskEvent();
            if (completed)
                return;
            if (!Client.IsConnect)
            {
                OnHandle(false, new R());
            }
            else
            {
                if (Client.AttachRequest(this, Index))
                    this.Client.SendRequest(this.RequestMessage, this.Index);
                else
                {
                    OnHandle(false, new R());
                }
            }
        }
            

        public void OnHandle(bool success,ISerializerable message)
        {
            UUIManager.S.UnMaskEvent();
            completed = true;
            if (OnCompleted != null)
            {
                OnCompleted(success,message as R);
            }
        }

        public void OnTimeOut()
        {
            OnHandle(false, new R());
        }

        public Request(RequestClient client, int index)
        {
            Client = client;
            RequestMessage = new S();
            Index = index;
            completed = false;
        }

        public int Index { private set; get; }

        public S RequestMessage { private set; get; }

        private RequestClient Client { set; get; }

        public Action<bool,R> OnCompleted;

        public bool IsTimeOut{ get { return start + 15 < Time.time; } }

    }

    private class ResponseHandler : ServerMessageHandler
    {
        public Dictionary<int, IHandler> _handlers = new Dictionary<int, IHandler>();
        public Dictionary<int, Type> _taskHandler = new Dictionary<int, Type>();

        private Queue<int> _del = new Queue<int>();
        public override void Update()
        {
            base.Update();
            foreach (var i in _handlers)
            {
                if (i.Value.IsTimeOut)
                {
                    i.Value.OnTimeOut();
                    _del.Enqueue(i.Key);
                }
            }
            while (_del.Count > 0)
                _handlers.Remove(_del.Dequeue());
        }

        public override void Handle(Message message)
        {
            int requestIndex = 0;
            Type responseType = MessageHandleTypes.GetTypeByIndex(message.Flag);
            ISerializerable response;
            using (var mem = new MemoryStream(message.Content))
            {
                using (var br = new BinaryReader(mem))
                {
                    if (message.Class == MessageClass.Response)
                    {
                        requestIndex = br.ReadInt32();
                    }
        
                    response=Activator.CreateInstance(responseType) as Proto.ISerializerable;
                    response.ParseFormBinary(br);
                    #if NETDEBUG
                    Debug.Log(response.GetType()+"-->"+JsonTool.Serialize(response));
                    #endif
                }
            }

            if (message.Class == MessageClass.Response)
            {
                IHandler handler;
                if (_handlers.TryGetValue(requestIndex, out handler))
                {
                    _handlers.Remove(requestIndex);
                    handler.OnHandle(true, response);
                }
            }
            else if (message.Class == MessageClass.Task)
            {
                Type handlerType;
                if (_taskHandler.TryGetValue(message.Flag, out handlerType))
                {
                    var handler = Activator.CreateInstance(handlerType);
                    var m = handlerType.GetMethod("DoTask");
                    m.Invoke(handler, new object[] { response});
                }
                else {
                    Debuger.LogError("NoHanlderType:" + message.Flag);
                }
            }
        }
    }

    public RequestClient(string host, int port)
        : base(port, host,false)
    {
        Handler = new ResponseHandler();
        this.RegisterHandler(MessageClass.Response, Handler);
        this.RegisterHandler(MessageClass.Task, Handler);
        UseSendThreadUpdate = false;
    }

    public void RegAssembly(Assembly assembly)
    {
        foreach (var i in assembly.GetTypes())
        {
            var atts = i.GetCustomAttributes(typeof(ServerTaskAttribute),false) as ServerTaskAttribute[];
            if (atts == null||atts.Length ==0) continue;
            var index =0;
            if (Proto.MessageHandleTypes.GetTypeIndex(atts[0].TaskType, out index))
                Handler._taskHandler.Add(index, i);
        }
    }

    private ResponseHandler Handler;

    private volatile int lastIndex = 0;

    public Request<S, R> CreateRequest<S, R>() where S : class, Proto.ISerializerable, new() where R : class, Proto.ISerializerable, new()
    {
        if (lastIndex == int.MaxValue)
            lastIndex = 0;
        var req = new Request<S, R>(this, lastIndex++);
        return req;
    }


        
    private void SendRequest(Proto.ISerializerable request, int requestIndex)
    {
        var index = 0;
        if (MessageHandleTypes.GetTypeIndex(request.GetType(), out index))
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(requestIndex);
                    request.ToBinary(bw);
                    #if NETDEBUG
                    Debug.Log(request.GetType()+"-->"+JsonTool.Serialize(request));
                    #endif
                }
                var result = new Message(MessageClass.Request, index, mem.ToArray());
                SendMessage(result);
            }
        }
    }

    private bool AttachRequest(IHandler hander, int requestIndex)
    {
        if (Handler._handlers.ContainsKey(requestIndex))
            return false;
        Handler._handlers.Add(requestIndex, hander);
        return true;
    }


    public override void OnClosed()
    {
        base.OnClosed();
        foreach (var  i in Handler._handlers)
        {
            i.Value.OnTimeOut();
        }
        Handler._handlers.Clear();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //Handler.Update();
    }

    public static Message ToMessage(MessageClass @class,Proto.ISerializerable m)
    {
        var index = 0;
        if (MessageHandleTypes.GetTypeIndex(m.GetType(), out index))
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    m.ToBinary(bw);
                }

                return new Message(@class, index, mem.ToArray());
            }
        }
        return null;
    }
}


