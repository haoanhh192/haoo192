using Animancer;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utilities;
using UnityEngine;
using UnityEngine.AI;
using static D2D.Utilities.CommonGameplayFacade;

[RequireComponent(typeof(OnTriggerEnterComponent))]
public class EnemyComponent : Unit
{
    public OnTriggerEnterComponent triggerEnterComponent;
    public Health health;

    [SerializeField] private float speed = 10f;
    [SerializeField] private int refreshNavMeshFrames = 4;
    [SerializeField] private Animations animations;

    [Header("Combat")]
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private SexyOverlap overlap;

    [Header("After Death")]
    [SerializeField] private GameObject powerUpPrefab;

    private float attackTimer;

    private NavMeshAgent navMesh;
    private CharacterCanvas canvas;
    private AnimancerComponent animancer;

    private void Awake()
    {
        triggerEnterComponent = GetComponent<OnTriggerEnterComponent>();
        health = GetComponent<Health>();
        navMesh = GetComponent<NavMeshAgent>();
        animancer = GetComponentInChildren<AnimancerComponent>();
        canvas = GetComponentInChildren<CharacterCanvas>();

        canvas.HealthBar.SetHealth(health);

        animancer.Play(animations.UnarmedRun);

        if (overlap == null)
        {
            overlap = GetComponentInChildren<SexyOverlap>();
        }

        navMesh.speed = speed;

        health.Died += Die;

        _stateMachine.On<WinState>(Die);
    }
    private void Update()
    {
        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(_formation.transform.position);
        }

        if (overlap.HasTouch && attackTimer <= Time.time)
        {
            var closestPlayer = overlap.NearestTouchedOfType<SquadMember>(transform);

            closestPlayer.health.ApplyDamage(gameObject, damage);

            attackTimer = Time.time + attackRate;
        }
    }
    public void GetHit(float damage)
    {
        health.ApplyDamage(gameObject, damage);
    }
    private void Die()
    {
        _enemySpawn.EnemyDied();

        var powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity).Get<XPPoint>();
        powerUp.Init(transform.position + transform.forward);

        Destroy(gameObject);
    }
}