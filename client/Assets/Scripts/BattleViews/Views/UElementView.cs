using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using EngineCore.Simulater;
using Google.Protobuf;

public abstract class UElementView : MonoBehaviour, IBattleElement, ISerializerableElement
{

    public int index { set; get; }

	#region IBattleElement implementation

    void IBattleElement.JoinState(int index)
    {
        Joined();
    }

    void IBattleElement.ExitState(int index)
    {
        DestorySelf();  
    }

    void IBattleElement.AttachElement(GObject el)
    {
        OnAttachElement(el);
    }
      
    int IBattleElement.Index{set{ this.index = value; }get{ return index; }}

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

    public void Joined(){}

    public abstract IMessage ToInitNotify();
}
