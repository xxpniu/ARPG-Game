using System;
using XNet.Libs.Utility;

namespace ServerUtility
{
    /// <summary>
    /// Console 日志记录、默认
    /// </summary>
    public class DefaultLoger : Loger
    {
        public override void WriteLog(DebugerLog log)
        {
            Console.WriteLine(log);
        }
    }
}

