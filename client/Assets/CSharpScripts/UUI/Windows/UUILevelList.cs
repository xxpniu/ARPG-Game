using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using Proto;
using EConfig;
using Proto.GateServerService;

namespace Windows
{
    partial class UUILevelList
    {
        public class ContentTableModel : TableItemModel<ContentTableTemplate>
        {
            public ContentTableModel(){}
            public override void InitModel()
            {
                this.Template.Button =this.Item.Root.GetComponent<Button>();
                //todo
                this.Template.Button.onClick.AddListener(()=>{
                    if(Onclick ==null) return;
                    Onclick(this);
                });
            }

            public Action<ContentTableModel> Onclick;
            public BattleLevelData Data{ set; get; }

            public void SetLevel(BattleLevelData level)
            {
                Data = level;
                this.Template.Button.SetText(level.Name);
            }
        }

        protected override void InitModel()
        {
            base.InitModel();
            Bt_Return.onClick.AddListener(() =>
                {
                    this.HideWindow();
                });
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
            var levels = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<BattleLevelData>();
            ContentTableManager.Count = levels.Length;
            int index = 0;
            foreach (var i in ContentTableManager)
            {
                i.Model.SetLevel(levels[index]);
                i.Model.Onclick = OnItemClick;
                index++;
            }



            ScrollView.SetLayoutVertical();
        }

        private void OnItemClick(ContentTableModel item)
        {
            var gate = UApplication.Singleton.GetGate() as GMainGate;
            if (gate == null) return;

            BeginGame.CreateQuery().SetCallBack(r =>
            {
                if (r.Code.IsOk())
                {
                    UApplication.Singleton.GotoBattleGate(r.ServerInfo, item.Data.ID);
                }
                else
                {
                    UApplication.Singleton.ShowError(r.Code);
                }
            }).SendRequest(gate.Client,new C2G_BeginGame { MapID =1 });
        }
           

        protected override void OnHide()
        {
            base.OnHide();
        }
    }
}