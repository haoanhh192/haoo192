using UnityEngine;

public class EnableHitVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem VFX;

    private void OnTriggerEnter(Collider collision)
    {
        VFX?.Play();
    }
}