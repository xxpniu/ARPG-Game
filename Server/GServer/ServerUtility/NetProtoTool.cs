using System;
using System.Reflection;
using Google.Protobuf;
using Proto.PServices;
using XNet.Libs.Net;

namespace ServerUtility
{
    public static class NetProtoTool
    {
        public static bool EnableLog { get; set; } = false;

        public delegate TO Invoke<TI, TO>(TI tI) where TI : IMessage where TO : IMessage;

        public class TaskBuilder<T> where T : IMessage
        {
            public TaskBuilder(Client cl, int api)
            {
                this.API = api;
                Client = cl;
            }

            private T Task { set; get; }

            private int API { set; get; }

            private Client Client { set; get; }

            public bool Send(Func<T> fun)
            {
                Task = fun.Invoke();
                return this.Send();
            }

            private bool Send()
            {
                if (Client?.Enable != true) return false;
                var message = new Message(MessageClass.Task, API, 0, Task.ToByteArray());
                Client?.SendMessage(message);
                return true;
            }
        }

        public static TaskBuilder<T> CreateTask<T>(this Client client,Invoke<T,T> info)
            where T : IMessage
        {
            var api = info.Method.GetCustomAttribute<APIAttribute>()?.ApiID;
            if (api.HasValue) return new TaskBuilder<T>(client, api.Value);
            return null;
        }

    }
}
