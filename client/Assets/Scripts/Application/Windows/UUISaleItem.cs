using Proto;
using ExcelConfig;
using static Proto.C2G_SaleItem.Types;
using EConfig;
using Proto.GateServerService;

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

                    var saleItem = new C2G_SaleItem.Types.SaleItem { Guid = Item.GUID, Num = saleNum };
                    var re = new C2G_SaleItem { };
                    re.Items.Add(saleItem);
                    var gate = UApplication.G<GMainGate>();
                    Proto.GateServerService.SaleItem.CreateQuery()
                    .SendRequest(gate.Client, re,
                    r =>
                    {
                        UApplication.S.ShowError(r.Code);
                        if (r.Code.IsOk())
                        {
                            HideWindow();
                            gate.Coin = r.Coin;
                            gate.Gold = r.Gold;
                            gate.UpdateItem(r.Diff);
                            //update

                        }
                    });
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