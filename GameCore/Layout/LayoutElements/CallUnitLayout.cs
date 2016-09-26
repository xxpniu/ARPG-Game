using System;
using Layout.EditorAttributes;
using Proto;

namespace Layout.LayoutElements
{
    [EditorLayoutAttribute("召唤单位")]
    public class CallUnitLayout:LayoutBase
    {
        public CallUnitLayout()
        {
            valueFrom = GetValueFrom.CurrentConfig;
            level = 1;
        }

        [Label("召唤角色ID")]
        public int characterID;

        [Label("等级取之来源")]
        public GetValueFrom valueFrom;

        [Label("等级")]
        public int level;

        [Label("持续时间(秒)")]
        public float time;

        [Label("召唤物最大数量")]
        public int maxNum;

        public override string ToString()
        {
            return string.Format("time:{0} ID:{1}", time, characterID);
        }
    }
}
