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

        public delegate T Invoke< T, V>(V tI) where T : IMessage where V :IMessage;

        public class TaskBuilder<T> where T : IMessage
        {
            public TaskBuilder(Client cl, T t)
            {
                Client = cl;
                this.Task = t;
            }

            private T Task { set; get; }

            private int API { set; get; }

            private Client Client { set; get; }
            public bool Send()
            {
                if (Client?.Enable != true) return false;

                Client?.SendMessage(Task.ToTask());
                return true;
            }
        }

        public static TaskBuilder<T> CreateTask<T>(this Client client, T task)
            where T : IMessage
        {
            return new TaskBuilder<T>(client, task);
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

        public static IMessage AsTask(this Message msg)
        {
            if (msg.Class != MessageClass.Task) throw new Exception("only can paser notify");
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

        public static Message ToNotityMessage(this IMessage msg)
        {
            if (MessageTypeIndexs.TryGetIndex(msg.GetType(), out int index))
            {
                return new Message(MessageClass.Notify, index, 0, msg.ToByteArray());
            }
            throw new Exception($"No found index by type {msg.GetType()} {msg.ToString()}");
        }

        public static Message ToTask(this IMessage task)
        {
            if (MessageTypeIndexs.TryGetIndex(task.GetType(), out int index))
            {
                return new Message(MessageClass.Task, index, 0, task.ToByteArray());
            }
            throw new Exception($"No found index by type {task.GetType()} {task.ToString()}");

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
