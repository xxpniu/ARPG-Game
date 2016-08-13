using System;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic;
using GameLogic.Game.Perceptions;

namespace MapServer.GameViews
{
    public class ViewBase: IViewBase
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

