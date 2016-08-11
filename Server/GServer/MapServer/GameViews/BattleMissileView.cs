using System;
using EngineCore;
using EngineCore.Simulater;
using GameLogic;
using GameLogic.Game.Elements;

namespace MapServer.GameViews
{
    public class BattleMissileView : BattleElement,IBattleMissile
    {
        public BattleMissileView(BattlePerceptionView view):base(view)
        {
        }

        private Transform transform;

        public ITransform Transform
        {
            get
            {
                return transform;
            }
        }


        public void SetPosition(GVector3 pos)
        {
            transform.Position = pos;
        }

    }
}

