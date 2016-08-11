using System;
using System.Threading;
using ServerUtility;
using XNet.Libs.Utility;

namespace LoginServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            int port = 1900;
            int servicePort = 1800;
            string datasources = "127.0.0.1";
            string db = "Game_Account_DB";
            string username = "root";
            string pwd = "54249636";
            if (args.Length > 4)
            {
                port = int.Parse(args[0]);
                servicePort= int.Parse(args[1]);
                datasources = args[2];
                db = args[3];
                username = args[4];
                pwd = args[5];
            }
            app = new Appliaction(port,servicePort,datasources,db,username,pwd);
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
