using System;
namespace MapServer.GameViews
{
    public class ParticlePlayerView: GameLogic.Game.LayoutLogics.IParticlePlayer
    {
        public ParticlePlayerView()
        {
        }

        public bool CanDestory
        {
            get
            {
                return true;
            }
        }

        public void AutoDestory(float time)
        {
            
        }

        public void DestoryParticle()
        {
           
        }
    }
}

