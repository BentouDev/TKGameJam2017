using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class ActionObject : MonoBehaviour
{
    private bool ShowGUI;
    public string GUIText;

    protected virtual void TriggerEnter(Collider collider, Pawn pawn)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var pawn = other.GetComponentInParent<Pawn>();
        if (pawn)
        {
            pawn.StartPromptUsage(this);
        }

        TriggerEnter(other, pawn);
    }

    private void OnTriggerExit(Collider other)
    {
        var pawn = other.GetComponentInParent<Pawn>();
        if (pawn)
            pawn.StopPromptUsage(this);
    }

    public abstract void OnActivate(PlayerController playerController, Pawn pawn);
}
