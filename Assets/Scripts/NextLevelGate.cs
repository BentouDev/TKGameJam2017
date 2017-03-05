using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelGate : MonoBehaviour
{
    public bool IsActive;

    public AnimationPlayer OpeningAnim;

    public ParticleSystem Particles;

    public bool Entered { get; private set; }

    void Start()
    {
        Particles = GetComponentInChildren<ParticleSystem>();
        Particles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Pawn>())
            return;

        if (IsActive)
        {
            Entered = true;
            MainGame.Instance.NextLevel();
        }
    }

    public void Open()
    {
        IsActive = true;
        Particles.Play();
        OpeningAnim.Play();
    }
}
