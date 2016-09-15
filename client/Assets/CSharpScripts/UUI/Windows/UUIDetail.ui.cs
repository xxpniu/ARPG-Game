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
    [UIResources("UUIDetail")]
    partial class UUIDetail : UUIAutoGenWindow
    {


        protected RawImage Icon;
        protected Text t_name;
        protected Text t_num;
        protected Text t_descript;
        protected Text t_prices;
        protected Button bt_cancel;
        protected Button bt_sale;




        protected override void InitTemplate()
        {
            base.InitTemplate();
            Icon = FindChild<RawImage>("Icon");
            t_name = FindChild<Text>("t_name");
            t_num = FindChild<Text>("t_num");
            t_descript = FindChild<Text>("t_descript");
            t_prices = FindChild<Text>("t_prices");
            bt_cancel = FindChild<Button>("bt_cancel");
            bt_sale = FindChild<Button>("bt_sale");


        }
    }
}