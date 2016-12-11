using System;
using GameLogic.Game.Elements;
using EngineCore;
using UMath;

namespace GameLogic.Game.LayoutLogics
{
	public interface IReleaserTarget
	{
		BattleCharacter Releaser{ get; }
		BattleCharacter ReleaserTarget{ get; }
		UVector3 TargetPosition{ get; }
	}
}

