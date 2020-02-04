using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using EngineCore.Simulater;
using Google.Protobuf;

public abstract class UElementView : MonoBehaviour, IBattleElement, ISerializerableElement
{

    public int Index { set; get; }

	#region IBattleElement implementation

    void IBattleElement.JoinState(int index)
    {
        OnJoined();
        this.Index = index;
        PerView.AttachView(this);
    }

    void IBattleElement.ExitState(int index)
    {
        PerView.DeAttachView(this);
        DestorySelf();  
    }

    void IBattleElement.AttachElement(GObject el)
    {
        OnAttachElement(el);
    }

	#endregion

    public GObject Element { private set; get; }

    public virtual void OnAttachElement(GObject el)
    {
        Element = el;
    }

    public void DestorySelf()
    {
        GameObject.Destroy (this.gameObject);   
    }

    public virtual void OnJoined() { }

    public abstract IMessage ToInitNotify();

    public UPerceptionView PerView { private set; get; }

    public void SetPrecpetion(UPerceptionView view)
    {
        PerView = view;
    }
}
