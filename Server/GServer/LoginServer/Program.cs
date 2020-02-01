using System;
using System.IO;
using System.Text;
using System.Threading;
using org.vxwo.csharp.json;
using ServerUtility;
using XNet.Libs.Utility;

namespace LoginServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            NetProtoTool.EnableLog = true;
            Debuger.Loger = new DefaultLoger();
            string json;
            if (args.Length > 0)
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
                json = File.ReadAllText(file, new UTF8Encoding(false));
               
            }
            else
            {
                json = "{" +
                    "\"ListenPort\":1900," +
                    "\"ServicePort\":1800," +
                    @"""DBHost"":""mongodb+srv://dbuser:54249636@cluster0-us8pa.gcp.mongodb.net/test?retryWrites=true&w=majority""," +
                    "\"DBName\":\"game\"," +
                    "\"Log\":true" +
                    "}";
            }


            Debuger.Log(json);
            var config = JsonReader.Read(json);
            var app = new Appliaction(config);
            var thread = new Thread(()=>
            {
                app.Start();
                while (app.IsRunning)
                {
                    Thread.Sleep(100);
                    app.Tick();
                }
            })
            {
                IsBackground = false
            };
            thread.Start();

            var MEvent = new ManualResetEvent(false);
            MEvent.Reset();
            var u = new UnixExitSignal();
            u.Exit += (s, e) =>
            {
                MEvent.Set();
                Debuger.Log("App will exit");
                app.Stop();// = false;
            };

            MEvent.WaitOne();

            if (thread.IsAlive)
            {
                thread.Join();
            }
            Debuger.Log("Appliaction had exited!");
        }


    }
}
