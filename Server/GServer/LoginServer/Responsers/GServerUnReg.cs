using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer
{
    [HandleType(typeof(G2L_UnReg))]
    public class GServerUnReg:Responser<Proto.G2L_UnReg, Proto.L2G_UnReg>
    {
        public GServerUnReg()
        {
            NeedAccess = false;
        }

        public override L2G_UnReg DoResponse(G2L_UnReg request, Client client)
        {
            Appliaction.Current.Servers.Remove(request.ServerID);
            return new L2G_UnReg { Code = ErrorCode.OK };
        }
    }
}

