using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tips
{
    [UITipResourcesAttribute("UUIHpNumber")]
    public class UUIHpNumber :UUITip
    {

        private Text text;

        protected override void OnCreate()
        {
            text = FindChild<Text>("Text");
            //iTween(text.gameObject,2.5f,0, new Vector3(30,200,0));
        }

        public void SetHp(int hp)
        {
            text.text = string.Format("{0}", Mathf.Abs( hp));
            text.color = hp > 0 ? Color.green : Color.red;
        }
    }
}