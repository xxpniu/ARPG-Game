using System;
using Layout.LayoutEffects;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using System.Reflection;

namespace GameLogic.Game.LayoutLogics
{

	public class EffectHandleAttribute : Attribute
	{
		public EffectHandleAttribute(Type handleType)
		{
			HandleType = handleType;
		}

		public Type HandleType { set; get; }
	}


	public class EffectBaseLogic
	{
		static EffectBaseLogic ()
		{
			_handlers = new Dictionary<Type, MethodInfo>();
			var methodInfos = typeof(EffectBaseLogic).GetMethods();
			foreach (var i in methodInfos)
			{
				var attrs = i.GetCustomAttributes(typeof(EffectHandleAttribute), false) as EffectHandleAttribute[];
				if (attrs.Length == 0) continue;
				_handlers.Add(attrs[0].HandleType, i);
			}
		}

		private static Dictionary<Type, MethodInfo> _handlers; 

		/// <summary>
		/// Effects the active.
		/// </summary>
		/// <param name="effectTarget">成熟效果的目标</param>
		/// <param name="effect">效果类型</param>
		/// <param name="releaser">魔法释放者</param>
		public static void EffectActive(BattleCharacter effectTarget, EffectBase effect,MagicReleaser releaser)
		{
			MethodInfo handle;
			if (_handlers.TryGetValue(effect.GetType(), out handle))
			{
				handle.Invoke(null, new object[] { effectTarget, effect, releaser });
			}
			else{
				throw new Exception(string.Format("Effect [{0}] no handler!!!",effect.GetType()));
			}
		}


		[EffectHandleAttribute(typeof(NormalDamageEffect))]
		public static void NormalDamage(BattleCharacter effectTarget, EffectBase e, MagicReleaser releaser)
		{
			var effect = e as NormalDamageEffect;
			int damage = -1;
			switch (effect.valueOf)
			{
				case ValueOf.FixedValue:
					damage = effect.DamageValue;
					break;
				case ValueOf.NormalAttack:
					damage = BattleAlgorithm.CalNormalDamage(releaser.ReleaserTarget.Releaser, effectTarget);
					break;
			}

			if (damage > 0)
			{
				effectTarget.SubHP(damage);
			}
  		}
	}
}

