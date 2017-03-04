using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBox : ActionObject
{
    private Rigidbody body;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    public override void OnActivate(PlayerController playerController, Pawn pawn)
    {
        body.AddForce(pawn.transform.position - transform.position * 100);
    }

    private void OnDrawGizmosSelected()
    {

    }
}
