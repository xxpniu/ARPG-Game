using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using EngineCore.Simulater;

public class UElementView : MonoBehaviour, IBattleElement {

    public int index { set; get; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
        

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
        
    }
      
    int IBattleElement.Index{set{ this.index = value; }get{ return index; }}

	#endregion

    public virtual void OnAttachElement(GObject el)
    {
    }

    public void DestorySelf()
    {
        GameObject.Destroy (this.gameObject);   
    }

    public void Joined()
    {}
}
