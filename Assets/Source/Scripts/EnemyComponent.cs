using D2D.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using static D2D.Utilities.CommonGameplayFacade;

[RequireComponent(typeof(OnTriggerEnterComponent))]
public class EnemyComponent : MonoBehaviour
{
    public OnTriggerEnterComponent triggerEnterComponent;
    public Health health;

    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int refreshNavMeshFrames = 4;

    private float currentHealth;

    private NavMeshAgent navMesh;

    private void Awake()
    {
        triggerEnterComponent = GetComponent<OnTriggerEnterComponent>();
        health = GetComponent<Health>();
        navMesh = GetComponent<NavMeshAgent>();

        health.Died += Die;

        currentHealth = maxHealth;
        navMesh.speed = speed;
    }
    private void Update()
    {
        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(_formation.transform.position);
        }
    }
    public void GetHit(float damage)
    {
        health.ApplyDamage(gameObject, damage + (_db.PowerIncreasePercent.Value * damage));
    }
    private void Die()
    {
        _enemySpawn.EnemyDied();

        Destroy(gameObject);
    }
}