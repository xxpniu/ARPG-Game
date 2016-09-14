using System;
using Layout.EditorAttributes;
using Proto;

namespace Layout.LayoutEffects
{

    [EditorEffect("行为锁")]
    public class ModifyLockEffect:EffectBase
    {
        public ModifyLockEffect()
        {
            lockType = ActionLockType.NOATTACK;
            revertType = RevertType.ReleaserDeath;
        }

        [Label("类型")]
        public ActionLockType lockType;

        [Label("回滚方式")]
        public RevertType revertType;
    }
}

