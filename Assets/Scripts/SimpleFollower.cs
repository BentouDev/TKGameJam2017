using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollower : AIBehaviour
{
    private Vector3 DesiredMovement;
    
    public override Vector3 GetDesiredMovement()
    {
        return DesiredMovement.normalized;
    }

    public override void OnBehave()
    {
        var dot  = Vector3.Dot(Myself.transform.up, Player.transform.up);
        var move = Player.transform.position - Myself.transform.position;

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
