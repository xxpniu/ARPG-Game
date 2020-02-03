using System;
using UnityEngine.UI;

namespace Tips
{
    [UITipResourcesAttribute("UUINotify")]
    public class UUINotify:UUITip
    {
        public UUINotify()
        {
        }

        private Text  t_text;

        protected override void OnCreate()
        {
            t_text = FindChild<Text>("Text");
        }

        public void SetNotify(string text)
        {
            t_text.text = text;
        }
    }
}

