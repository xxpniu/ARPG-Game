using System;
using Layout.EditorAttributes;

namespace Layout.LayoutEffects
{
    public enum BreakReleaserType
    {
        ALL = 0,
        InStartLayoutMagic,
        Buff
    }

    [EditorEffect("打断施法")]
    public class BreakReleaserEffect :EffectBase
    {
        public BreakReleaserEffect()
        {
            breakType = BreakReleaserType.InStartLayoutMagic;
        }
        [Label("打断类型")]
        public BreakReleaserType breakType;
    }
}

