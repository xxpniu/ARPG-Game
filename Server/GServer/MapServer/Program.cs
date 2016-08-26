using System;
using System.IO;
using System.Text;
using System.Threading;
using org.vxwo.csharp.json;
using ServerUtility;
using XNet.Libs.Utility;

namespace MapServer
{
    class Program
    {

        public static void Main(string[] args)
        {
            Debuger.Loger = new DefaultLoger();
            var json = string.Empty;
            if (args.Length > 0)
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
                json = File.ReadAllText(file, new UTF8Encoding(false));
                Debuger.Log(json);
            }
            else {
                json = "{" +
                    "\"Port\":2001," + //端口
                    "\"LoginServerProt\":\"1800\"," +
                    "\"LoginServerHost\":\"127.0.0.1\"," +
                    "\"ServiceHost\":\"127.0.0.1\"," +
                    "\"ConfigRoot\":\"../../../../\"" +
                    "\"MaxBattle\":10000"+
                    "\"Log\":true" +
                    "}";
            }
            MEvent.Reset();
            var config = JsonReader.Read(json);
            app = new Appliaction(config);
            app.Start();
            var thread = new Thread(Runer);
            thread.IsBackground = true;
            thread.Start();

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

        private static ManualResetEvent MEvent = new ManualResetEvent(false);
        private static Appliaction app;
        private static void Runer()
        {
            //app.Start();
            while (app.IsRunning)
            {
                app.Tick();
                Thread.Sleep(100);
            }

            MEvent.Set();
        }
    }

}
