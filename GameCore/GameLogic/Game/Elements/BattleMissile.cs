using System;
using EngineCore.Simulater;
using Layout.LayoutElements;

namespace GameLogic.Game.Elements
{

	public enum MissileState
	{
		NoStart,
		Moving,
		Hit,
		Death
	}

	public class BattleMissile:BattleElement<IBattleMissile>
	{
		public BattleMissile(GControllor controllor,MagicReleaser releaser,
		                     IBattleMissile view, 
		                     MissileLayout layout) : base(controllor, view)
		{
			State = MissileState.NoStart;
			Releaser = releaser;
			Layout = layout;
		}
			
		public MagicReleaser Releaser { private set; get; }

		public MissileLayout Layout { private set; get; }

		public MissileState State { set; get; }

		public float TotalTime { get; internal set; }

		public float TimeStart { set; get; }
	}
}

