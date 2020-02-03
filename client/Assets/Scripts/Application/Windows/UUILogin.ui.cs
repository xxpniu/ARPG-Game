using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UGameTools;
using UnityEngine.UI;
//AUTO GenCode Don't edit it.
namespace Windows
{
    [UIResources("UUILogin")]
    partial class UUILogin : UUIAutoGenWindow
    {


        protected Text t_userName;
        protected Text t_pwd;
        protected InputField if_userName;
        protected InputField if_pwd;
        protected Button bt_submit;
        protected Button bt_reg;




        protected override void InitTemplate()
        {
            base.InitTemplate();
            t_userName = FindChild<Text>("t_userName");
            t_pwd = FindChild<Text>("t_pwd");
            if_userName = FindChild<InputField>("if_userName");
            if_pwd = FindChild<InputField>("if_pwd");
            bt_submit = FindChild<Button>("bt_submit");
            bt_reg = FindChild<Button>("bt_reg");


        }
    }
}