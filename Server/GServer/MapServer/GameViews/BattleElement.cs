using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
namespace MapServer.GameViews
{
    public class BattleElement :IBattleElement
    {
        public BattleElement(BattlePerceptionView view)
        {
            PerceptionView = view;
        }

        public long Index { set; get; }

        public BattlePerceptionView PerceptionView { private set; get; }

        public void ExitState(GObject el)
        {
            PerceptionView.DeAttachView(this);
        }

        public void JoinState(GObject el)
        {
            Index = el.Index;
            PerceptionView.AttachView(this);
        }

        public virtual void Update(GTime time)
        { 
        
        }
    }
}

