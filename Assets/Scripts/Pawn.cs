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
    
    private float GravityAccumulator;

    private Quaternion DesiredDirection { get; set; }

    [Header("Ground Raycast")]
    public Transform GroundRaycastStart;
    public float GroundRaycastLength;

    [Header("Gravity")]
    public Transform WorldOrigin;
    private Quaternion FlatRot;
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
        FlatRot = Quaternion.LookRotation(FlatDir);
        
        DesiredDirection = Quaternion.LookRotation(transform.forward, transform.up) * FlatRot;
    }
    
    public void CheckGrounded()
    {
        IsGrounded = Gravity.CheckGravity(GroundRaycastStart.position, -transform.up, GroundRaycastLength);

        var target = Quaternion.LookRotation(transform.forward, Gravity.GravityDirection);
        var final  = Quaternion.RotateTowards(transform.rotation, target, float.PositiveInfinity);

        transform.rotation = final;
    }
    
    void HandleMovement()
    {
        Velocity = DesiredDirection * Vector3.forward * Speed * FlatDir.magnitude;
        
        if (IsGrounded)
        {
            GravityAccumulator = 0;
        }
        else
        {
            GravityAccumulator += Gravity.GravityStrength;
        }

        Velocity += GravityAccumulator * Gravity.GravityDirection;
        
        //Velocity = Quaternion.FromToRotation(Vector3.up, Gravity.GravityDirection) * Velocity;

        Debug.DrawRay(transform.position, Velocity, Color.red, 5.0f);
        Debug.DrawRay(transform.position, DesiredDirection * Vector3.forward, Color.yellow, 5.0f);
        Debug.DrawRay(transform.position, transform.up, Color.green, 5.0f);
        Debug.DrawRay(transform.position, Gravity.GravityDirection, Color.magenta, 5.0f);
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
        GUI.Label(new Rect(100,120,200,30), "Grav : " + GravityAccumulator);
        GUI.Label(new Rect(100,140,200,30), "Acc : " + GravityAccumulator);
        GUI.Label(new Rect(100,160,200,30), "FlatDir : " + FlatRot);
        GUI.Label(new Rect(100,180,200,30), "Grounded : " + IsGrounded);
    }
}
