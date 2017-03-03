using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCamera Camera;
    public GameObject PawnPrefab;
    public Transform StartPoint;
    public Transform WorldOrigin;

    public Pawn Pawn { get; private set; }
    
    public void OnStart()
    {
        SpawnPawn();

        if (!Camera)
            Camera = FindObjectOfType<PlayerCamera>();

        Pawn.OnStart();
        Camera.SetTarget(Pawn.transform);
        Camera.SetGravity(Pawn.Gravity);
        Camera.OnStart();
    }

    public void SpawnPawn()
    {
        if (Pawn)
            return;

        var go = Instantiate(PawnPrefab, StartPoint.position, StartPoint.rotation) as GameObject;
        Pawn = go.GetComponent<Pawn>();
        Pawn.WorldOrigin = WorldOrigin;
    }

    public void OnUpdate()
    {
        var x = Input.GetAxis("MoveX");
        var y = Input.GetAxis("MoveY");

        Pawn.MovementDirection(x, y);
        Pawn.OnUpdate();

        Camera.LookAngles(Input.GetAxis("LookX"), Input.GetAxis("LookY"));
        Camera.OnUpdate();
    }
}
