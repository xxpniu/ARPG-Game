using System;
using Google.Protobuf;
using Proto;
using XNet.Libs.Net;

public static class ExtendTools
{

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

    public static bool IsOk(this ErrorCode er)
    {
        return er == ErrorCode.Ok;
    }
}
