using System;
using EngineCore.Simulater;

namespace GameLogic.Game.Controllors
{
	public class BattleCharacterControllor:GControllor
	{
		public BattleCharacterControllor (GPerception per):base(per)
		{
			
		}

		public override GAction GetAction (GTime time, GObject current)
		{
			
			return  GAction.Empty;
		}



	}
}

