using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using Proto;

namespace MapServer.GameViews
{
    public abstract class BattleElement : IBattleElement, ISerializerableElement
    {
        public BattleElement(BattlePerceptionView view)
        {
            PerceptionView = view;
        }

        public int Index { set; get; }
        public GObject Element { private set; get; }

        public BattlePerceptionView PerceptionView { private set; get; }

        void IBattleElement.ExitState(int index)
        {
            PerceptionView.DeAttachView(this);
        }

        void IBattleElement.JoinState(int index)
        {
            Index = index;
            PerceptionView.AttachView(this);
        }

        void IBattleElement.AttachElement(GObject el)
        {

            this.Element = el;
        }
        public abstract ISerializerable GetInitNotify();


        public virtual void Update(GTime time)
        {

        }

    }
}

