using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    public Pawn Player { get; private set; }
    public Pawn Myself { get; private set; }

    public void OnStart(Pawn pawn, Pawn self)
    {
        Player = pawn;
        Myself = self;
    }

    public abstract void OnBehave();

    public abstract Vector3 GetDesiredMovement();
}
