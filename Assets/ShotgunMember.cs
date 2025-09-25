using Cinemachine;
using D2D.Utilities;
using UnityEngine;

public class ShotgunMember : SquadMember
{
    [SerializeField] private Vector2 projectileForce;
    [SerializeField] private int shotParts = 5;
    [SerializeField] private GameObject bulletPrefab;
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
            var bullet = Instantiate(bulletPrefab, transform.position + Vector3.up / 2, Quaternion.LookRotation(transform.forward));

            var projectile = bullet.GetComponent<ProjectileComponent>();

            var direction = (currentTarget.transform.position - transform.position).normalized;

            var randomDir = Vector3.Lerp(-rightVector, rightVector, Random.value);

            projectile.rb.AddForce((direction + randomDir)* projectileForce.RandomFloat(), ForceMode.VelocityChange);

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

        Destroy(@object.gameObject);
    }
}