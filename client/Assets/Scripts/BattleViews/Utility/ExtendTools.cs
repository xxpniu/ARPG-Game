using System;
using Google.Protobuf;
using Proto;
using XNet.Libs.Net;

public static class ExtendTools
{
    public static bool IsOk(this ErrorCode er)
    {
        return er == ErrorCode.Ok;
    }


   
}
