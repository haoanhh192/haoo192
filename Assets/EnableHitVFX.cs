using D2D;
using DG.Tweening;
using UnityEngine;

public class EnableHitVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem VFX;

    private Tween tween;

    private void OnTriggerEnter(Collider collision)
    {
        VFX?.Play();

        tween.KillTo0();

        tween = transform.DOPunchScale(Vector3.one * 0.005f, .2f);
    }
}