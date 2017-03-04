using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBox : ActionObject
{
    public HurtVolume Hurt;

    private Rigidbody body;

    public GravityController Gravity;

    private float gravAcc;
    private bool grounded;

    public float MaxVelocity = 60;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();

        if(Gravity)
            Gravity.OnStart();
    }

    private void FixedUpdate()
    {
        grounded = Gravity.CheckGravity(transform.position, -Gravity.GravityDirection, 1.25f);
        if (!grounded)
        {
            gravAcc += Gravity.GravityStrength;
            gravAcc = Mathf.Min(Mathf.Max(MaxVelocity, gravAcc), -MaxVelocity);
        }
        else
        {
            gravAcc = 0;
            Hurt.Owner = null;
        }

        Hurt.Enabled = !grounded;

        body.velocity = Gravity.GravityDirection * gravAcc;
    }

    public override void OnActivate(PlayerController playerController, Pawn pawn)
    {
        if (!Gravity)
            return;

        if (grounded)
        {
            Hurt.Owner = pawn;
            Gravity.GravityDirection = -Gravity.GravityDirection;
        }

        // body.AddForce(pawn.transform.position - transform.position * 100);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Gravity)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Gravity.GravityDirection * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -Gravity.GravityDirection * 2);
    }
}
