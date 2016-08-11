using System;
using EngineCore.Simulater;
using GameLogic.Game.Perceptions;

namespace MapServer.GameViews
{
    public class ViewBase: GameLogic.IViewBase
    {
        public ViewBase()
        {
            
        }

        public IBattlePerception Create(ITimeSimulater simulater)
        {
            return new BattlePerceptionView(simulater);
        }
    }
}

