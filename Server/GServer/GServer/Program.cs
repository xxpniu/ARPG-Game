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
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
            var json = File.ReadAllText(file,new UTF8Encoding(false));
            Debuger.Log(json);
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
            if (runner.IsAlive)
                runner.Join();
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
