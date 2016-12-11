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

        /// <summary>
        /// 获取MD5的密码
        /// </summary>
        /// <returns>The pwd.</returns>
        /// <param name="password">Password.</param>
        /// <param name="db">Db.</param>
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
