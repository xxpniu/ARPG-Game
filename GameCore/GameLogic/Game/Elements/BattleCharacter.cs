using EngineCore.Simulater;
using Layout.LayoutEffects;
using GameLogic.Game.AIBehaviorTree;
using System;
using System.Collections.Generic;

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

	public class ReleaseHistory
	{
		public int MagicDataID;
		public float LastTime;
		public float CdTime;

		public bool IsCoolDown(float time)
		{
			return time > LastTime + CdTime;
		}

		public float TimeToCd(float time)
		{
			return Math.Max(0, (LastTime + CdTime) - time); 
		}
	}

	public class BattleCharacter:BattleElement<IBattleCharacter>
	{
		public BattleCharacter (int configID,GControllor controllor, IBattleCharacter view):base(controllor,view)
		{
			HP = 0;
			HPMax = 0;//will new an intansce
			DamageMax = 0;
			DamageMin = 0;
			Attack = 0;
			Defence = 0;
			ConfigID = configID;
		}

		public int ConfigID { private set; get; }
		private Dictionary<int, ReleaseHistory> _history = new Dictionary<int, ReleaseHistory>();

		public ComplexValue HPMax{ private set; get;}
		public ComplexValue DamageMin{ private set; get;}
		public ComplexValue DamageMax{ private set; get;}
		public ComplexValue Attack{ private set; get;}
		public ComplexValue Defence{ private set; get;}

		public BodyType TBody{ set; get;}
		public AttackType TAttack{ set; get;}
		public DefanceType TDefance{ set; get;}
		public DamageType TDamage{ set; get;}

		public string Name { set; get; }

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
			var dead = HP == 0;//is dead
			if (dead) OnDeath();
			return dead;
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


		public AITreeRoot AIRoot { set; get; }

		internal void Init()
		{
			HP = HPMax.FinalValue;
			_history.Clear();
		}

		protected void OnDeath()
		{
			View.Death();
			Destory(this, 2.5f);
		}


		public void AttackMagicHistory(int magicID, float now) 
		{

			if (_history.ContainsKey(magicID))
			{
				var data = ExcelConfig.ExcelToJSONConfigManager
				                      .Current.GetConfigByID<ExcelConfig.CharacterMagicData>(magicID);
				//cdTime;

				_history.Add(magicID, new ReleaseHistory
				{
					MagicDataID = magicID,
					CdTime =data.TickTime,
					LastTime = now
				});

			}
			else {
				var d = _history[magicID];
				d.LastTime = now;
			}
		}

		public bool IsCoolDown(int magicID, float now, bool autoAttach = false)
		{
			ReleaseHistory h;
			bool isOK = true;
			if (_history.TryGetValue(magicID, out h))
			{ 
				isOK = h.IsCoolDown(now); 
			}
			if (autoAttach)
			{
				AttackMagicHistory(magicID, now);
			}
			return isOK;
		}
	}
}

