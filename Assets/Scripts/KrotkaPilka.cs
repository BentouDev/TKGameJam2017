using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrotkaPilka : MonoBehaviour
{
    private RaycastHit Hit;

    public float Speed = 6;

	void Start ()
    {
		
	}
	
	void Update ()
	{
	    var x = Input.GetAxis("MoveX");
	    var y = Input.GetAxis("MoveY");

	    var dir = new Vector3(x, 0, y);

        var move = Quaternion.LookRotation(transform.forward, transform.up) * dir * Speed;

	    Physics.Raycast(transform.position, -transform.up, out Hit);

        transform.rotation = Quaternion.LookRotation(transform.forward, Hit.normal);
	    transform.position += move;
	}
}
