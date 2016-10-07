using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;

namespace Windows
{
    partial class UIEquipment
    {
        public class EquipListTableModel : TableItemModel<EquipListTableTemplate>
        {
            public EquipListTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }
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
    }
}