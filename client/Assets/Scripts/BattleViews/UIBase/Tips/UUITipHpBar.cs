using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tips
{
    [UITipResources("UUITipHpBar")]
    public class UUITipHpBar : UUITip
    {
        protected override void OnCreate()
        {

            text = FindChild<Text>("Text");
            slider = FindChild<Scrollbar>("Scrollbar");
        }

        private Text text;
        private Scrollbar slider;
        private float targetHp =-1f;
        private float tcur;
        private int max;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            tcur = Mathf.Lerp(tcur, targetHp, Time.deltaTime * 5);
            slider.size = (float)tcur / (float)max;
        }

        public void SetHp(int cur,int max)
        {
            if (targetHp < 0)
            {
                this.tcur = targetHp = cur;
            }
            this.max = max;
            targetHp = cur;
            text.text = string.Format("{0}/{1}", cur, max);
        }
    }

}