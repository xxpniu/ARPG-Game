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
    partial class UUISaleItem
    {

        protected override void InitModel()
        {
            base.InitModel();
            s_salenum.onValueChanged.AddListener((v) =>
                {
                    saleNum = (int)v ;
                    ShowSale();
                });
            bt_close.onClick.AddListener(() =>
                {
                    this.HideWindow();
                });
            
            bt_OK.onClick.AddListener(() =>
                {
                    if(saleNum==0) return;

                    var gate = UAppliaction.S.G<GMainGate>();
                    var request = gate.Client.CreateRequest<Proto.C2G_SaleItem,Proto.G2C_SaleItem>();
                    var saleItem = new SaleItem{ Guid = Item.GUID, Num =saleNum};
                    request.RequestMessage.Items.Add(saleItem);
                    request.OnCompleted =(s,r)=>{
                        
                        if(r.Code == ErrorCode.OK)
                        {
                            HideWindow();
                            gate.Coin = r.Coin;
                            gate.Gold =r.Gold;
                            gate.UpdateItem(r.Diff);
                            //update
                            UAppliaction.S.ShowError(r.Code);
                            //UUIManager.S.UpdateUIData();
                        }
                        else{
                            UAppliaction.S.ShowError(r.Code);
                        }
                    };
                    request.SendRequest();
                });
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
            config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(Item.ItemID);
            t_name.text = config.Name;
            s_salenum.minValue = 0;
            s_salenum.maxValue = Item.Num;
            s_salenum.value = saleNum = Item.Num;
            ShowSale();
        }


        private void ShowSale()
        {
            t_num.text = saleNum.ToString();
            t_pricetotal.text = (saleNum * config.SalePrice).ToString();
        }

        private ItemData config;

        protected override void OnHide()
        {
            base.OnHide();
        }

        private PlayerItem Item;
        private int saleNum = 1;

        public void Show(PlayerItem item)
        {
            Item = item;
            ShowWindow();
        }
    }
}