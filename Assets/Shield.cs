using D2D;
using D2D.Gameplay;
using UnityEngine;

public class Shield : Unit
{
    private Rigidbody[] rb;

    private void Awake()
    {
        rb = ChildrenGets<Rigidbody>();

        ParentGet<Health>().Died += OnDie;
    }

    private void OnDie()
    {
        foreach (var rigid in rb)
        {
            rigid.isKinematic = false;
        }
    }
}