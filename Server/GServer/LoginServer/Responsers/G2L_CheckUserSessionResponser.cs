using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer
{
    [HandleType(typeof(G2L_CheckUserSession))]
    public class G2L_CheckUserSessionResponser:Responser<G2L_CheckUserSession,L2G_CheckUserSession>
    {
        public G2L_CheckUserSessionResponser()
        {
            NeedAccess = true;
        }

        public override L2G_CheckUserSession DoResponse(G2L_CheckUserSession request, Client client)
        {
            
            if (Appliaction.Current.GetSession(request.UserID) == request.Session)
            {
                return new L2G_CheckUserSession { Code = ErrorCode.OK };
            }
            else {
                return new L2G_CheckUserSession { Code = ErrorCode.Error };
            }
        }
    }
}

