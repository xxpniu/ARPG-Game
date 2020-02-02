using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;

namespace Windows
{
    partial class UUIMain
    {

        protected override void InitModel()
        {
            base.InitModel();
            bt_fight.onClick.AddListener(() =>
                {
                    var ui = UUIManager.Singleton.CreateWindow<Windows.UUILevelList>();
                    ui.ShowWindow();
                    //UAppliaction.Singleton.GoToGameBattleGate(1);
                });

            bt_package.onClick.AddListener(() =>
                {
                    var ui = UUIManager.Singleton.CreateWindow<Windows.UUIPackage>();
                    ui.ShowWindow();
                });
            //Write Code here
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
            var gate = UApplication.S.G<GMainGate>();

            lb_coin.text = gate.Coin.ToString("N0");
            lb_gold.text = gate.Gold.ToString("N0");
        }
    }
}