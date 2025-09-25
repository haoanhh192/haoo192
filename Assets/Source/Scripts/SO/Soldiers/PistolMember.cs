using Cinemachine;
using UnityEngine;

public class PistolMember : SquadMember
{
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField, TagField] private string enemyTag;

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