using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Proto;

namespace PNet
{
    public interface INetMessage
    {
        byte[] GetRequestBytes();
        void SetResponseFromBytes(byte[] bytes);
        byte[] GetResponseBytes();
        void SetRequestFromBytes(byte[] bytes);
    }

    public delegate void RequestCallBack<T>(T response) where T:ISerializerable;
    public abstract class NetMessage<Req, Res>:INetMessage
        where Req : ISerializerable, new()
        where Res :ISerializerable, new()
    {
        public NetMessage()
        {
            Request = new Req();
            Response = new Res();
        }
        public Req Request { private set; get; }
        public Res Response { private set; get; }

        public byte[] GetRequestBytes()
        {
            using(var mem = new MemoryStream())
            {
                using(var bw = new BinaryWriter(mem))
                {
                    Request.ToBinary(bw);
                }
                return mem.ToArray();
            }
        }

        public byte[] GetResponseBytes()
        {
            using(var mem = new MemoryStream())
            {
                using(var bw = new BinaryWriter(mem))
                {
                    Response.ToBinary(bw);
                }
                return mem.ToArray();
            }
        }
        public void SetResponseFromBytes(byte[] bytes)
        {
             using(var mem = new MemoryStream(bytes))
             {
                 using(var br = new BinaryReader(mem))
                 {
                     Response.ParseFormBinary(br);
                 }
             }
        }
        public void SetRequestFromBytes(byte[] bytes)
        {
             using(var mem = new MemoryStream(bytes))
             {
                 using(var br = new BinaryReader(mem))
                 {
                     Request.ParseFormBinary(br);
                 }
             }
        }

        public void SetRequest(Req request)
        {
            this.Request = request;
        }

        public void SetResponse(Res response)
        {
            this.Response = response;
        }
    }

    public class NetMessageAttribute : Attribute { }
}
