using Cinemachine;
using UnityEngine;

public class RifleGunMember : SquadMember
{
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private float delayBetweenRows = 1.2f;
    [SerializeField] private int shotsRow = 4;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField, TagField] private string enemyTag;

    private int shotsInRow;

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var bullet = Instantiate(bulletPrefab, transform.position + Vector3.up / 2, Quaternion.LookRotation(transform.forward));

        var projectile = bullet.GetComponent<ProjectileComponent>();

        var direction = (currentTarget.transform.position - transform.position).normalized;
        projectile.rb.AddForce(direction * projectileForce, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter += HitEnemy;

        shotsInRow++;

        if (shotsInRow % shotsRow == 0)
        {
            reloadTime = Time.time + delayBetweenRows;
        }
        else
        {
            reloadTime = Time.time + memberClass.ReloadDuration;
        }
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