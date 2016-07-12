using System;
using GameLogic.Game.Elements;

namespace GameLogic.Game
{
	/// <summary>
	/// 战斗中的算法
	/// 
	/// </summary>
	public sealed class BattleAlgorithm
	{
		public BattleAlgorithm ()
		{
			
		}

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

		public static int CalNormalDamage(BattleCharacter attack, BattleCharacter defencer)
		{
			//lever = clamp(1+((L_a - D_f)/10) ,0,2)
			//damage = clamp((1+(AT_a-DF_f)/100),0.3f,2f)* Rand(Dmin_a,Dmax_a);
		
			float lvl = Clamp(((float)attack.Level - (float)defencer.Level)/10f,-1f,2f);
			float rate = Clamp ((1f + ((float)attack.Attack.FinalValue - (float)defencer.Defence.FinalValue) / 10f)+lvl, 0.3f, 2f);

			float damage = rate * Randomer.RandomMinAndMax (attack.DamageMin.FinalValue, attack.DamageMax.FinalValue);
			return (int)damage;
		}

		public static float[][] AttackRate = new float[][]{
			new float[]{0,0},
			new float[]{0.5f,-0.5f}
		};

		public static float[][] DamageRate = new float[][]
		{
			new float[]{0f,0f},//混乱
			new float[]{0f,0.5f},
			new float[]{.5f,-0.5f}
		};

		//处理伤害类型加成
		public static int CalFinalDamage(int damage,BattleCharacter attack, BattleCharacter defencer)
		{
			float rate = 1+( AttackRate [(int)attack.TAttack] [(int)defencer.TDefance] +
				DamageRate [(int)attack.TDamage] [(int)defencer.TBody]);
			float result = damage * rate;
			return (int)result;
		}
	}
}

