using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class ActionObject : MonoBehaviour
{
    private bool ShowGUI;
    public string GUIText;

    private void OnTriggerEnter(Collider other)
    {
        var pawn = other.GetComponentInParent<Pawn>();
        if (pawn)
            pawn.StartPromptUsage(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var pawn = other.GetComponentInParent<Pawn>();
        if (pawn)
            pawn.StopPromptUsage(this);
    }

    public abstract void OnActivate(PlayerController playerController, Pawn pawn);
}
