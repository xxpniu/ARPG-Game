using System;
using Proto;
using XNet.Libs.Net;
using System.Linq;
using ServerUtility;
using LoginServer.Managers;

namespace LoginServer.Responsers
{
    [HandleType(typeof(C2S_Reg))]
    public class RegResponser:Responser<C2S_Reg,S2C_Reg>
    {
        public RegResponser()
        {
            NeedAccess = false;
        }

        public override S2C_Reg DoResponse(C2S_Reg request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new S2C_Reg { Code = ErrorCode.VersionError };
            }

            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new S2C_Reg { Code = ErrorCode.RegInputEmptyOrNull };
            }


            using (var db = new DataBaseContext.GameAccountDb(Appliaction.Current.Connection))
            {
                var query = db.TbaCCount.Where(t => t.UserName == request.UserName).Count();
                if (query > 0)
                {
                    return new S2C_Reg
                    {
                        Code = ErrorCode.RegExistUserName
                    };
                }
                else {
                    var acc = new DataBaseContext.TbaCCount 
                    {
                        UserName = request.UserName,
                        Password = request.Password,
                        CreateDateTime = DateTime.UtcNow,
                        LoginCount =0,
                        LastLoginDateTime = DateTime.UtcNow,
                        ServerID = 1
                    };

                    db.TbaCCount.InsertOnSubmit(acc);
                    db.SubmitChanges();
                    //var userID = Convert.ToInt64( db.InsertWithIdentity(acc));
                    var session = DateTime.UtcNow.Ticks.ToString();
                    Appliaction.Current.SetSession(acc.ID, session);
                    var mapping = ServerManager.Singleton.GetGateServerMappingByServerID(acc.ServerID);
                    if (mapping == null)
                    {
                        return new S2C_Reg { Code = ErrorCode.NOFoundServerID };
                    }
                    return new S2C_Reg
                    {
                        Code = ErrorCode.OK,
                        Session = session,
                        UserID = acc.ID,
                        Server = mapping.ServerInfo
                    };
                }
            }
          
        }
    }
}

