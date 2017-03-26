using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using UnityEngine;
using Proto;
using ExcelConfig;
using GameLogic.Game.Perceptions;

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
                this.Template.Button.onClick.AddListener(
                    () =>
                    {
                        if((lastTime +0.3f >UnityEngine.Time.time))return;
                        lastTime = UnityEngine.Time.time;
                        if (OnClick == null)
                            return;
                        OnClick(this);
                    });
            }

            public Action<GridTableModel> OnClick;

            public void SetMagic(int id, float cdTime)
            {
                if (magicID != id)
                {
                    magicID = id;
                    MagicData = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterMagicData>(id);
                    var per = UPerceptionView.S as IBattlePerception;
                    var magic = per.GetMagicByKey(MagicData.MagicKey);

                    if (magic != null)
                        this.Template.Button.SetText( magic.name);
                }
            }

            private int magicID = -1;
            public ExcelConfig.CharacterMagicData MagicData;
            private float cdTime = 0.01f;

            private float lastTime = 0;

            public void Update(UCharacterView view, float now)
            {
                HeroMagicData data;
                if (view.MagicCds.TryGetValue(magicID, out data))
                {
                    var time = Mathf.Max(0, data.CDTime - now);
                    this.Template.Cost.text = time > 0 ? string.Format("{0:0.0}", time) : string.Empty;
                    if (cdTime < time)
                        cdTime = time;
                    if (time > 0)
                    {
                        lastTime = UnityEngine.Time.time;
                    }
                    if (cdTime > 0)
                    {
                        this.Template.ICdMask.fillAmount = time / cdTime;
                    }
                    else
                    {
                        this.Template.ICdMask.fillAmount = 0;
                    }
                }
            }
        }

        protected override void InitModel()
        {
            base.InitModel();
            bt_Auto.onClick.AddListener(() =>
                {
                    //IsAuto = !IsAuto;
                    SetAuto(!IsAuto);
                });

            bt_Exit.onClick.AddListener(() =>
                {
                    var gate = UAppliaction.Singleton.GetGate() as BattleGate;
                    if (gate == null)
                        return;
                    var request =gate.Client.CreateRequest<C2B_ExitBattle,B2C_ExitBattle>();
                    request.RequestMessage .UserID = UAppliaction.Singleton.UserID;
                    request.OnCompleted=(s,r)=>{
                        if(r.Code == ErrorCode.OK)
                        {
                            UAppliaction.Singleton.GoBackToMainGate();
                        }
                    };
                    request.SendRequest();
                });
        }

        private void SetAuto(bool auto)
        {
            var action = new Proto.Action_AutoFindTarget{ Auto = auto};
            var gate = UAppliaction.Singleton.GetGate() as BattleGate;
            if (gate == null)
                return;
            IsAuto = auto;
            gate.SendAction(action);
            bt_Auto.SetText(IsAuto?"暂停":"战斗");
        }

        private bool IsAuto = true;

        protected override void OnShow()
        {
            base.OnShow();
           
            this.GridTableManager.Count = 0;
            bt_Auto.SetText(IsAuto?"暂停":"战斗");
            //SetAuto(true);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            var gate = UAppliaction.Singleton.GetGate() as BattleGate;
            if (gate == null)
                return;
            var timeSpan = TimeSpan.FromSeconds(gate.TimeServerNow);
            this.Time.text = string.Format("{0:00}:{1:00}", (int)timeSpan.TotalMinutes, timeSpan.Seconds);
            foreach (var i in GridTableManager)
            {
                i.Model.Update(view,gate.TimeServerNow);
            }
        }

        //private float targetPoint;

        private void OnClick (ExcelConfig.CharacterData data)
        {
            //ExcelConfig.CharacterData data =null;
           
        }
        protected override void OnHide()
        {
            base.OnHide();
        }

        public void InitCharacter(UCharacterView view)
        {
            var magic = view.MagicCds.Where(t=>IsMaigic(t.Key)).ToList();
            this.GridTableManager.Count = magic.Count;
            int index = 0;
            foreach (var i in GridTableManager)
            {
                i.Model.SetMagic(magic[index].Key, magic[index].Value.CDTime);
                i.Model.OnClick = OnRelease;
                index++;
            }
            this.view = view;
        }

        private void OnRelease(GridTableModel item)
        {
            var action = new Proto.Action_ClickSkillIndex{  MagicKey = item.MagicData.MagicKey };
            var gate = UAppliaction.Singleton.GetGate() as BattleGate;
            if (gate == null)
                return;
            gate.SendAction(action);
        }

        private UCharacterView view;

        public bool IsMaigic(int id)
        {
            var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterMagicData>(id);
            if (data == null)
                return false;
            return data.ReleaseType == (int)Proto.MagicReleaseType.Magic;
        }



    }
}