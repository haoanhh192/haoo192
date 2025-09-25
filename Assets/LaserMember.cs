using Cinemachine;
using D2D;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class LaserMember : SquadMember
{
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private PoolType bulletPrefab;
    [SerializeField, TagField] private string enemyTag;

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var bullet = _poolHub.Spawn(bulletPrefab, shootPoint.transform.position);
        bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

        var projectile = bullet.GetComponent<ProjectileComponent>();

        projectile.rb.velocity = Vector3.zero;

        _audioManager.PlayOneShot(_gameData.laserClip, .5f);

        var direction = (currentTarget.transform.position - shootPoint.transform.position).normalized;
        projectile.rb.AddForce(direction * projectileForce, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter -= HitEnemy;
        projectile.enterComponent.OnEnter += HitEnemy;

        reloadTime = Time.time + memberClass.ReloadDuration;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyComponent>().GetHit(memberClass.Damage);
        }

        @object.gameObject.SetActive(false);
    }
}