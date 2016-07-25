using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UGameTools;
using UnityEngine.UI;
//AUTO GenCode Don't edit it.
namespace Windows
{
    [UIResources("UUIBattle")]
    partial class UUIBattle : UUIAutoGenWindow
    {
        public class GridTableTemplate : TableItemTemplate
        {
            public GridTableTemplate(){}
            public Text Text;
            public Text Cost;

            public override void InitTemplate()
            {
                Text = FindChild<Text>("Text");
                Cost = FindChild<Text>("Cost");

            }
        }


        protected Text Point;
        protected GridLayoutGroup Grid;
        protected Text Time;


        protected UITableManager<AutoGenTableItem<GridTableTemplate, GridTableModel>> GridTableManager = new UITableManager<AutoGenTableItem<GridTableTemplate, GridTableModel>>();


        protected override void InitTemplate()
        {
            base.InitTemplate();
            Point = FindChild<Text>("Point");
            Grid = FindChild<GridLayoutGroup>("Grid");
            Time = FindChild<Text>("Time");

            GridTableManager.InitFromGrid(Grid);

        }
    }
}