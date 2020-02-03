using System;
using System.Reflection;
using Google.Protobuf;
using Proto;
using Proto.PServices;
using XNet.Libs.Net;

namespace XNet.Libs.Utility
{
    public static class NetProtoTool
    {
        public static bool EnableLog { get; set; } = false;

        public delegate T Invoke<T>(T tI) where T : IMessage;

        public class TaskBuilder<T> where T : IMessage
        {
            public TaskBuilder(Client cl, int api,T t)
            {
                this.API = api;
                Client = cl;
                this.Task = t;
            }

            private T Task { set; get; }

            private int API { set; get; }

            private Client Client { set; get; }
            public bool Send()
            {
                if (Client?.Enable != true) return false;
                var message = new Message(MessageClass.Task, API, 0, Task.ToByteArray());
                Client?.SendMessage(message);
                return true;
            }
        }

        public static TaskBuilder<T> CreateTask<T>(this Client client,Invoke<T> info, T task)
            where T : IMessage
        {
            var api = info.Method.GetCustomAttribute<APIAttribute>()?.ApiID;
            if (api.HasValue) return new TaskBuilder<T>(client, api.Value, task);
            return null;
        }

        public static Message ToNotityMessage(this IMessage msg)
        {
            if (MessageTypeIndexs.TryGetIndex(msg.GetType(), out int index))
            {
                return new Message(MessageClass.Notify, index, 0, msg.ToByteArray());
            }
            throw new Exception($"No found index by type {msg.GetType()} {msg.ToString()}");
        }

        public static IMessage AsAction(this Message msg)
        {
            if (msg.Class != MessageClass.Action) throw new Exception("only can paser notify");

            if (MessageTypeIndexs.TryGetType(msg.Flag, out Type t))
            {
                var ty = Activator.CreateInstance(t) as IMessage;
                ty.MergeFrom(msg.Content);
                return ty;
            }
            throw new Exception($"No found type by index  {msg.Flag}");
        }

        public static IMessage AsNotify(this Message msg)
        {
            if (msg.Class != MessageClass.Notify) throw new Exception("only can paser notify");

            if (MessageTypeIndexs.TryGetType(msg.Flag, out Type t))
            {
                var ty = Activator.CreateInstance(t) as IMessage;
                ty.MergeFrom(msg.Content);
                return ty;
            }
            throw new Exception($"No found type by index  {msg.Flag}");

        }

        public static Message ToAction(this IMessage msg)
        {
            if (MessageTypeIndexs.TryGetIndex(msg.GetType(), out int index))
            {
                return new Message(MessageClass.Action, index, 0, msg.ToByteArray());
            }
            throw new Exception($"No found index by type {msg.GetType()} {msg.ToString()}");
        }

    }
}
