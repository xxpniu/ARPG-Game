using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer
{
    [HandleType(typeof(G2L_Reg))]
    public class GServerReg : Responser<G2L_Reg, L2G_Reg>
    {
        public GServerReg()
        {
            NeedAccess = false;
        }

        public override L2G_Reg DoResponse(G2L_Reg request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new L2G_Reg { Code = ErrorCode.VersionError };
            }

            if (Appliaction.Current.Servers.HaveKey(request.ServerID))
            {
                return new L2G_Reg { Code = ErrorCode.Error };
            }
            else {

                client.HaveAdmission = true;
                Appliaction.Current.Servers.Add(
                    request.ServerID,
                    new GameServerInfo
                    {
                        ServerID = request.ServerID,
                        Host = request.Host,
                        Port = request.Port
                    }
                );
                return new L2G_Reg { Code = ErrorCode.OK };
            }
        }
    }
}

