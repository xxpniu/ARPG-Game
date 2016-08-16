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
                Debuger.Log(json);
            }
            else {
                json = "{" +
                    "\"ListenPort\":1900," +
                    "\"ServicePort\":1800," +
                    "\"DBHost\":\"127.0.0.1\"," +
                    "\"DBName\":\"game_account_db\","+
                    "\"DBUser\":\"root\"," +
                    "\"DBPwd\":\"54249636\"," +
                    "\"Log\":true"+
                    "}";
            }
            var config = JsonReader.Read(json);
            app = new Appliaction(config);
            app.Start();
            var thread = new Thread(Runer);
            thread.IsBackground = false;
            thread.Start();
            var u = new UnixExitSignal();
            u.Exit += (s, e) => {
                Debuger.Log("App will exit");
                app.Stop();// = false;
            };
            while (app.IsRunning)
            {
                Thread.Sleep(100);
#if MONO

#endif
            }
           
            //thread.Join();
        }
    

        private static Appliaction app;
       // private static bool isRunning = false;
        private static void Runer()
        {
            //app.Start();
            while (app.IsRunning)
            {
                Thread.Sleep(100);
                app.Tick();
            }
           
        }
    }
}
