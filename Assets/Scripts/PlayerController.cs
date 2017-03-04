using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
    public PlayerCamera Camera;
    public GameObject PawnPrefab;
    public List<Transform> StartPoints = new List<Transform>();
    public Transform WorldOrigin;

    public Pawn Pawn { get; private set; }

    public ActionObject CurrentActionObject { get; private set; }

    public override void StartPromptUsage(ActionObject obj)
    {
        CurrentActionObject = obj;
    }

    public override void StopPromptUsage(ActionObject obj)
    {
        CurrentActionObject = null;
    }

    public override void OnStart()
    {
        SpawnPawn();

        if (!Camera)
            Camera = FindObjectOfType<PlayerCamera>();

        Pawn.OnStart(this);
        Camera.SetTarget(Pawn.transform);
        Camera.SetGravity(Pawn.Gravity);
        Camera.OnStart();
    }

    public void SpawnPawn()
    {
        if (Pawn)
            return;

        var startPoint = StartPoints[Random.Range(0, StartPoints.Count)];
        var go = Instantiate(PawnPrefab, startPoint.position, startPoint.rotation) as GameObject;

        var dir = startPoint.position - WorldOrigin.position;

        Pawn = go.GetComponent<Pawn>();
        Pawn.WorldOrigin = WorldOrigin;
        Pawn.Gravity.DefaultGravityDir = -dir.normalized;
    }

    public override void OnUpdate()
    {
        var x = Input.GetAxis("MoveX");
        var y = Input.GetAxis("MoveY");

        if (CurrentActionObject && Input.GetButton("Activate"))
            CurrentActionObject.OnActivate(this, this.Pawn);

        if(Input.GetButton("NextRune"))
            Pawn.NextRune();

        if(Input.GetButton("PreviousRune"))
            Pawn.PreviousRune();

        if (Input.GetButtonDown("PlaceRune"))
            Pawn.PlaceRune();

        Pawn.MovementDirection(x, y);
        Pawn.OnUpdate();

        Camera.LookAngles(Input.GetAxis("LookX"), Input.GetAxis("LookY"));
        Camera.OnUpdate();
    }

    void OnGUI()
    {
        if (!CurrentActionObject)
            return;

        GUI.Label(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 15.0f, 200, 30), CurrentActionObject.GUIText);
    }
}
