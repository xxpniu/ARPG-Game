using System;
using System.Linq;
using LoginServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer.Responsers
{
    [HandleType(typeof(C2L_Login))]
    public class C2L_LoginResponser : Responser<C2L_Login,L2C_Login>
    {
        public C2L_LoginResponser()
        {
            NeedAccess = false;
        }

        public override L2C_Login DoResponse(C2L_Login request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new L2C_Login { Code = ErrorCode.VersionError };
            }
            using (var db = new DataBaseContext.GameAccountDb(Appliaction.Current.Connection))
            {

                var query = db.TbaCCount
                              .Where(t => t.UserName == request.UserName && t.Password == request.Password)
                              .SingleOrDefault();
                if (query == null)
                {
                    return new L2C_Login { Code = ErrorCode.LoginFailure };
                }
                else
                {
                    var session = DateTime.UtcNow.Ticks.ToString();
                    Appliaction.Current.SetSession(query.ID, session);
                    query.LastLoginDateTime = DateTime.UtcNow;
                    query.LoginCount += 1;
                    db.SubmitChanges();

                    var mapp = ServerManager.Singleton.GetGateServerMappingByServerID(query.ServerID);
                    if (mapp == null)
                    {
                        return new L2C_Login { Code = ErrorCode.NOFoundServerID };

                    }
                    UserServerInfo info;
                    if (BattleManager.Singleton.GetBattleServerByUserID(query.ID, out info))
                    {
                        var task = new Task_L2B_ExitUser { UserID = query.ID };
                        var server = ServerManager.Singleton.GetBattleServerMappingByServerID(info.BattleServerID);
                        if (server != null)
                        {
                            var connection = Appliaction.Current.GetServerConnectByClientID(server.ClientID);
                            if (connection != null)
                            {
                                var message = NetProtoTool.ToNetMessage(MessageClass.Task, task);
                                connection.SendMessage(message);
                            }
                        }
                    }
                    return new L2C_Login
                    {
                        Code = ErrorCode.OK,
                        Server =  mapp.ServerInfo,
                        Session = session,
                        UserID = query.ID
                    };
                }
                
            }
        }

    }
}

