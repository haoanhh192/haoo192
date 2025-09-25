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
    private CapsuleCollider collider;

    private bool isDead = false;

    private void Awake()
    {
        triggerEnterComponent = GetComponent<OnTriggerEnterComponent>();
        health = GetComponent<Health>();
        navMesh = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        animancer = GetComponentInChildren<AnimancerComponent>();
        canvas = GetComponentInChildren<CharacterCanvas>();

        canvas.HealthBar.SetHealth(health);
        health.SetMaxPoints(health.MaxPoints + (health.MaxPoints * _db.PassedLevels.Value / 10), true);

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
        if (isDead)
        {
            return;
        }

        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(_formation.transform.position);

            if (Vector3.Distance(transform.position, _formation.transform.position) > _gameData.enemyDespawnDistance)
            {
                DespawnEnemy();
            }
        }

        if (overlap.HasTouch && attackTimer <= Time.time)
        {
            var closestPlayer = overlap.NearestTouchedOfType<SquadMember>(transform);
            
            if (closestPlayer != null)
            {
                closestPlayer.health.ApplyDamage(gameObject, damage);
            }

            attackTimer = Time.time + attackRate;
        }
    }
    public void SetSpeed(float multiplier)
    {
        navMesh.speed = speed * multiplier;
    }
    public void SetHealth(float multiplier)
    {
        health.SetMaxPoints(health.MaxPoints * multiplier, true);
    }
    public void GetHit(float damage)
    {
        if (isDead)
        {
            return;
        }

        health.ApplyDamage(gameObject, damage);
    }
    private void Die()
    {
        var powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity).Get<XPPoint>();
        powerUp.Init(transform.position, _formation.transform.position);

        _enemySpawn.EnemyDied();

        isDead = true;

        navMesh.isStopped = true;
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;

        canvas.HealthBar.gameObject.SetActive(false);

        overlap.enabled = false;

        collider.enabled = false;

        animancer.Animator.applyRootMotion = true;
        animancer.Play(animations.Death);

        Destroy(gameObject, 3f);
    }
    private void DespawnEnemy()
    {
        _enemySpawn.EnemyDied();

        isDead = true;

        Destroy(gameObject);
    }
}