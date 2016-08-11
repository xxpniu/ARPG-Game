using System;
using System.Collections.Generic;
using System.Threading;
using XNet.Libs.Net;
using Proto;

namespace UnitTester
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var list = new List<RequestClient>();
            int i = 0;
            while (true)
            {
                i++;

                if (i == 100)
                {
                    
                    i = 0;
                    Thread.Sleep(5000);
                    foreach (var c in list)
                    {
                        c.Disconnect();
                    }
                }
                Thread.Sleep(30);
                var client = new RequestClient( "127.0.0.1",1900);
                client.UseSendThreadUpdate = true;
                client.OnConnectCompleted = (s, e) => {
                    if (e.Success)
                    {
                        var request = client.CreateRequest<C2S_Login,S2C_Login>();
                        request.RequestMessage.Password = "54249636";
                        request.RequestMessage.UserName = "xxp";
                        request.SendRequest();
                    }
                };
                client.Connect();
                list.Add(client);
            }
            //return false;
        }
    }
}
