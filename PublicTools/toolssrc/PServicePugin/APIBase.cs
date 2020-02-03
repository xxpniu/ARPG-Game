using System;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Proto.PServices
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
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
        void ProcessRequest<Request, Response>(APIBase<Request, Response> api)
            where Request : IMessage, new()
            where Response : IMessage, new();
    }

    public abstract class APIBase<Request, Response>
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

        public APIBase<Request, Response> SendRequest(IChannel channel, Request request, ReqeustCallback<Response> callback)
        {
            return this.SetCallBack(callback).SetRequest(request).SendRequest(channel);
        }

        public APIBase<Request, Response> SendRequest(IChannel channel)
        {
            IsDone = false;
            channel.ProcessRequest(this);
            return this;
        }

        public APIBase<Request, Response> SetResponse(Response response)
        {
            this.QueryRespons = response;
            this.IsDone = true;
            return this;
        }

        public async Task<Response> SendRequestAsync(IChannel channel, Request request)
        {
            this.SetRequest(request);
            SendRequest(channel);
            while (!IsDone)
            {
                await Task.Yield();
            }
            return this.QueryRespons;

        }

        public bool IsDone { private set; get; }
    }


    public interface IServices
    {
         Task<Response> Login<Request,Response>(Request req)
            where Request : IMessage, new()  where Response : IMessage, new();
    }


    public abstract class Services
    {
        public virtual async Task<bool> Handle(bool request) { throw new NotImplementedException(); }
    }
   
}
