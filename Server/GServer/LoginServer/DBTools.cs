using System;
using System.Linq;

namespace LoginServer
{
    public class DBTools
    {
        public class PwdResult
        {
            public string Pwd { set; get; }
        }

        public DBTools()
        {
        }
        public static string GetPwd(string password, DataBaseContext.GameAccountDb db)
        {
            var result = db.ExecuteQuery<PwdResult>
                           ("SELECT PASSWORD({0}) as Pwd", password)
                                 .FirstOrDefault();

            var pwd = result.Pwd;
            return pwd;
        }

    }
}
