using System;
using System.Collections.Generic;
using System.Threading;
using XNet.Libs.Net;
using Proto;
using XNet.Libs.Utility;

namespace UnitTester
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Debuger.Loger = new ServerUtility.DefaultLoger();
            ServerUtility.NetProtoTool.EnableLog = true;
            var host = "127.0.0.1";
            var port = 1900;
            int i = 0;
            isRunning = true;
            var list = new List<MulitPlayerUnitTest>();
            while (i++ < 1000)
            {
                var m = new MulitPlayerUnitTest("UTester" + i, "123456", host, port);
                list.Add(m);
                m.Begin();

                Thread.Sleep(300);
            }

            while (isRunning)
            {
                Thread.Sleep(100);
            }


        }

        private static bool isRunning = false;
    }
}
