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
    [UIResources("UUILevelList")]
    partial class UUILevelList : UUIAutoGenWindow
    {
        public class ContentTableTemplate : TableItemTemplate
        {
            public ContentTableTemplate(){}
            public Button Button;

            public override void InitTemplate()
            {
                Button = FindChild<Button>("Button");
            }
        }


        protected ScrollRect ScrollView;
        protected GridLayoutGroup Content;
        protected Button Bt_Return;


        protected UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>> ContentTableManager = new UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>>();


        protected override void InitTemplate()
        {
            base.InitTemplate();
            ScrollView = FindChild<ScrollRect>("ScrollView");
            Content = FindChild<GridLayoutGroup>("Content");
            Bt_Return = FindChild<Button>("Bt_Return");

            ContentTableManager.InitFromGrid(Content);

        }
    }
}