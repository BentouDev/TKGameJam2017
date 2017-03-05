using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuneEffect : MonoBehaviour
{
    protected bool IsActive = false;

    public Pawn Owner;
    public Rune Parent;
    
    public void OnStart(Pawn owner, Rune parent)
    {
        Owner = owner;
        Parent = parent;

        OnPostStart();
    }

    protected virtual void OnPostStart()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var pawn = other.GetComponentInParent<Pawn>()
                ?? other.GetComponentInChildren<Pawn>();
        if (pawn != Owner)
        {
            OnApplyEffect(pawn);
        }
    }

    public abstract void OnApplyEffect(Pawn victim);

    protected void DestroyRune()
    {
        DestroyObject(Parent.gameObject);
    }
}
