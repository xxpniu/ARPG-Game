using System;
using GameLogic.Game.Elements;
using Proto;

namespace GameLogic.Game
{
    
    public struct DamageResult
    {
        public DamageResult(DamageType t, bool isMissed, int da)
        {
            DType = t;
            IsMissed = isMissed;
            Damage = da;
        }
        public DamageType DType;
        public bool IsMissed;
        public int Damage;
    }
	/// <summary>
	/// 战斗中的算法
	/// 
	/// </summary>
	public sealed class BattleAlgorithm
	{
		public BattleAlgorithm ()
		{
            //HeroPropertyType.Agility
		}

        public static float FORCE_HP = 10;
        public static float KNOWLEGDE_MP = 10;
        public static float AGILITY_DEFANCE = 1;
        public static float AGILITY_SUBWAITTIME = 2;//每点敏捷降低攻击间隔时间毫秒
        public static float AGILITY_ADDSPEED = 0.02f;//0.02米
        public static float ATTACK_MIN_WAIT = 100;//攻击最低间隔200毫秒
        public static float FORCE_CURE_HP =0.1f; //每点力量没秒增加血量
        public static float KNOWLEDGE_CURE_MP = 0.1f;//每点智力增加魔法
        public static float MAX_SPEED = 5.4f;//最大速度
        public static float DEFANCE_RATE = 0.006f;//伤害减免参数
		/// <summary>
		/// 取中间数
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static float Clamp(float value, float min, float max)
		{
			if (value > max)
				return max;
			if (value < min)
				return min;
			return value;
		}


        public static int CalNormalDamage(BattleCharacter attack)
        {
            float damage = Randomer.RandomMinAndMax(
                attack[HeroPropertyType.DamageMin].FinalValue,
                attack[HeroPropertyType.DamageMax].FinalValue);
            
            switch (attack.Category)
            {
                case HeroCategory.Agility:
                    damage += attack[HeroPropertyType.Agility].FinalValue;
                    break;
                case HeroCategory.Force:
                    damage += attack[HeroPropertyType.Force].FinalValue;
                    break;
                case HeroCategory.Knowledge:
                    damage += attack[HeroPropertyType.Knowledge].FinalValue;
                    break;
            }
            return (int)damage;
        }

		public static float[][] DamageRate = new float[][]
		{
			new float[]{0f,0f,0f},//混乱
			new float[]{0f,0.5f,0f},
			new float[]{.5f,-0.5f,0f}
		};

        //处理伤害类型加成
        public static int CalFinalDamage(int damage, DamageType dType, DefanceType dfType)
        {
            float rate = 1 + DamageRate[(int)dType][(int)dfType];
            float result = damage * rate;
            return (int)result;
        }

        public static DamageResult GetDamageResult(int damage,DamageType dType, BattleCharacter defencer)
        {
            bool isMissed = false;
            switch (dType)
            {
                case DamageType.Physical:
                    {
                        var d = defencer[HeroPropertyType.Defance].FinalValue + defencer[HeroPropertyType.Agility].FinalValue*AGILITY_DEFANCE;
                        //处理防御((装甲)*0.006))/(1+0.006*(装甲) 
                        damage = damage - (int)(damage *
                            (d * DEFANCE_RATE) /(1 + DEFANCE_RATE * d));
                        
                        isMissed = GRandomer.Probability10000(defencer[HeroPropertyType.Jouk].FinalValue);
                    }
                    break;
                case DamageType.Magic:
                    {
                        damage =(int)( damage *(1 - defencer[HeroPropertyType.Resistibility].FinalValue / 10000f));
                    }
                    break;
                default:
                    break;
            }

            return new DamageResult
            {
                Damage = damage,
                DType = dType,
                IsMissed = isMissed 
            };
        }
	}
}

