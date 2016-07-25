using System;
using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using GameLogic.Game.LayoutLogics;

public class UParticlePlayer:MonoBehaviour, IParticlePlayer
{

    private bool IsDestory = false;

    #region IParticlePlayer implementation
    public void DestoryParticle()
    {
        IsDestory = true;
        Destroy(this.gameObject);
    }

    public void AutoDestory(float time)
    {
        IsDestory = true;
        Destroy(gameObject, time); 
    }
        

    public bool CanDestory
    {
        get
        {
            return !IsDestory;
        }
    }
    #endregion
   
}
