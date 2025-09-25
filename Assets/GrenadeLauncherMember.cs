using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherMember : SquadMember
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField, TagField] private string enemyTag;

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var bullet = Instantiate(grenadePrefab, transform.position + Vector3.up / 2, Quaternion.LookRotation(transform.forward));

        var projectile = bullet.GetComponent<ProjectileComponent>();

        var direction = (currentTarget.transform.position - transform.position).normalized;
        projectile.rb.AddForce(direction * 30f, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter += HitEnemy;

        reloadTime = Time.time + memberClass.baseReloadDuration;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyComponent>().GetHit(1);
            Destroy(Instantiate(explosionVFX, other.transform.position, Quaternion.identity), 3f);
        }

        Destroy(@object.gameObject);
    }
}
