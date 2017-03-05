using System;
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
    public Animator Anim;

    [Header("Movement")]
    public float Speed;
    public float JumpPower;

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
    public int RuneCount { get { return Runes.Count; } }

    private RaycastHit LastGroundHit;

    private IController Owner;

    [Header("Health")]
    public bool AnimateIntensity;
    private float StartingIntensity;
    public int MaxHealthPoints = 3;
    private int CurrentHealthPoints;
    public Gradient HealthColor;
    public Material HealthMaterial;
    public string HealthMaterialColorName = "_mainColor";
    public Light HealthLight;

    [Header("Runes")]
    public int MaxRuneCount = 3;
    private List<Rune> Runes = new List<Rune>();
    public Vector3 RuneOffset;

    private bool Jumped;
    
    private int CurrentRune;
    private float CurrentRuneRotation;

    private float LastRuneChangeTime;

    public float RuneChangeDelay;
    
    public void NextRune()
    {
        if(Time.time - LastRuneChangeTime < RuneChangeDelay)
            return;

        LastRuneChangeTime = Time.time;

        CurrentRune++;
        if (CurrentRune == Runes.Count)
            CurrentRune = 0;
    }

    public void PreviousRune()
    {
        if (Time.time - LastRuneChangeTime < RuneChangeDelay)
            return;

        LastRuneChangeTime = Time.time;

        CurrentRune--;
        if (CurrentRune < 0)
            CurrentRune = Runes.Count - 1;
    }

    private float angleQuater;
    private Quaternion currentRot;
    
    public void RotateRune()
    {
        if (Runes.Count == 0)
            return;

        angleQuater = 360.0f / (float) Runes.Count;
        currentRot  = Quaternion.Euler(0, angleQuater * CurrentRune, 0);

        for (int i = 0; i < Runes.Count; i++)
        {
            float currentAngle = angleQuater * i;
            var localRot = Quaternion.Euler(0, currentAngle, 0);

            var finalSmoothRot = Quaternion.RotateTowards(Runes[i].transform.localRotation, currentRot * localRot, Time.deltaTime * angleQuater * 2);

            Runes[i].transform.localRotation = finalSmoothRot;
            Runes[i].transform.localPosition = finalSmoothRot * RuneOffset;
        }
    }

    public void OnStart(IController owner)
    {
        Owner = owner;

        if(!Body)
            Body = GetComponent<Rigidbody>();

        if (!Gravity)
            Gravity = GetComponent<GravityController>();

        if (!Anim)
            Anim = GetComponentInChildren<Animator>();

        Gravity.OnStart();
        transform.up = Gravity.DefaultGravityDir;

        CurrentHealthPoints = MaxHealthPoints;

        if(HealthLight)
            StartingIntensity = HealthLight.intensity;
    }

    private void Update()
    {
        UpdateColor();
    }

    public void OnUpdate()
    {
        RotateRune();
        CheckGrounded();
        HandleMovement();
        HandleAnimation();
    }

    public void HandleAnimation()
    {
        if (!Anim)
            return;

        Anim.SetFloat("Move", Velocity.magnitude);
    }

    public void MovementDirection(Vector3 move)
    {
        DesiredDirection = move;
    }

    public void UpdateColor()
    {
        var coef = CurrentHealthPoints / (float) (MaxHealthPoints);
        var hpColor = HealthColor.Evaluate(coef);

        if (HealthMaterial)
            HealthMaterial.SetColor(HealthMaterialColorName, hpColor);

        if (HealthLight)
        {
            if(AnimateIntensity)
                HealthLight.intensity = Mathf.Lerp(0, StartingIntensity, coef);

            HealthLight.color = hpColor;
        }
    }
    
    public void MovementDirection(float moveX, float moveY)
    {
        FlatDir = new Vector3(moveX, 0, moveY);

        var forward = Camera.main.transform.forward;
        
        DesiredDirection = Quaternion.LookRotation(forward, transform.up) * FlatDir;
    }
    
    public void CheckGrounded()
    {
        if (Jumped)
        {
            Jumped = false;
            IsGrounded = false;
        }
        else
        {
            IsGrounded = Gravity.CheckGravity(GroundRaycastStart.position, -transform.up, GroundRaycastLength);
        }

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

            if (Input.GetButtonDown("Jump"))
            {
                Jumped = true;
            }
        }
        else
        {
            GravityAccumulator += Gravity.GravityStrength;
        }

        if (Jumped)
        {
            GravityAccumulator = JumpPower;
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
        GUI.Label(new Rect(100,200,200,30), "Jumped : " + Jumped);
        GUI.Label(new Rect(100,220,200,30), "QuatRot : " + currentRot);
        GUI.Label(new Rect(100,240,200,30), "AngleQuat : " + angleQuater);
        GUI.Label(new Rect(100,260,200,30), "CurrentRune : " + CurrentRune + "/" + Runes.Count);
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

        if (!Anim)
            return;

        Anim.SetTrigger("OnHit");
    }

    public bool AddRune(Rune rune)
    {
        if (Runes.Count < MaxRuneCount)
        {
            rune.transform.SetParent(Mesh);
            Runes.Add(rune);
            return true;
        }

        return false;
    }

    public void PlaceRune()
    {
        if (Runes.Count > 0)
        {
            var placedRune = Runes[CurrentRune];

            Runes.RemoveAt(CurrentRune);

            CurrentRune = Mathf.Max(0, Mathf.Min(RuneCount - 1, CurrentRune));

            placedRune.transform.SetParent(null);
            placedRune.SpawnRune(this);

            RaycastHit hit;
            Physics.Raycast(transform.position, -Gravity.GravityDirection, out hit);

            placedRune.transform.position = hit.point;
            placedRune.transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
        }
    }

    public void Reset()
    {
        Velocity = Vector3.zero;
        DesiredDirection = Vector3.zero;
        GravityAccumulator = 0;
        Jumped = false;
    }
}
