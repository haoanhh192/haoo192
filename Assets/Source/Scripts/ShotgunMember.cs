using Cinemachine;
using D2D;
using D2D.Utilities;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class ShotgunMember : SquadMember
{
    [SerializeField] private Vector2 projectileForce;
    [SerializeField] private int shotParts = 5;
    [SerializeField] private PoolType bulletPrefab;
    [SerializeField, TagField] private string enemyTag;

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var rightVector = transform.right / 2;

        for (int i = 0; i < shotParts; i++)
        {
            var bullet = _poolHub.Spawn(bulletPrefab, shootPoint.transform.position);
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

            var projectile = bullet.GetComponent<ProjectileComponent>();

            var muzzleFlash = Instantiate(_gameData.muzzleFlash, shootPoint.transform.position, Quaternion.LookRotation(transform.forward));
            Destroy(muzzleFlash, 2f);

            var direction = (currentTarget.transform.position - shootPoint.transform.position).normalized;

            var randomDir = Vector3.Lerp(-rightVector, rightVector, Random.value);

            projectile.rb.AddForce((direction + randomDir)* projectileForce.RandomFloat(), ForceMode.VelocityChange);

            projectile.enterComponent.OnEnter -= HitEnemy;
            projectile.enterComponent.OnEnter += HitEnemy;
        }

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