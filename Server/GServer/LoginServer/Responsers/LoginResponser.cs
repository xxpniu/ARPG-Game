using System;
using System.Linq;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer.Responsers
{
    [HandleType(typeof(Proto.C2S_Login))]
    public class LoginResponser : Responser<C2S_Login,S2C_Login>
    {
        public LoginResponser()
        {
            NeedAccess = false;
        }

        public override S2C_Login DoResponse(C2S_Login request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new S2C_Login { Code = ErrorCode.VersionError };
            }
            using (var db = new DataBaseContext.GameAccountDb(Appliaction.Current.Connection))
            {

                var query = db.TbaCCount
                              .Where(t => t.UserName == request.UserName && t.Password == request.Password)
                              .SingleOrDefault();
                if (query == null)
                {
                    return new S2C_Login { Code = ErrorCode.LoginFailure };
                }
                else
                {
                    var session = DateTime.UtcNow.Ticks.ToString();
                    Appliaction.Current.SetSession(query.ID, session);
                    query.LastLoginDateTime = DateTime.UtcNow;
                    query.LoginCount += 1;
                    db.SubmitChanges();

                    return new S2C_Login
                    {
                        Code = ErrorCode.OK,
                        Server = Appliaction.Current.Servers[query.ServerID],
                        Session = session,
                        UserID = query.ID
                    };
                }
                
            }
        }

    }
}

