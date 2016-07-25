using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using UnityEngine;

namespace Windows
{
    partial class UUIBattle
    {
        public class GridTableModel : TableItemModel<GridTableTemplate>
        {
            public GridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }

        protected override void InitModel()
        {
            base.InitModel();

            var drag = this.uiRoot.AddComponent<DragRecognizer>();
            drag.OnGesture += (t) =>
                {
                    switch(t.State)
                    {
                        case GestureRecognitionState.Started:
                            break;
                        case GestureRecognitionState.InProgress:
                            {
                                var offset = -t.DeltaMove.y /Screen.height;
                                var res= ThridPersionCameraContollor.Singleton.forward.y +offset;
                                ThridPersionCameraContollor.Singleton.forward.y = Mathf.Clamp(res,-1, -0.05f);
                            }
                            break;
                        case GestureRecognitionState.Ended:
                            break;
                    }
                };
           var pinch = this.uiRoot.AddComponent<PinchRecognizer>();
            pinch.OnGesture += (t) =>
                {
                    switch(t.State)
                    {
                        case GestureRecognitionState.InProgress:
                            {
                                var offset = t.Delta /(Screen.height/2);
                                var res= ThridPersionCameraContollor.Singleton.Distance  + offset;
                                ThridPersionCameraContollor.Singleton.Distance = Mathf.Clamp(res,10,25);
                            }
                            break;
                    }
                };
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
            var datas = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.CharacterData> (t => t.ID <= 4);
            this.GridTableManager.Count = datas.Length;
            int index = 0;
            foreach (var i in GridTableManager) {
                var b = i.Root.transform.GetComponent<Button> ();
                var text = i.Root.transform.FindChild<Text> ("Text");
                var Cost = i.Root.transform.FindChild<Text> ("Cost");
                text.text = datas [index].Name;
                Cost.text = datas [index].Cost.ToString ();
                var data = datas [index];
                //only once
                b.onClick.RemoveAllListeners ();
                b.onClick.AddListener (() => {
                    OnClick(data);
                });
                index++;
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            var gate = UAppliaction.Singleton.GetGate() as UGameGate;
            if (gate == null)
                return;
            //targetPoint = Mathf.Lerp(targetPoint, gate.pointLeft,UnityEngine.Time.deltaTime *5 );
            Point.text = string.Format ("{0:0}/{1}",(int)gate.pointLeft,gate.LevelData.MaxPoint);
            var time = System.TimeSpan.FromSeconds (gate.LeftTime);
            Time.text = string.Format ("剩余时间 {0:00}:{1:00}", time.Minutes, time.Seconds);
        }

        //private float targetPoint;

        private void OnClick (ExcelConfig.CharacterData data)
        {
            //ExcelConfig.CharacterData data =null;
            var gate = UAppliaction.Singleton.GetGate() as UGameGate;
            if (gate == null)
                return;
            gate.CreateCharacter (data);

        }

        protected override void OnHide()
        {
            base.OnHide();
        }
    }
}