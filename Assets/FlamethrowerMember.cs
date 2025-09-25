using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerMember : SquadMember
{
    [SerializeField] private Vector2 projectileForce;
    [SerializeField] private int checkSteps = 5;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField, TagField] private string enemyTag;
    [SerializeField, TagField] private LayerMask enemyLayer;

    private List<EnemyComponent> hitEnemies = new();

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        float step = 1 / checkSteps;

        var sideVector = transform.right / 2;
        
        for (int i = 1; i <= checkSteps; i++)
        {
            var direction = transform.forward + Vector3.Lerp(sideVector, -sideVector, step * i);

            Physics.Raycast(transform.position, direction, out RaycastHit raycastHit, maxDistance, enemyLayer);

            raycastHit.
        }



        reloadTime = Time.time + memberClass.ReloadDuration;
    }

    private void HitEnemy(EnemyComponent enemy)
    {
        enemy.GetHit(memberClass.Damage);
    }
}