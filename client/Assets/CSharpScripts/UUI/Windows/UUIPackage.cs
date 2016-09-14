using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;

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
            }
        }

        protected override void InitModel()
        {
            base.InitModel();
            bt_close.onClick.AddListener(() =>
                {
                    HideWindow();
                });
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
            ContentTableManager.Count = 50;
        }
        protected override void OnHide()
        {
            base.OnHide();
        }
    }
}