using System;
using EngineCore.Simulater;
using EngineCore;
using Layout.LayoutEffects;

namespace GameLogic.Game.Elements
{
	//远程攻击对盾牌防御
	public enum AttackType
	{
		Normal=0,
		Remote
	}

	public enum DefanceType
	{
		Normal=0,
		Shield
	}
		

	public enum BodyType
	{
		Human,//人类
		Skeleton//骷髅
	}



	public class BattleCharacter:BattleElement<IBattleCharacter>
	{
		public BattleCharacter (GControllor controllor, IBattleCharacter view):base(controllor,view)
		{
			HP = 0;
			HPMax = 0;//will new an intansce
			DamageMax = 0;
			DamageMin = 0;
			Attack = 0;
			Defence = 0;
		}
			

		public ComplexValue HPMax{ private set; get;}
		public ComplexValue DamageMin{ private set; get;}
		public ComplexValue DamageMax{ private set; get;}
		public ComplexValue Attack{ private set; get;}
		public ComplexValue Defence{ private set; get;}

		public BodyType TBody{ set; get;}
		public AttackType TAttack{ set; get;}
		public DefanceType TDefance{ set; get;}
		public DamageType TDamage{ set; get;}

		public int TeamIndex{ set; get;}
		public int Level{ set; get;}

		public int HP{ private set; get;} 

		public bool IsDeath{
			get
			{ 
				return HP == 0;
			}
		}

		public bool SubHP(int hp)
		{
			if (hp <= 0)
				return false;
			if (HP == 0)
				return true;
			HP -= hp;
			if (HP <= 0)
				HP = 0;
			return HP == 0;//is dead
		}

		public void AddHP(int hp)
		{
			if (hp <= 0)
				return;
			if (HP >= HPMax.FinalValue)
				return;
			HP += hp;
			if (HP >= HPMax.FinalValue)
				HP = HPMax.FinalValue;
		}


	}
}

