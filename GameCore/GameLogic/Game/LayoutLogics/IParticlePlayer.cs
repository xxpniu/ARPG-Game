using System;

namespace GameLogic.Game.LayoutLogics
{
	public interface IParticlePlayer
	{
		void DestoryParticle();
		void AutoDestory(float time);
		bool CanDestory{ get;}
	}
}

