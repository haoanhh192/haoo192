using Cinemachine;
using System.Linq;
using UnityEngine;

public class SquadComponent : MonoBehaviour
{
    [Header("Squad Settings")]
    [SerializeField] private SquadMember[] squadMembers;
    [SerializeField] private float _speed = 5f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    private Joystick _joystick;
    private FormationComponent _formation;

    private SquadMember openSquadMember;

    private void Awake()
    {
        _joystick = FindObjectOfType<Joystick>();
        _formation = FindObjectOfType<FormationComponent>();
        cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();

        squadMembers = GetComponentsInChildren<SquadMember>();

        _formation.RecreateFormation(Vector3.zero, 1f, squadMembers.Length - 1);
        SetMembersToCinemachineGroup();
    }

    private void Update()
    {
         Movement();
         Shoot();
    }

    private void SetMembersToCinemachineGroup()
    {
        foreach (var oldTarget in cinemachineTargetGroup.m_Targets)
        {
            cinemachineTargetGroup.RemoveMember(oldTarget.target);
        }

        foreach (var member in squadMembers)
        {
            cinemachineTargetGroup.AddMember(member.transform, 1, 5);
        }
    }
    private void Shoot()
    {
        foreach (var member in squadMembers)
        {
            member.currentTarget = member.overLapEnemy.NearestTouchedOfType<EnemyComponent>(member.transform);

            if (member.currentTarget != null)
            {
                member.Shoot(member.currentTarget.transform);
            }
        }
    }

    private void Movement()
    {
        SetNotBlockedMember();

        var swift = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized * _speed;

        if (swift.magnitude < 0.1f)
        {
            foreach (var member in squadMembers)
            {
                member.animancer.Layers[0].Play(member.animations.Idle);

                member.navMesh.isStopped = true;
                member.navMesh.ResetPath();
            }

            return;
        }

        int memberIndex = -1;

        foreach (var member in squadMembers)
        {
            memberIndex++;

            member.animancer.Layers[0].Play(member.animations.RunForward);
            member.navMesh.isStopped = false;

            Transform formationPoint = _formation.FormationPoints[memberIndex];

            if (member.Equals(openSquadMember))
            {
                _formation.transform.position = member.transform.position - formationPoint.transform.localPosition;
            }

            if (Vector3.Distance(member.transform.position, formationPoint.position) > .5f)
            {
                member.navMesh.SetDestination(formationPoint.position);

                continue;
            }

            member.navMesh.ResetPath();
            
            member.navMesh.Move(swift * Time.deltaTime);

            if (member.currentTarget == null)
            {
                member.targetVector = member.transform.position + swift;
            }
            else
            {
                member.targetVector = member.currentTarget.transform.position;
            }
        }
    }

    private void SetNotBlockedMember()
    {
        if (openSquadMember == null || openSquadMember.overLapObstacle.HasTouch)
        {
            openSquadMember = squadMembers.DefaultIfEmpty(squadMembers[0]).FirstOrDefault(x => !x.overLapObstacle.HasTouch);
        }
    }
}