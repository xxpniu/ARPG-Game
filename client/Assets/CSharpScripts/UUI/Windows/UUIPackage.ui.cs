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
            public RawImage RawImage;
            public Text Text;

            public override void InitTemplate()
            {
                Button = FindChild<Button>("Button");
                RawImage = FindChild<RawImage>("RawImage");
                Text = FindChild<Text>("Text");

            }
        }


        protected GridLayoutGroup Content;
        protected Button bt_close;
        protected Text t_size;


        protected UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>> ContentTableManager = new UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>>();


        protected override void InitTemplate()
        {
            base.InitTemplate();
            Content = FindChild<GridLayoutGroup>("Content");
            bt_close = FindChild<Button>("bt_close");
            t_size = FindChild<Text>("t_size");

            ContentTableManager.InitFromGrid(Content);

        }
    }
}