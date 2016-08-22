using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;

public class UElementView : MonoBehaviour, IBattleElement {

    public long Index;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IBattleElement implementation

	public virtual void JoinState (EngineCore.Simulater.GObject el)
	{
        this.Index = el.Index;
	}

	public virtual void ExitState (EngineCore.Simulater.GObject el)
	{
        DestorySelf();
	}

	#endregion


    public void DestorySelf()
    {
        GameObject.Destroy (this.gameObject);   
    }
}
