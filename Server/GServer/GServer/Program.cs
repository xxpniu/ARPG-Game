using System;
using System.IO;
using System.Text;
using System.Threading;
using org.vxwo.csharp.json;
using ServerUtility;
using XNet.Libs.Utility;

namespace GServer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Debuger.Loger = new DefaultLoger();
            string json = string.Empty;
            if (args.Length > 0)
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
                json = File.ReadAllText(file, new UTF8Encoding(false));
                Debuger.Log(json);
            }
            else {
                json = "{" +
                    "\"ListenPort\":1700," +
                    "\"Host\":\"127.0.0.1\"," +
                    "\"ServicePort\":2000," +
                    "\"ServiceHost\":\"127.0.0.1\""+
                    "\"LoginServerHost\":\"127.0.0.1\"," +
                    "\"LoginServerPort\":\"1800\"," +
                    "\"DBHost\":\"127.0.0.1\"," +
                    "\"DBName\":\"game_db\"," +
                    "\"DBUser\":\"root\"," +
                    "\"DBPwd\":\"54249636\"," +
                    "\"ServerID\":\"1\"," +
                    "\"ConfigPath\":\"../../../../\"" +
                    "\"Log\":true" +
                    "\"EnableGM\":true"+
                    "}";
            }
            var config = JsonReader.Read(json);
            app = new Appliaction(config);
            app.Start();

            var runner = new Thread(Runner);
            runner.IsBackground = false;
            runner.Start();

            var u = new UnixExitSignal();
            u.Exit += (s, e) =>
            {
                Debuger.Log("App will Exit!");
                app.Stop();
            };

            while (app.IsRunning)
            {
                Thread.Sleep(100);
            }

        }

        private static Appliaction app;
       
        private static void Runner()
        {

            while (app.IsRunning)
            {
                Thread.Sleep(100);
                app.Tick();
            }
        }
    }
}
