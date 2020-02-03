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
                    @"""DBHost"":""mongodb://127.0.0.1:27017/""," +
                    "\"DBName\":\"game\"," +
                    "\"Log\":true" +
                    "}";
            }


            Debuger.Log(json);
            var config = JsonReader.Read(json);
            var app = new Appliaction(config);
            var MEvent = new ManualResetEvent(false);
            var thread = new Thread(()=>
            {
                app.Start();
                while (app.IsRunning)
                {
                    Thread.Sleep(100);
                    app.Tick();
                }
                MEvent.Set();
            })
            {
                IsBackground = false
            };
            thread.Start();

            
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
