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
    [UIResources("UUISaleItem")]
    partial class UUISaleItem : UUIAutoGenWindow
    {


        protected Button bt_close;
        protected Text t_name;
        protected Text t_pricetotal;
        protected Text t_num;
        protected Slider s_salenum;
        protected Button bt_OK;




        protected override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<Button>("bt_close");
            t_name = FindChild<Text>("t_name");
            t_pricetotal = FindChild<Text>("t_pricetotal");
            t_num = FindChild<Text>("t_num");
            s_salenum = FindChild<Slider>("s_salenum");
            bt_OK = FindChild<Button>("bt_OK");


        }
    }
}