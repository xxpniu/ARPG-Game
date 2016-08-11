using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;

namespace Windows
{
    partial class UUIBattleResult
    {

        protected override void InitModel()
        {
            base.InitModel();
            Bt_Ok.onClick.AddListener(() =>
                {
                    //UAppliaction.Singleton.GoToMainGate();
                });

            Bt_Again.onClick.AddListener(() =>
                {
                    var gate = UAppliaction.Singleton.GetGate() as UGameGate;
                    if(gate ==null) return;

                    UAppliaction.Singleton.GoToGameBattleGate(gate.LevelData.ID);
                });
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
        }
        protected override void OnHide()
        {
            base.OnHide();
        }

        public void ShowResult(bool isWin)
        {
            
        }
    }
}