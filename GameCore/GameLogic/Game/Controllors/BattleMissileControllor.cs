using System;
using EngineCore;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;

namespace GameLogic.Game.Controllors
{
	public class BattleMissileControllor:GControllor
	{
		public BattleMissileControllor (GPerception per):base(per)
		{
			
		}

		public override GAction GetAction (GTime time, GObject current)
		{
			//var per = this.Perception as BattlePerception;
			var missile = current as BattleMissile;
			switch (missile.State)
			{
				case MissileState.NoStart:
					{
						var distance =GVector3.Distance (missile.Releaser.ReleaserTarget.Releaser.View.Transform.Position,missile.Releaser.ReleaserTarget.ReleaserTarget.View.Transform.Position);
						missile.TotalTime = distance / missile.Layout.speed;
						missile.TimeStart = time.Time;
						missile.State = MissileState.Moving;
						missile.Releaser.OnEvent(Layout.EventType.EVENT_MISSILE_CREATE);
					}
					break;
				case MissileState.Moving:
					{
						if ((time.Time - missile.TimeStart) >= missile.TotalTime)
						{
							missile.Releaser.OnEvent(Layout.EventType.EVENT_MISSILE_HIT);
							missile.State = MissileState.Hit;
						}
					}
					break;
				case MissileState.Hit:
					{
						missile.Releaser.OnEvent(Layout.EventType.EVENT_MISSILE_DEAD);
						missile.State = MissileState.Death;
					}
					break;
				case MissileState.Death:
					GObject.Destory(missile);
					break;
			}
			return GAction.Empty;
		}
	}
}

