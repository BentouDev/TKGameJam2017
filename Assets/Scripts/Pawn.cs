using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;

[RequireComponent(typeof(Rigidbody))]
public class Pawn : MonoBehaviour, IDamageable
{
    [Header("Debug")]
    public bool DrawDebug;

    [Header("References")]
    public Transform Mesh;
    public Rigidbody Body;
    public GravityController Gravity;

    [Header("Movement")]
    public float Speed;

    public Vector3 Velocity { get; private set; }
    
    private float GravityAccumulator;

    private Vector3 DesiredDirection { get; set; }

    [Header("Ground Raycast")]
    public Transform GroundRaycastStart;
    public float GroundRaycastLength;

    [Header("Gravity")]
    public Transform WorldOrigin;
    private Vector3 FlatDir;

    public bool IsGrounded { get; private set; }
    public bool IsAlive { get { return CurrentHealthPoints > 0; } }

    private RaycastHit LastGroundHit;

    private IController Owner;

    [Header("Health")]
    public int MaxHealthPoints = 3;
    private int CurrentHealthPoints;
    public Gradient HealthColor;
    public Material HealthMaterial;
    public string HealthMaterialColorName = "_mainColor";
    public Light HealthLight;
    
    public void OnStart(IController owner)
    {
        Owner = owner;

        if(!Body)
            Body = GetComponent<Rigidbody>();

        if (!Gravity)
            Gravity = GetComponent<GravityController>();

        Gravity.OnStart();
        transform.up = Gravity.DefaultGravityDir;

        CurrentHealthPoints = MaxHealthPoints;
    }

    public void OnUpdate()
    {
        CheckGrounded();
        HandleMovement();
        UpdateColor();
    }

    public void MovementDirection(Vector3 move)
    {
        DesiredDirection = move;
    }

    public void UpdateColor()
    {
        var hpColor = HealthColor.Evaluate(CurrentHealthPoints/(float) (MaxHealthPoints));

        if (HealthMaterial)
            HealthMaterial.SetColor(HealthMaterialColorName, hpColor);

        if (HealthLight)
            HealthLight.color = hpColor;
    }
    
    public void MovementDirection(float moveX, float moveY)
    {
        FlatDir = new Vector3(moveX, 0, moveY);

        var forward = Camera.main.transform.forward;
        
        DesiredDirection = Quaternion.LookRotation(forward, transform.up) * FlatDir;
    }
    
    public void CheckGrounded()
    {
        IsGrounded = Gravity.CheckGravity(GroundRaycastStart.position, -transform.up, GroundRaycastLength);

        var forward = Vector3.Cross(transform.right, Gravity.GravityDirection);
        var target = Quaternion.LookRotation(forward, Gravity.GravityDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, float.PositiveInfinity);

        if (DesiredDirection.magnitude > Mathf.Epsilon)
        {
            Mesh.rotation = Quaternion.RotateTowards(Mesh.rotation, Quaternion.LookRotation(DesiredDirection, Gravity.GravityDirection), float.PositiveInfinity);
        }
    }
    
    void HandleMovement()
    {
        var project = Vector3.ProjectOnPlane(DesiredDirection, Gravity.GravityDirection);
        Velocity = project * Speed;
        
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

        if(!DrawDebug)
            return;

        Debug.DrawRay(transform.position, Velocity, Color.red, 5.0f);
        Debug.DrawRay(transform.position, DesiredDirection, Color.yellow, 5.0f);
        Debug.DrawRay(transform.position, transform.up, Color.green, 5.0f);
        Debug.DrawRay(transform.position, Gravity.GravityDirection, Color.magenta, 5.0f);
    }

    private void FixedUpdate()
    {
        Body.velocity = Velocity;
        Velocity = Vector3.zero;
    }
    
    void OnGUI()
    {
        if (!DrawDebug)
            return;

        GUI.Label(new Rect(100,100,200,30), "Vel : " + Velocity);
        GUI.Label(new Rect(100,120,200,30), "Grav : " + GravityAccumulator);
        GUI.Label(new Rect(100,140,200,30), "Acc : " + GravityAccumulator);
        GUI.Label(new Rect(100,160,200,30), "FlatDir : " + FlatDir);
        GUI.Label(new Rect(100,180,200,30), "Grounded : " + IsGrounded);
    }

    public void StartPromptUsage(ActionObject actionObject)
    {
        Owner.StartPromptUsage(actionObject);
    }

    public void StopPromptUsage(ActionObject actionObject)
    {
        Owner.StopPromptUsage(actionObject);
    }

    public void DealDamage(int damage)
    {
        CurrentHealthPoints -= damage;
    }
}
