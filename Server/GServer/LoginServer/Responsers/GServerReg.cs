using System;
using LoginServer.Managers;
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


            client.HaveAdmission = true;
            client.UserState = request.ServerID;
            var server = new GameServerInfo
            {
                ServerID = request.ServerID,
                Host = request.Host,
                Port = request.Port
            };
            var success = ServerManager
                .Singleton
                .AddGateServer(
                    client.ID,
                    server,
                   request.ServiceHost,
                   request.ServicesProt
                );

            if (!success)
            {
                return new L2G_Reg { Code = ErrorCode.Error };
            }

            client.OnDisconnect += OnDisconnect;
            return new L2G_Reg { Code = ErrorCode.OK };

        }

        public static void OnDisconnect(object sender, EventArgs e)
        {
            var client = sender as Client;
            ServerManager.Singleton.RemoveGateServer((int)client.UserState);
        }
    }
}

