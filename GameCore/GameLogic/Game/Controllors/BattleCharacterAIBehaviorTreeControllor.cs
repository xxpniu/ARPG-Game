using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;

namespace GameLogic
{
	public class BattleCharacterAIBehaviorTreeControllor:GControllor
	{
		public BattleCharacterAIBehaviorTreeControllor(GPerception per):base(per)
		{
			
		}

		public override GAction GetAction(GTime time, GObject current)
		{
			var character = current as BattleCharacter;
			if (character.AIRoot != null)
				character.AIRoot.Tick();
			return GAction.Empty;
		}
	}
}

