using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollower : AIBehaviour
{
    private Vector3 DesiredMovement;

    public float MinDistance = 0.5f;
    
    public override Vector3 GetDesiredMovement()
    {
        return DesiredMovement.normalized;
    }

    public override void OnBehave()
    {
        var move = Player.transform.position - Myself.transform.position;

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
}
