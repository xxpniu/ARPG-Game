using System;
using System.IO;
using System.Text;
using System.Threading;
using org.vxwo.csharp.json;
using Proto;
using ServerUtility;
using XNet.Libs.Utility;

namespace GServer
{
    class Program
    {
        public static void Main(string[] args)
        {

            Debuger.Loger = new DefaultLoger();
            string json = string.Empty;
            if (args.Length > 0)
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
                json = File.ReadAllText(file, new UTF8Encoding(false));

            }
            else
            {
                json = "{" +
                    "\"ListenPort\":1700," +
                    "\"Host\":\"127.0.0.1\"," +
                    "\"ServicePort\":2000," +
                    "\"ServiceHost\":\"127.0.0.1\"" +
                    "\"LoginServerHost\":\"127.0.0.1\"," +
                    "\"LoginServerPort\":\"1800\"," +
                    "\"DBHost\":\"mongodb://127.0.0.1:27017/\"," +
                    "\"DBName\":\"game\"," +
                    "\"ServerID\":\"1\"," +
                    "\"ConfigPath\":\"/Users/xiexiongping/Documents/github/version/Server/\"" +
                    "\"Log\":true" +
                    "\"EnableGM\":true" +
                    "}";
            }


            Debuger.Log(json);

            var MEvent = new ManualResetEvent(false);
            var config = JsonReader.Read(json);
            var app = new Appliaction(config);

            MEvent.Reset();
            var u = new UnixExitSignal();
            u.Exit += (s, e) =>
            {
                MEvent.Set();
                Debuger.Log("App will exit");
                app.Stop();// = false;
            };
            var runner = new Thread(() =>
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
            runner.Start();

            MEvent.WaitOne();

            if (runner.IsAlive) { runner.Join(); }
            Debuger.Log("Appliaction had exited!");


        }

      
    }
}
