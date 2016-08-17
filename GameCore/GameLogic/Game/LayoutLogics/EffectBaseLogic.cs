using System;
using Layout.LayoutEffects;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using System.Reflection;
using GameLogic.Game.Perceptions;

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
        static EffectBaseLogic()
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
        public static void EffectActive(BattleCharacter effectTarget, EffectBase effect, MagicReleaser releaser)
        {
            MethodInfo handle;
            if (_handlers.TryGetValue(effect.GetType(), out handle))
            {
                handle.Invoke(null, new object[] { effectTarget, effect, releaser });
            }
            else {
                throw new Exception(string.Format("Effect [{0}] no handler!!!", effect.GetType()));
            }
        }

        [EffectHandleAttribute(typeof(NormalDamageEffect))]
        public static void NormalDamage(BattleCharacter effectTarget, EffectBase e, MagicReleaser releaser)
        {
            var per = releaser.Controllor.Perception as BattlePerception;
            var effect = e as NormalDamageEffect;
            int damage = -1;
            switch (effect.valueOf)
            {
                case ValueOf.FixedValue:
                    damage = effect.DamageValue;
                    break;
                case ValueOf.NormalAttack:
                    damage = BattleAlgorithm.CalFinalDamage(
                        BattleAlgorithm.CalNormalDamage(releaser.ReleaserTarget.Releaser),
                        releaser.ReleaserTarget.Releaser.TDamage,
                        effectTarget.TDefance);
                    break;
            }

            var result = BattleAlgorithm
               .GetDamageResult(damage, releaser.ReleaserTarget.Releaser.TDamage, effectTarget);

            if (releaser.ReleaserTarget.Releaser.TDamage != Proto.DamageType.Magic)
            {
                if (!result.IsMissed)
                {
                    var cureHP = (int)(result.Damage * releaser.ReleaserTarget.Releaser[Proto.HeroPropertyType.SuckingRate].FinalValue / 10000f);
                    if (cureHP > 0)
                        per.CharacterAddHP(releaser.ReleaserTarget.Releaser, cureHP);
                }
            }
          
            per.ProcessDamage(releaser.ReleaserTarget.Releaser,effectTarget, result);
        }

        //CureEffect
        [EffectHandleAttribute(typeof(CureEffect))]
        public static void Cure(BattleCharacter effectTarget, EffectBase e, MagicReleaser releaser)
        {
            var per = releaser.Controllor.Perception as BattlePerception;
            var effect = e as CureEffect;
            int cure = -1;
            switch (effect.valueType)
            {
                case ValueOf.FixedValue:
                    cure = effect.value;
                    break;
                case ValueOf.NormalAttack:
                    cure = BattleAlgorithm.CalNormalDamage(
                        releaser.ReleaserTarget.Releaser);
                    break;
            }

            if (cure > 0)
            {
                per.CharacterAddHP(effectTarget, cure);
            }
        }

        [EffectHandle(typeof(AddBufEffect))]
        public static void AddBuff(BattleCharacter effectTarget, EffectBase e, MagicReleaser releaser)
        {
            var effect = e as AddBufEffect;
            var per = releaser.Controllor.Perception as BattlePerception;
            per.CreateReleaser(
                effect.buffMagicKey,
                new ReleaseAtTarget(releaser.ReleaserTarget.Releaser, effectTarget),
                ReleaserType.Buff
            );
        }

        [EffectHandle(typeof(BreakReleaserEffect))]
        public static void BreakAction(BattleCharacter effectTarget, EffectBase e, MagicReleaser releaser)
        {
            var effect = e as BreakReleaserEffect;
            var per = releaser.Controllor.Perception as BattlePerception;
            per.BreakReleaserByCharacter(effectTarget, effect.breakType);
        }

        [EffectHandle(typeof(AddPropertyEffect))]
        public static void AddProperty(BattleCharacter effectTarget, EffectBase e, MagicReleaser releaser)
        {
            var effect = e as AddPropertyEffect;
            var per = releaser.Controllor.Perception as BattlePerception;

            per.ModifyProperty(effectTarget, effect.property, effect.addType, effect.addValue);
            if (effect.revertType == RevertType.ReleaserDeath)
            {
                releaser.RevertProperty(effectTarget, effect.property, effect.addType, effect.addValue);
            }
        }
    }
}

