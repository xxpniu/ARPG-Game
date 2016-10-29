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
    [UIResources("UIEquipment")]
    partial class UIEquipment : UUIAutoGenWindow
    {
        public class EquipListTableTemplate : TableItemTemplate
        {
            public EquipListTableTemplate(){}
            public Button bt_euqipType;

            public override void InitTemplate()
            {
                bt_euqipType = FindChild<Button>("bt_euqipType");

            }
        }
        public class ContentTableTemplate : TableItemTemplate
        {
            public ContentTableTemplate(){}
            public Image EquipItem;
            public Text lb_equipName;
            public Text lb_equipDes;

            public override void InitTemplate()
            {
                EquipItem = FindChild<Image>("EquipItem");
                lb_equipName = FindChild<Text>("lb_equipName");
                lb_equipDes = FindChild<Text>("lb_equipDes");

            }
        }


        protected GridLayoutGroup EquipList;
        protected VerticalLayoutGroup Content;


        protected UITableManager<AutoGenTableItem<EquipListTableTemplate, EquipListTableModel>> EquipListTableManager = new UITableManager<AutoGenTableItem<EquipListTableTemplate, EquipListTableModel>>();
        protected UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>> ContentTableManager = new UITableManager<AutoGenTableItem<ContentTableTemplate, ContentTableModel>>();


        protected override void InitTemplate()
        {
            base.InitTemplate();
            EquipList = FindChild<GridLayoutGroup>("EquipList");
            Content = FindChild<VerticalLayoutGroup>("Content");

            EquipListTableManager.InitFromGrid(EquipList);
            ContentTableManager.InitFromTable(Content);

        }
    }
}