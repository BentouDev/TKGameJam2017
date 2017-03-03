using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public Vector3 GravityDirection { get; private set; }
    public Vector3 DefaultGravityDir;
    public float GravityStrength = -9.81f;

    public LayerMask GroundRaycastMask;

    private RaycastHit LastGroundHit;

    public void OnStart()
    {
        GravityDirection = DefaultGravityDir;
    }

    public bool CheckGravity(Vector3 rayStart, Vector3 rayDir, float rayLenght)
    {
        bool grounded = Physics.Raycast(rayStart, rayDir, out LastGroundHit, rayLenght, GroundRaycastMask);

        Debug.DrawRay(rayStart, rayDir, Color.green);

        if (grounded)
        {
            GravityDirection = LastGroundHit.normal;
            Debug.DrawRay(LastGroundHit.point, LastGroundHit.normal, Color.cyan);
        }

        return grounded;
    }
}
