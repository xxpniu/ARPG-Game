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
            else {
                json = "{" +
                    "\"ListenPort\":1700," +
                    "\"Host\":\"127.0.0.1\"," +
                    "\"ServicePort\":2000," +
                    "\"ServiceHost\":\"127.0.0.1\""+
                    "\"LoginServerHost\":\"127.0.0.1\"," +
                    "\"LoginServerPort\":\"1800\"," +
                    "\"DBHost\":\"mongodb+srv://dbuser:54249636@cluster0-us8pa.gcp.mongodb.net/test?retryWrites=true&w=majority\"," +
                    "\"DBName\":\"game\","+
                    "\"ServerID\":\"1\"," +
                    "\"ConfigPath\":\"../../../../../\"" +
                    "\"Log\":true" +
                    "\"EnableGM\":true"+
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
                u = null;
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
                u?.CurrentWait.Wait(10);
                MEvent.Set();
            })
            {
                IsBackground = false
            };
            runner.Start();

            MEvent.WaitOne();

            if (runner.IsAlive)
            {
                runner.Join(1000);
            }
            Debuger.Log("Appliaction had exited!");

        }

      
    }
}
