using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using Proto;
using ExcelConfig;

namespace Windows
{
    partial class UUIPackage
    {
        public class ContentTableModel : TableItemModel<ContentTableTemplate>
        {
            public ContentTableModel(){}
            public override void InitModel()
            {
                //todo
                Template.Button.onClick.AddListener(
                    () =>
                    {
                        if (OnClickItem == null)
                            return;
                        OnClickItem(this);
                    });
            }
            public Action<ContentTableModel> OnClickItem;
            public ItemData Config;
            public PlayerItem pItem;
            public void SetItem(PlayerItem item)
            {
                var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.ItemID);
                Config = itemconfig;
                pItem = item;
                Template.Text.text = item.Num>1? item.Num.ToString():string.Empty;
            }
        }

        protected override void InitModel()
        {
            base.InitModel();
            bt_close.onClick.AddListener(
                () =>
                {
                    HideWindow();
                });
        }
        protected override void OnShow()
        {
            base.OnShow();
            lb_itemName.text = lb_itemdescript.text = string.Empty;
            OnUpdateUIData();

        }
        protected override void OnHide()
        {
            base.OnHide();
        }

        protected override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            var gate = UAppliaction.S.G<GMainGate>();
            ContentTableManager.Count = gate.package.Items.Count;
            int index = 0;
            foreach (var i in ContentTableManager)
            {
                i.Model.SetItem(gate.package.Items[index]);
                i.Model.OnClickItem = ClickItem;
                index++;
            }
             
            if (ContentTableManager.Count > 0)
            {
                ClickItem(ContentTableManager[0].Model);
            }
        }

        private void ClickItem(ContentTableModel item)
        {
            lb_itemName.text = item.Config.Name + 
                (item.pItem.Num>1?string.Format(" * {0}",item.pItem.Num):string.Empty);
            lb_itemdescript.text = item.Config.Description;
            //Show Item UI
        }
    }
}