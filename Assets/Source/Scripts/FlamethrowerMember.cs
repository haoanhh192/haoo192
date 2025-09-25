using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;
public class FlamethrowerMember : SquadMember
{
    [SerializeField] private int checkSteps = 5;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField, TagField] private string enemyTag;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private ParticleSystem flamesVFX;

    private List<EnemyComponent> hitEnemies = new();

    private List<Ray> drawRay = new();

    private void OnDrawGizmos()
    {
        foreach (var ray in drawRay)
        {
            Gizmos.DrawRay(ray);        
        }
    }
    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        float step = 1f / checkSteps;

        var sideVector = Vector3.right / 2;

        drawRay = new();

        for (int i = 1; i <= checkSteps; i++)
        {
            var direction = transform.forward + Vector3.Lerp(sideVector, -sideVector, step * i);

            Ray ray = new Ray(transform.position, direction);

            drawRay.Add(ray);

            var hits = Physics.RaycastAll(ray, maxDistance, _gameData.EnemyLayer);

            foreach (var hit in hits)
            {
                var enemy = hit.collider.GetComponent<EnemyComponent>();

                if (enemy != null && !hitEnemies.Contains(enemy))
                {
                    hitEnemies.Add(enemy);
                }
            }
        }

        if (hitEnemies.Count > 0)
        {
            foreach (var enemy in hitEnemies)
            {
                enemy.GetHit(memberClass.Damage);
            }

            flamesVFX.Play();

            hitEnemies.Clear();
        }
        else
        {
            flamesVFX.Stop();
        }

        reloadTime = Time.time + memberClass.ReloadDuration;
    }
}