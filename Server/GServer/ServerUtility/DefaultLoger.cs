using System;
using XNet.Libs.Utility;

namespace ServerUtility
{/// <summary>
 /// Console 日志记录、默认
 /// </summary>
    public class DefaultLoger : Loger
    {
        public override void WriteLog(DebugerLog log)
        {

            switch (log.Type)
            {
                case LogerType.Debug:
                case LogerType.Waring:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogerType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.WriteLine(log.ToString());
        }
    }
}

