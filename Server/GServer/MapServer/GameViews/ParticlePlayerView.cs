using System;
using GameLogic.Game.LayoutLogics;

namespace MapServer.GameViews
{
    public class ParticlePlayerView: IParticlePlayer
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

