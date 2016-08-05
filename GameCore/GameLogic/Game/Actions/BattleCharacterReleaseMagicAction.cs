using System;
using EngineCore.Simulater;
using GameLogic.Game.Perceptions;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;

namespace GameLogic.Game.Actions
{
	public class BattleCharacterReleaseMagicAction:GAction
	{
		public BattleCharacterReleaseMagicAction (string key, BattleCharacter target, GPerception per):base(per)
		{
			this.key = key;
			this.target = target;
		}

		private string key;
		private BattleCharacter target;

		public override void Execute (GTime time, GObject current)
		{
			var per = this.Perceptipn as BattlePerception;
			var release = per.CreateReleaser (key, new ReleaseAtTarget (current as BattleCharacter, target));
		}
	}
}

