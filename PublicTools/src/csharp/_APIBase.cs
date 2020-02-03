using System;
using Google.Protobuf;
using System.Threading.Tasks;
using System.Threading;

namespace Proto.PServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class APIAttribute : Attribute
    {
        public int ApiID { private set; get; }

        public APIAttribute(int api)
        {
            this.ApiID = api;
        }
    }

    public delegate void ReqeustCallback<Response>(Response res) where Response : IMessage, new();

    public interface IChannel
    {
        int ProcessRequest<Request, Response>(APIBase<Request, Response> api)
            where Request : IMessage, new()
            where Response : IMessage, new();
    }

    public interface IApiBase
    {
        int? RequestIndex { get; }

        void FinishResponse(IMessage msg);
    }

    public abstract class APIBase<Request, Response> : IApiBase
        where Request : IMessage, new()
        where Response : IMessage, new()
    {


        protected APIBase() { }

        public int API
        {
            get
            {
                var api = this.GetType().GetCustomAttributes(typeof(APIAttribute), false) as APIAttribute[];
                if (api.Length == 0) throw new Exception("No found api");
                var apiUrl = api[0].ApiID;
                return apiUrl;
            }
        }

        public Request QueryRequest { private set; get; }

        public Response QueryRespons { get; private set; }

        public ReqeustCallback<Response> Callback { private set; get; }

        public APIBase<Request, Response> SetRequest(Request req)
        {
            QueryRequest = req;
            return this;
        }

        public APIBase<Request, Response> SetCallBack(ReqeustCallback<Response> callback)
        {
            Callback = callback;
            return this;
        }

        public  APIBase<Request, Response> SendRequest(IChannel channel, Request request, ReqeustCallback<Response> callback)
        {
            return this.SetCallBack(callback).SetRequest(request).SendRequest(channel);
        }

        private APIBase<Request, Response> SendRequest(IChannel channel)
        {
            if (RequestIndex.HasValue) throw new Exception("Exsist request");
            IsDone = false;
            RequestIndex = channel.ProcessRequest(this);
            return this;
        }

        public APIBase<Request, Response> SetResponse(Response response)
        {
            this.QueryRespons = response;
            this.IsDone = true;
            Callback?.Invoke(QueryRespons);
            return this;
        }

        public bool IsDone { private set; get; }

        public int? RequestIndex { private set; get; }

        public void FinishResponse(IMessage message)
        {
            this.SetResponse((Response)message);
        }

        public float TimeOut{set;get;} = 10000; //10s

        public async Task<Response> SendAsync(IChannel channel, Request request)
        {
            this.SetRequest(request);
            SendRequest(channel);
            await Task.Factory.StartNew(() =>
            {
                var start = DateTime.Now;
                while (!IsDone)
                {
                    var cost = DateTime.Now - start ;
                    if(cost.TotalMilliseconds>TimeOut) break;
                    Thread.Sleep(100);
                }
            });
            return this.QueryRespons;
        }

        public Response GetResult(IChannel channel, Request request)
        {
            var task = this.SendAsync(channel,request);
            task.Wait(10000);
            return task.Result;
        }

        public System.Collections.IEnumerator Send(IChannel channel,Request request)
        {
            this.SetRequest(request);
            SendRequest(channel);
            while (!IsDone)  yield return null;
        }
    }

}
