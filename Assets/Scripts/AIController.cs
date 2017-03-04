using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public bool DrawDebug;
    public GameObject PawnPrefab;
    public Transform StartPoint;
    public Transform WorldOrigin;

    public Pawn Pawn { get; private set; }

    public void OnStart()
    {
        SpawnPawn();
        
        Pawn.OnStart();
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
        Pawn.DrawDebug = DrawDebug;
        Pawn.MovementDirection(0, 0);
        Pawn.OnUpdate();
    }
}
