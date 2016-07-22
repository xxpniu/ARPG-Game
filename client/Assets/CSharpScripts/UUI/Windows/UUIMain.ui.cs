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
    [UIResources("UUIMain")]
    partial class UUIMain : UUIAutoGenWindow
    {


        protected Text lb_gold;
        protected Text lb_coin;
        protected Button bt_package;
        protected Button bt_fight;




        protected override void InitTemplate()
        {
            base.InitTemplate();
            lb_gold = FindChild<Text>("lb_gold");
            lb_coin = FindChild<Text>("lb_coin");
            bt_package = FindChild<Button>("bt_package");
            bt_fight = FindChild<Button>("bt_fight");


        }
    }
}