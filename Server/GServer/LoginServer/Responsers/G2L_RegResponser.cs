using System;
using LoginServer.Managers;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace LoginServer
{
    [HandleType(typeof(G2L_Reg), HandleResponserType.SERVER_SERVER)]
    public class G2L_RegResponser : Responser<G2L_Reg, L2G_Reg>
    {
        public G2L_RegResponser()
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
                Port = request.Port,
                MaxServiceCount = request.MaxPlayer
            };
            var success = ServerManager
                .S.AddGateServer(
                    client.ID,
                    request.CurrentPlayer,
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

