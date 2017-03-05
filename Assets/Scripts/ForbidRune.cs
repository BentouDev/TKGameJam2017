using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbidRune : RuneEffect
{
    public SphereCollider Collider;

    public float LifeTime = 10;

    private float StartTime;

    protected override void OnPostStart()
    {
        foreach (Collider collider in Owner.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(Collider, collider);
        }
    }

    public override void OnApplyEffect(Pawn victim)
    {
        IsActive = true;
        StartTime = Time.time;
    }

    private void Update()
    {
        if (IsActive && Time.time - StartTime > LifeTime)
        {
            DestroyRune();
        }
    }
}
