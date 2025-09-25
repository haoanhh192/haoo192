using Animancer;
using D2D;
using D2D.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

using static D2D.Utilities.CommonGameplayFacade;

[RequireComponent(typeof(Health))]
public class SquadMember : Unit
{
    public NavMeshAgent navMesh;
    public SexyOverlap overLapObstacle;
    public SexyOverlap overLapEnemy;
    public MemberClass memberClass;
    public Animations animations;
    public Health health;
    public AnimancerComponent animancer;

    [Header("Movement Settings")]
    public Vector3 targetVector;
    public float rotationLerp = 20f;

    [HideInInspector]
    public EnemyComponent currentTarget;

    private CharacterCanvas canvas;
    private Camera currentCamera;

    internal float reloadTime = 0;


    [Button("Debug Kill")]
    private void DebugKill()
    {
        health.ApplyDamage(null, health.CurrentPoints);
    }
    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        overLapObstacle = GetComponent<SexyOverlap>();
        health = GetComponent<Health>();
        canvas = GetComponentInChildren<CharacterCanvas>();

        canvas.HealthBar.SetHealth(health);
    }
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetVector - transform.position), Time.deltaTime * rotationLerp);
    }
    public virtual void Init()
    {

    }
    public virtual void Shoot(Transform target)
    {

    }
}