using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TrapRune : RuneEffect
{
    private float OnEnterTime;
    public float TrapTime;

    private Pawn Victim;

    public override void OnApplyEffect(Pawn victim)
    {
        OnEnterTime = Time.time;
        Victim = victim;
        IsActive = true;
    }

    void Update()
    {
        if (IsActive && Time.time - OnEnterTime < TrapTime)
        {
            Victim.Reset();
        }
        else if(IsActive && Time.time - OnEnterTime > TrapTime)
        {
            Victim = null;
            DestroyRune();
        }
    }
}
