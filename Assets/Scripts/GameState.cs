using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    public MainGame Game { get; private set; }

    public virtual void OnEnd()
    {

    }

    public virtual void OnStart()
    {

    }
    
    public virtual void OnUpdate()
    {
        
    }

    public void Init(MainGame mainGame)
    {
        Game = mainGame;
    }
}
