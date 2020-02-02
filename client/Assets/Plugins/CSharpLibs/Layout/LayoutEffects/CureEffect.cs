using System;
using Layout.EditorAttributes;

namespace Layout.LayoutEffects
{
    [EditorEffect("恢复生命")]
    public class CureEffect:EffectBase
    {
        public CureEffect()
        {
            valueType = ValueOf.NormalAttack;
        }

        [Label("取值来源")]
        public ValueOf valueType;

        [Label("值")]
        public int value;
    }
}

