using System;
using System.Threading;
using ServerUtility;
using XNet.Libs.Utility;

namespace GServer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var port = 1700;
            var serivcePort = 1701;
            var loginHost = "127.0.0.1";
            var loginPort = 1800;

            var dbHost = "127.0.0.1";
            var dbName = "Game_DB";
            var dbUser = "root";
            var dbPwd = "54249636";
            var ServerID = 1;
            var key = "key001";
            var root = "../../../../";

            if (args.Length == 11)
            {
                port = int.Parse(args[0]);
                serivcePort = int.Parse(args[1]);
                loginHost = args[2];
                loginPort = int.Parse(args[3]);
                dbHost = args[4];
                dbName = args[5];
                dbUser = args[6];
                dbPwd = args[7];
                ServerID = int.Parse(args[8]);
                root = args[9];
                key = args[10];

            }

            app = new Appliaction(port, serivcePort, loginHost, loginPort, dbHost, dbName, dbUser, dbPwd, ServerID, key, root);

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
