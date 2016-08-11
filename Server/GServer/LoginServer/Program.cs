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
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
            var json = File.ReadAllText(file,new UTF8Encoding(false));
            Debuger.Log(json);
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
           
            thread.Join();
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
