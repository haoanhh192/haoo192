using Cinemachine;
using UnityEngine;

public class PistolMember : SquadMember
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField, TagField] private string enemyTag;

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var bullet = Instantiate(bulletPrefab, transform.position + Vector3.up, Quaternion.LookRotation(transform.forward));

        var projectile = bullet.GetComponent<ProjectileComponent>();

        projectile.rb.AddForce(bullet.transform.forward * 10f, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter += HitEnemy;

        reloadTime = Time.time + memberClass.baseReloadDuration;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyComponent>().GetHit(1);
        }

        Destroy(@object.gameObject);
    }
}