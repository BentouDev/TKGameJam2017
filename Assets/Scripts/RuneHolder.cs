using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RuneHolder : MonoBehaviour
{
    public Rune Rune;
    public bool PlayerOnly;
    public float DeathAnimTime;

    private void OnTriggerEnter(Collider other)
    {
        if (Rune == null)
            return;

        var pawn = other.GetComponentInParent<Pawn>() ?? other.GetComponentInChildren<Pawn>();
        if (pawn && pawn.AddRune(Rune))
        {
            Rune = null;
            StartCoroutine(KillMe());
        }
    }

    IEnumerator KillMe()
    {
        yield return new WaitForSeconds(DeathAnimTime);
        DestroyObject(gameObject);
    }
}
