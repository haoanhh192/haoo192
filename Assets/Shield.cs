using D2D;
using D2D.Gameplay;
using UnityEngine;

public class Shield : Unit
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = Get<Rigidbody>();

        ParentGet<Health>().Died += OnDie;
    }

    private void OnDie()
    {
        rb.isKinematic = false;

        rb.AddForce(-transform.forward * 50f);
    }
}