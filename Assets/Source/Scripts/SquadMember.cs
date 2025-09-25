using Animancer;
using D2D;
using D2D.Gameplay;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class SquadMember : MonoBehaviour
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
    public float rotationLerp = 5f;

    [HideInInspector]
    public EnemyComponent currentTarget;


    internal float reloadTime = 0;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        overLapObstacle = GetComponent<SexyOverlap>();
        health = GetComponent<Health>();
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