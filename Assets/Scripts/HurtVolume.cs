using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtVolume : MonoBehaviour
{
    [Header("Main")]
    public bool Enabled;
    public bool DrawDebug;

    [Header("Collision")]
    public bool OnStay;
    public bool OnEnter;

    [Header("Damage")]
    public int Damage;
    public float Delay;

    private float LastHurt;

    public Transform OwnerTransform;

    public IDamageable Owner;

    private void Start()
    {
        Owner = (IDamageable) GetComponentInParent(typeof(IDamageable)) 
             ?? (IDamageable) GetComponentInChildren(typeof(IDamageable));
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Enabled || !OnStay || Time.time - LastHurt < Delay)
        {
            return;
        }

        IDamageable damageable = (IDamageable) other.GetComponentInParent(typeof(IDamageable)) 
                              ?? (IDamageable) other.GetComponentInChildren(typeof(IDamageable));
        if (damageable != null && damageable != Owner)
        {
            LastHurt = Time.time;
            damageable.DealDamage(Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Enabled || !OnEnter || Time.time - LastHurt < Delay)
        {
            return;
        }

        IDamageable damageable = (IDamageable) other.GetComponentInParent(typeof(IDamageable))
                              ?? (IDamageable) other.GetComponentInChildren(typeof(IDamageable));
        if (damageable != null && damageable != Owner)
        {
            LastHurt = Time.time;
            damageable.DealDamage(Damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (DrawDebug && OwnerTransform != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, OwnerTransform.position);
        }
    }
}
