using System;
using Google.Protobuf;

public interface ISerializerableElement
{
    IMessage ToInitNotify();
}
