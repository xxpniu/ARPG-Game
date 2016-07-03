using System;
using GameLogic.Game.Elements;

namespace GameLogic.Game.Perceptions
{
	public interface IBattlePerception
	{
		IBattleCharacter CreateBattleCharacterView();
	}
}

