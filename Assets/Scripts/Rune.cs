using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public GameObject RuneEffectPrefab;

    public void SpawnRune(Pawn pawn)
    {
        var go = Instantiate(RuneEffectPrefab);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            go.transform.SetParent(transform, true);

        var rune = go.GetComponent<RuneEffect>();

        rune.OnStart(pawn, this);
    }
}
