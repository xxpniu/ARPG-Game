using System;
using Proto;
using XNet.Libs.Net;
using System.Linq;
using org.vxwo.csharp.json;
using System.Collections.Generic;
using ServerUtility;
using GServer.Managers;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_Login),HandleResponserType.CLIENT_SERVER)]
    public class C2G_LoginResponser:Responser<Proto.C2G_Login,Proto.G2C_Login>
    {
        public C2G_LoginResponser()
        {
            NeedAccess = false;
        }

        public override G2C_Login DoResponse(C2G_Login request, Client client)
        {
            if (!ProtoTool.CompareVersion(request.Version))
            {
                return new G2C_Login { Code = ErrorCode.VersionError };
            }


            if (string.IsNullOrWhiteSpace(request.Session))
                return new G2C_Login { Code = ErrorCode.Error };



            var req = Appliaction.Current.Client.CreateRequest<G2L_CheckUserSession, L2G_CheckUserSession>();
            req.RequestMessage.Session = request.Session;
            req.RequestMessage.UserID = request.UserID;
            ErrorCode resultCode = ErrorCode.Error;
            req.OnCompleted = (s,res) =>
            {
                if (s&&res.Code == ErrorCode.OK)
                {
                    //kick other
                    var clients = Appliaction.Current.ListenServer.CurrentConnectionManager.AllConnections;
                    foreach (var i in clients)
                    {
                        if (i.UserState!=null && (long)i.UserState == request.UserID)
                        {
                            i.Close();
                        }
                    }

                    resultCode = ErrorCode.OK;
                    client.HaveAdmission = true;
                    client.UserState = request.UserID;
                }
            };
            req.SendRequest();

            if (client.HaveAdmission)
            {
                Managers.UserData data;
                var manager = MonitorPool.S.Get<UserDataManager>();
                if (!manager.TryToGetUserData(request.UserID, out data))
                {
                    return new G2C_Login { Code = ErrorCode.NoGamePlayerData };
                }

                return new G2C_Login
                {
                    Code = ErrorCode.OK,
                    Package = data.GetPackage(),
                    Hero = data.GetHero(),
                    Coin = data.Coin,
                    Gold  =data.Gold

                };
            }
            else 
            {
                return new G2C_Login { Code = resultCode };
            }
        }
    }
}

