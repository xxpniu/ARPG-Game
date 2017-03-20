using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using Proto;

namespace Windows
{
    partial class UUILogin
    {

        protected override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_submit.onClick.AddListener(() =>
                {
                    var userName = if_userName.text;
                    var pwd = if_pwd.text;
                    var gate = UAppliaction.Singleton.GetGate() as LoginGate;
                    if(gate ==null) return;
                    var request = gate.Client.CreateRequest<C2L_Login,L2C_Login>();
                    request.RequestMessage.Password = pwd;
                    request.RequestMessage.UserName = userName;
                    request.RequestMessage.Version = Proto.ProtoTool.GetVersion();
                    request.OnCompleted = (success, response) =>{
                        if (response.Code == ErrorCode.OK)
                        {
                            UAppliaction.Singleton.GoServerMainGate(response.Server, response.UserID, response.Session);
                        }
                        else
                        {
                            UAppliaction.Singleton.ShowError(response.Code);   
                        }
                    };
                    request.SendRequest();
                });
            bt_reg.onClick.AddListener(() =>
                {
                    var userName = if_userName.text;
                    var pwd = if_pwd.text;
                    var gate = UAppliaction.Singleton.GetGate() as LoginGate;
                    var request = gate.Client.CreateRequest<C2L_Reg,L2C_Reg>();
                    request.RequestMessage.Password = pwd;
                    request.RequestMessage.UserName = userName;
                    request.RequestMessage.Version = ProtoTool.GetVersion();
                    request.OnCompleted =(s,r)=>{
                        if (r.Code == ErrorCode.OK)
                        {
                            UAppliaction.Singleton.GoServerMainGate(r.Server, r.UserID, r.Session);
                        }
                        else
                        {
                            UUITipDrawer.Singleton.ShowNotify("Server Response:" + r.Code);
                        }
                    };
                    request.SendRequest();
                });
        }

        protected override void OnShow()
        {
            base.OnShow();

            //bt_submit.SetText();
        }

        protected override void OnHide()
        {
            base.OnHide();

        }
    }
}