using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollower : AIBehaviour
{
    private Vector3 DesiredMovement;

    public Transform Target;

    public float MinDistance = 0.5f;

    public float ChangeTargetTime = 2;

    public float SearchSphereRadius;

    public LayerMask SearchLayerMask;

    private float LastTargetTime;
    
    public override Vector3 GetDesiredMovement()
    {
        return DesiredMovement.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, SearchSphereRadius);
    }

    public void SearchForTarget()
    {
        bool found = false;
        var results = Physics.OverlapSphere(Myself.transform.position, SearchSphereRadius, SearchLayerMask);
        foreach (Collider collider in results)
        {
            if (collider.GetComponent<PriorityRune>()
            || collider.GetComponentInParent<RuneHolder>()
            || collider.GetComponentInParent<ActionObject>())
            {
                found = true;

                AquireTarget(collider.transform);

                break;
            }
        }

        if (!found)
        {
            AquireTarget(Player.transform);
        }
    }

    public void AquireTarget(Transform transform)
    {
        LastTargetTime = Time.time;
        Target = transform;
    }

    public override void OnBehave()
    {
        if(Time.time - LastTargetTime > ChangeTargetTime)
            SearchForTarget();

        if(!Target)
            return;

        var move = Target.transform.position - Myself.transform.position;

        if (Mathf.Abs(move.magnitude) < MinDistance)
        {
            DesiredMovement = Vector3.zero;
            return;
        }

        var project = Vector3.ProjectOnPlane(move, Myself.Gravity.GravityDirection);
        var rotated = Quaternion.FromToRotation(Myself.Gravity.GravityDirection, Myself.transform.up) * project;

        DesiredMovement = rotated;

        if (!Myself.DrawDebug)
            return;

        Debug.DrawRay(Myself.transform.position, move, Color.red);
        Debug.DrawRay(Myself.transform.position, project, Color.blue);
        Debug.DrawRay(Myself.transform.position, rotated, Color.green);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(100,300,200,30), "Target : " + Target);
    }
}
