using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pawn : MonoBehaviour
{
    [Header("Debug")]
    public bool DrawDebug;

    [Header("References")]
    public Rigidbody Body;
    public GravityController Gravity;

    [Header("Movement")]
    public float Speed;

    public Vector3 Velocity { get; private set; }

    public Vector3 GravityForce { get { return GravityAccumulator * Gravity.GravityDirection; } }

    private float GravityAccumulator;

    private Vector3 DesiredDirection { get; set; }

    [Header("Ground Raycast")]
    public Transform GroundRaycastStart;
    public float GroundRaycastLength;

    [Header("Gravity")]
    public Transform WorldOrigin;
    private Vector3 FaceDirection;
    private Vector3 FlatDir;

    public bool IsGrounded { get; private set; }
    public bool IsAlive { get; private set; }

    private RaycastHit LastGroundHit;
    
    public void OnStart()
    {
        if(!Body)
            Body = GetComponent<Rigidbody>();

        if (!Gravity)
            Gravity = GetComponent<GravityController>();

        Gravity.OnStart();
    }

    public void OnUpdate()
    {
        CheckGrounded();
        HandleMovement();
    }
    
    public void MovementDirection(float moveX, float moveY)
    {
        FlatDir = new Vector3(moveX, 0, moveY);
        
        DesiredDirection = Quaternion.LookRotation(Vector3.Normalize(transform.forward), Gravity.GravityDirection) * FlatDir;
    }
    
    public void CheckGrounded()
    {
        IsGrounded = Gravity.CheckGravity(GroundRaycastStart.position, -transform.up, GroundRaycastLength);
        transform.up = Gravity.GravityDirection;
    }

    void HandleMovement()
    {
        Velocity = FlatDir * Speed;

        if (IsGrounded)
        {
            GravityAccumulator = 0;
            Velocity = Quaternion.FromToRotation(Vector3.up, Gravity.GravityDirection) * Velocity;
        }
        else
        {
            GravityAccumulator += Gravity.GravityStrength;
        }

        Velocity += GravityForce;
    }

    private void FixedUpdate()
    {
        Body.velocity = Velocity;
    }
    
    void OnGUI()
    {
        if (!DrawDebug)
            return;

        GUI.Label(new Rect(100,100,200,30), "Vel : " + Velocity);
        GUI.Label(new Rect(100,120,200,30), "Grav : " + GravityForce);
        GUI.Label(new Rect(100,140,200,30), "Acc : " + GravityAccumulator);
        GUI.Label(new Rect(100,160,200,30), "FlatDir : " + FlatDir);
        GUI.Label(new Rect(100,180,200,30), "Grounded : " + IsGrounded);
    }
}
