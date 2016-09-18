using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using Proto;
using ExcelConfig;
using UnityEngine;

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
                Template.RawImage.texture =ResourcesManager.S.LoadResources<Texture2D>("Icon/" + itemconfig.Icon);
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
            t_size.text = string.Format("{0}/{1}", gate.package.Items.Count, gate.package.MaxSize);
        }

        private void ClickItem(ContentTableModel item)
        {
            var ui = UUIManager.S.CreateWindow<UUIDetail>();
            ui.Show(item.pItem);
            //Show Item UI
        }
    }
}