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
    partial class UUIDetail
    {

        protected override void InitModel()
        {
            base.InitModel();
            bt_cancel.onClick.AddListener(() =>
                {
                    HideWindow();
                });
            bt_sale.onClick.AddListener(() =>
                {
                    this.HideWindow();
                    var ui = UUIManager.S.CreateWindow<UUISaleItem>();
                    ui.Show(this.item);
                    //show sale ui
                });
            //Write Code here
        }



        protected override void OnShow()
        {
            base.OnShow();

            var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.ItemID);
            t_num.text = item.Num > 1 ? item.Num.ToString() : string.Empty;
            t_descript.text = config.Description;
            t_name.text = config.Name;
            t_prices.text = "售价 " + config.SalePrice;
            Icon.texture =ResourcesManager.S.LoadResources<Texture2D>("Icon/" + config.Icon);
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        private PlayerItem item;

        public void Show(PlayerItem item)
        {
            this.item = item;
            this.ShowWindow();
        }
    }
}