using System;
using Proto;
using XNet.Libs.Net;
using System.Linq;
using ServerUtility;
using LoginServer.Managers;

namespace LoginServer.Responsers
{
    [HandleType(typeof(C2L_Reg),HandleResponserType.CLIENT_SERVER)]
    public class C2L_RegResponser:Responser<C2L_Reg,L2C_Reg>
    {
        public C2L_RegResponser()
        {
            NeedAccess = false;
        }

        public override L2C_Reg DoResponse(C2L_Reg request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new L2C_Reg { Code = ErrorCode.VersionError };
            }

            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new L2C_Reg { Code = ErrorCode.RegInputEmptyOrNull };
            }




            using (var db = Appliaction.Current.GetDBContext())
            {
                var query = db.TbaCCount.Where(t => t.UserName == request.UserName).Count();
                if (query > 0)
                {
                    return new L2C_Reg
                    {
                        Code = ErrorCode.RegExistUserName
                    };
                }
                else 
                {

                    var free= ServerManager.S.GetFreeGateServer();
                    if (free == null)
                    {
                        return new L2C_Reg() { Code = ErrorCode.NoFreeGateServer };
                    }

                    var serverID = free.ServerInfo.ServerID;

                    var pwd = DBTools.GetPwd(request.Password,db);
                    var acc = new DataBaseContext.TbaCCount 
                    {
                        UserName = request.UserName,
                        Password = pwd,
                        CreateDateTime = DateTime.UtcNow,
                        LoginCount =0,
                        LastLoginDateTime = DateTime.UtcNow,
                        ServerID = serverID
                    };

                    db.TbaCCount.InsertOnSubmit(acc);
                    db.SubmitChanges();
                    //var userID = Convert.ToInt64( db.InsertWithIdentity(acc));
                    var session = DateTime.UtcNow.Ticks.ToString();
                    Appliaction.Current.SetSession(acc.ID, session);
                    var mapping = ServerManager.Singleton.GetGateServerMappingByServerID(acc.ServerID);
                    if (mapping == null)
                    {
                        return new L2C_Reg { Code = ErrorCode.NOFoundServerID };
                    }
                    return new L2C_Reg
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

