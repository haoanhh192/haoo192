using Cinemachine;
using D2D.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class SquadComponent : GameStateMachineUser
{
    [Header("Squad Settings")]
    [SerializeField] private List<SquadMember> squadMembers;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _formationRadius = 1.5f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    [Header("Debug")]
    [SerializeField] private GameObject pistolMemberPrefab;
    [SerializeField] private GameObject grenadeLauncherPrefab;
    [SerializeField] private GameObject flamethrowerPrefab;

    private Joystick _joystick;
    private FormationComponent _formation;

    private SquadMember openSquadMember;

    private float temporaryFireRateIncrease;
    private float temporaryFirePowerIncrease;

    public SquadMember FirstSquadMember => squadMembers[0];

    public float TemporaryFireRateIncrease => temporaryFireRateIncrease / 100;
    public float TemporaryFirePowerIncrease => temporaryFirePowerIncrease / 100;

    private void Awake()
    {
        _squad = this;

        _formation = FindObjectOfType<FormationComponent>();
        cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();

        squadMembers = GetComponentsInChildren<SquadMember>().ToList();

        foreach (var member in squadMembers)
        {
            member.animancer.Layers[0].Play(member.animations.Idle);
            member.health.Died += () => MemberDie(member);
        }

        _formation.RecreateFormation(Vector3.zero, _formationRadius, squadMembers.Count - 1);
        SetMembersToCinemachineGroup();
    }
    private void Update()
    {
        if (!_stateMachine.Last.Is<RunningState>())
        {
            return;
        }

         Movement();
         Shoot();
    }
    protected override void OnGameRun()
    {
        _joystick = FindObjectOfType<Joystick>();
    }
    public void AddMember(SquadMember member)
    {
        if (member != null && !squadMembers.Contains(member))
        {
            squadMembers.Add(member);
            _formation.RecreateFormation(squadMembers[0].transform.position, _formationRadius, squadMembers.Count - 1);
            member.health.Died += () => MemberDie(member);
            SetMembersToCinemachineGroup();
        }
    }
    public void IncreaseFireRate(float value)
    {
        temporaryFireRateIncrease += value;
    }
    public void IncreaseFirePower(float value)
    {
        temporaryFirePowerIncrease += value;
    }

    private void MemberDie(SquadMember member)
    {
        if (squadMembers.Contains(member))
        {
            squadMembers.Remove(member);

            if (squadMembers.Count <= 0)
            {
                _stateMachine.Push(new LoseState());

                return;
            }

            _formation.RecreateFormation(Vector3.zero, _formationRadius, squadMembers.Count - 1);
            SetMembersToCinemachineGroup();
        }
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
                member.targetVector = member.currentTarget.transform.position;
                member.Shoot(member.currentTarget.transform);
            }
        }
    }
    private void Movement()
    {
        SetNotBlockedMember();

        var swift = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized * _speed;

        if (swift.magnitude < .1f)
        {
            int index = -1;

            foreach (var member in squadMembers)
            {
                index++;

                if (index == 0)
                {
                    member.navMesh.ResetPath();
                    member.animancer.Layers[0].Play(member.animations.Idle);
                    continue;
                }

                Transform formationPoint = _formation.FormationPoints[index];
                
                if (Vector3.Distance(formationPoint.position, member.navMesh.transform.position) > .1f)
                {
                    member.animancer.Layers[0].Play(member.animations.RunForward);
                    member.navMesh.SetDestination(formationPoint.position);
                }
                else
                {
                    member.animancer.Layers[0].Play(member.animations.Idle);
                }

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
            
            if (memberIndex == 0)
            {
                member.navMesh.Move(swift * Time.deltaTime);
                _formation.transform.position = member.transform.position;
            }
            else
            {
                member.navMesh.SetDestination(formationPoint.position);

                if (Vector3.Distance(member.transform.position, formationPoint.position) > 1f)
                {
                    member.navMesh.speed = _speed + 2;
                }
                else
                {
                    member.navMesh.speed = _speed;
                }
            }
            

            if (member.currentTarget == null)
            {
                member.targetVector = member.transform.position + swift;
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