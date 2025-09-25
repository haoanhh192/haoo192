using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;
public class GrenadeLauncherMember : SquadMember
{
    [SerializeField] private float projectileForce = 30f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField, TagField] private string enemyTag;
    [SerializeField] private LayerMask enemyLayer;

    public override void Init()
    {
        runForward = animations.RunWithRifle;
    }
    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var bullet = Instantiate(grenadePrefab, shootPoint.transform.position, Quaternion.LookRotation(currentTarget.transform.position - shootPoint.transform.position));

        var muzzleFlash = _poolHub.Spawn(_gameData.bulletMuzzleFlash, shootPoint.transform.position);
        muzzleFlash.transform.rotation = Quaternion.LookRotation(transform.forward);

        var projectile = bullet.GetComponent<ProjectileComponent>();

        var direction = (currentTarget.transform.position - shootPoint.transform.position).normalized;
        projectile.rb.AddForce(direction * projectileForce, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter += HitEnemy;

        reloadTime = Time.time + memberClass.ReloadDuration;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(enemyTag))
        {
            var enemies = Physics.OverlapSphere(other.transform.position, explosionRadius, _gameData.EnemyLayer);

            foreach (var enemy in enemies)
            {
                enemy.GetComponent<EnemyComponent>().GetHit(memberClass.Damage);
            }

            Destroy(Instantiate(explosionVFX, other.transform.position, Quaternion.identity), 3f);
        }

        Destroy(@object.gameObject);
    }
}
