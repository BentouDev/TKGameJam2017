using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public Vector3 GravityDirection { get; private set; }
    public Vector3 DefaultGravityDir;
    public float GravityStrength = -9.81f;
    public float SnapThreshold = 0.0001f;

    public LayerMask GroundRaycastMask;

    private RaycastHit LastGroundHit;

    public void OnStart()
    {
        GravityDirection = DefaultGravityDir;
    }

    public bool CheckGravity(Vector3 rayStart, Vector3 rayDir, float rayLenght)
    {
        bool grounded = Physics.Raycast(rayStart, rayDir, out LastGroundHit, rayLenght, GroundRaycastMask);
        
        if (grounded)
        {
            GravityDirection = Vector3.Lerp(GravityDirection, LastGroundHit.normal, 0.5f); // Vector3.RotateTowards(GravityDirection, LastGroundHit.normal, 1, 1);//
            //if (Mathf.Abs((GravityDirection - LastGroundHit.normal).magnitude) < SnapThreshold)
            //    GravityDirection = LastGroundHit.normal;
        }

        return grounded;
    }
}
