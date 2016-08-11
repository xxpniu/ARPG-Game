using System;
using Layout.EditorAttributes;

namespace Layout.LayoutEffects
{
    public enum AddType
    {
        Base,
        Append,
        Rate
    }

    public enum RevertType
    {
        None,
        ReleaserDeath
    }

    [EditorEffect("修改属性")]
    public class AddPropertyEffect : EffectBase
    {
        public AddPropertyEffect()
        {
            addType = AddType.Append;
            addValue = 0;
            revertType = RevertType.ReleaserDeath;
        }

        [Label("修改类型")]
        public AddType addType;

        [Label("修改值")]
        public int addValue;

        [Label("恢复方式")]
        public RevertType revertType;

        [Label("属性类型")]
        public Proto.HeroPropertyType property;
    }
}

