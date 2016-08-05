using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.Utility
{
    /// <summary>
    /// author:xxp
    /// </summary>
    public class Debuger
    {
        public static void Log(object msg)
        {
            DoLog(LogerType.Log, msg.ToString());
        }

        public static void LogWaring(object msg)
        {
            DoLog(LogerType.Waring, msg.ToString());
        }

        public static void LogError(object msg)
        {
            DoLog(LogerType.Error, msg.ToString());
        }

        public static void DebugLog(string msg)
        { 
#if DEBUG
            DoLog(LogerType.Debug, msg);
#endif
        }

        private static void DoLog(LogerType type, string msg)
        {
            var log = new DebugerLog()
            {
                LogTime = DateTime.Now,
                Message = msg,
                Type = type
            };
            Loger.WriteLog(log);
        }

        static Debuger()
        {
            Loger = new DefaultLoger();
        }

        public static Loger Loger { set; get; }
    }
    /// <summary>
    /// 日志记录者
    /// </summary>
    public abstract class Loger
    {
        public abstract void WriteLog(DebugerLog log);
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogerType
    {
        Log,
        Waring,
        Error,
        Debug
    }
    /// <summary>
    /// 日志
    /// </summary>
    public class DebugerLog
    {
        public LogerType Type { set; get; }
        public string Message { set; get; }
        public DateTime LogTime { set; get; }
        public override string ToString()
        {
            return string.Format(
                 "[{0}]({2:hh:mm:ss MM/dd}):{1}", Type, Message, LogTime
                );
        }
    }
    /// <summary>
    /// Console 日志记录、默认
    /// </summary>
    public class DefaultLoger : Loger
    {
        public override void WriteLog(DebugerLog log)
        {
            switch(log.Type)
            {
                case  LogerType.Debug:
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
