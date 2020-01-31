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
            else
            {
                json = "{" +
                    "\"ListenPort\":1900," +
                    "\"ServicePort\":1800," +
                    @"""DBHost"":""mongodb+srv://{1}:{0}@cluster0-us8pa.gcp.mongodb.net/test?retryWrites=true&w=majority""," +
                    "\"DBName\":\"game\"," +
                    "\"DBUser\":\"dbuser\"," +
                    "\"DBPwd\":\"54249636\"," +
                    "\"Log\":true" +
                    "}";
            }
            var config = JsonReader.Read(json);
            app = new Appliaction(config);
            app.Start();
            var thread = new Thread(Runer)
            {
                IsBackground = false
            };
            thread.Start();
            while (app.IsRunning)
            {
                Thread.Sleep(100);
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
