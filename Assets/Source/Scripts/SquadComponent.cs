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
            member.Init();
            member.animancer.Layers[0].Play(member.animations.Idle);
            member.health.Died += () => MemberDie(member);
        }

        _stateMachine.On<WinState>(SqaudIdle);
        var squadMembersCount = squadMembers.Count;

        _formation.RecreateFormation(Vector3.zero, squadMembersCount <= 4 ? _formationRadius - .3f : _formationRadius, squadMembersCount - 1);
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
    private void SqaudIdle()
    {
        foreach (var member in squadMembers)
        {
            member.animancer.Layers[0].Play(member.animations.Idle);
        }
    }
    protected override void OnGameRun()
    {
        _joystick = FindObjectOfType<Joystick>();
    }
    public void AddMember(SquadMember member)
    {
        if (member != null && !squadMembers.Contains(member))
        {
            member.Init();
            squadMembers.Add(member);

            var squadMembersCount = squadMembers.Count;
            _formation.RecreateFormation(squadMembers[0].transform.position, squadMembersCount <= 4 ? _formationRadius - .3f : _formationRadius, squadMembersCount - 1);
            member.health.Died += () => MemberDie(member);
            SetMembersToCinemachineGroup();
            PunchScaleSquad();
        }
    }
    private void PunchScaleSquad()
    {
        for (int i = squadMembers.Count - 1; i >= 0; i--)
        {
            squadMembers[i].PunchScaleWithDelay(_gameData.punchDelay * i);
        }

        var vfx = Instantiate(_gameData.levelUpVFX, squadMembers[0].transform.position, Quaternion.LookRotation(Vector3.up));
        Destroy(vfx, 2f);
    }
    public void IncreaseFireRate(float value)
    {
        temporaryFireRateIncrease += value;
        PunchScaleSquad();
    }
    public void IncreaseFirePower(float value)
    {
        temporaryFirePowerIncrease += value;
        PunchScaleSquad();
    }
    public void HealSquad(float value)
    {
        foreach (var member in squadMembers)
        {
            member.health.Heal(member.health.MaxPoints * (value / 100));
        }
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

            var squadMembersCount = squadMembers.Count;
            _formation.RecreateFormation(Vector3.zero, squadMembersCount <= 4 ? _formationRadius - .3f : _formationRadius, squadMembersCount - 1);
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
                var newTargetPos = member.currentTarget.transform.position;
                newTargetPos.y = member.transform.position.y;
                member.targetVector = newTargetPos;

                member.Shoot(member.currentTarget.transform);
            }
        }
    }
    private void Movement()
    {
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
                    member.animancer.Layers[0].Play(member.runForward);
                    member.navMesh.updateRotation = true;
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

            member.animancer.Layers[0].Play(member.runForward);
            member.navMesh.isStopped = false;

            Transform formationPoint = _formation.FormationPoints[memberIndex];
            
            if (memberIndex == 0)
            {
                member.navMesh.Move(swift * Time.deltaTime);
                member.navMesh.ResetPath();
                _formation.transform.position = member.transform.position;
            }
            else
            {
                member.navMesh.SetDestination(formationPoint.position);
                member.navMesh.updateRotation = false;

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