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
    [UIResources("UUIPackage")]
    partial class UUIPackage : UUIAutoGenWindow
    {
        public class ContentTableTemplate : TableItemTemplate
        {
            public ContentTableTemplate(){}
            public Button Button;
            public RectTransform RawImage;
            public Text Text;

            public override void InitTemplate()
            {
                Button = FindChild<Button>("Button");
                RawImage = FindChild<RectTransform>("RawImage");
                Text = FindChild<Text>("Text");

            }
        }


        protected GridLayoutGroup Content;
        protected Text lb_itemName;
        protected Text lb_itemdescript;
        protected Button bt_close;


        protected UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>> ContentTableManager = new UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>>();


        protected override void InitTemplate()
        {
            base.InitTemplate();
            Content = FindChild<GridLayoutGroup>("Content");
            lb_itemName = FindChild<Text>("lb_itemName");
            lb_itemdescript = FindChild<Text>("lb_itemdescript");
            bt_close = FindChild<Button>("bt_close");

            ContentTableManager.InitFromGrid(Content);

        }
    }
}