using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityRune : RuneEffect
{
    public float LifeTime;
    private float StartTime;

    public override void OnApplyEffect(Pawn victim)
    {
        StartTime = Time.time;
        IsActive = true;
    }

    private void Update()
    {
        if (IsActive)
        {
            if(Time.time - StartTime > LifeTime)
                DestroyRune();
        }
    }
}
