using System;
using Astar;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic;
using GameLogic.Game.Perceptions;

namespace MapServer.GameViews
{
    public class ViewBase: IViewBase
    {
        public ViewBase(Pathfinder finder)
        {
            Finder = finder;
        }

        public Pathfinder Finder { private set; get; }

        public IBattlePerception Create(ITimeSimulater simulater)
        {
            return new BattlePerceptionView(simulater, Finder);
        }
    }
}

