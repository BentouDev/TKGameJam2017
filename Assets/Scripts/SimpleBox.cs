using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBox : ActionObject
{
    public bool DefaultGravityFromUp;

    public float RayvastLength = 1.5f;

    public float SphereCastRadius = 1;
    public float SphereCastDistance = 1;

    public HurtVolume Hurt;

    private Rigidbody body;

    public GravityController Gravity;

    private float gravAcc;
    private bool grounded;

    public float MaxVelocity = 60;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();

        if(DefaultGravityFromUp)
            Gravity.DefaultGravityDir = transform.up;

        if(Gravity)
            Gravity.OnStart();
    }

    private void FixedUpdate()
    {
        var colliders = Physics.OverlapSphere(transform.position - Gravity.GravityDirection * SphereCastDistance, SphereCastRadius, Gravity.GroundRaycastMask);
        grounded = false;
        foreach (Collider collider in colliders)
        {
            if (collider.transform.IsChildOf(transform))
                continue;
            else
            {
                grounded = true;
                break;
            }
        }

        // grounded = Gravity.CheckGravity(transform.position, -Gravity.GravityDirection, RayvastLength);
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

    public override void OnActivate(IController playerController, Pawn pawn)
    {
        if (!Gravity)
            return;

        if (grounded)
        {
            Hurt.Owner = pawn;
            Hurt.OwnerTransform = pawn.transform;
            Gravity.GravityDirection = -Gravity.GravityDirection;
        }

        // body.AddForce(pawn.transform.position - transform.position * 100);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Gravity)
            return;

        Gizmos.color = grounded ? Color.red : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Gravity.GravityDirection * SphereCastDistance, SphereCastRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Gravity.GravityDirection * SphereCastDistance, SphereCastRadius);
    }
}
