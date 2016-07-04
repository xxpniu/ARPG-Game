using System;
using GameLogic.Game.Elements;
using EngineCore;

namespace GameLogic.Game.LayoutLogics
{
	public interface IReleaserTarget
	{
		BattleCharacter Releaser{ get; }
		BattleCharacter ReleaserTarget{ get; }
		GVector3 TargetPosition{ get; }
	}
}

