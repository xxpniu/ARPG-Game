using System;
using Layout.EditorAttributes;

namespace Layout.LayoutEffects
{
	public enum ValueOf
	{
		NormalAttack = 0,
		FixedValue
	}

	[EditorEffect("攻击伤害")]
	public class NormalDamageEffect:EffectBase
	{
		public NormalDamageEffect ()
		{
			
		}
		[Label("取值来源")]
		public ValueOf valueOf;

		[Label("固定伤害值")]
		public int DamageValue;

		public override string ToString()
		{
			if (valueOf == ValueOf.FixedValue)
			{
				return string.Format("对目标造成固定伤害{0}", DamageValue);
			}
			else {
				return "对目标造普通攻击伤害";
			}
		}
	}
}

