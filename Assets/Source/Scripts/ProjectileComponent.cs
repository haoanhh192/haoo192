using UnityEngine;

[RequireComponent(typeof(OnTriggerEnterComponent))]
public class ProjectileComponent : MonoBehaviour
{
    public OnTriggerEnterComponent enterComponent;
    public Rigidbody rb;

    private void Awake()
    {
        enterComponent = GetComponent<OnTriggerEnterComponent>();
        rb = GetComponent<Rigidbody>();
    }
}