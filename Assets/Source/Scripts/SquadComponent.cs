using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class SquadComponent : MonoBehaviour
{
    [Header("Squad Settings")]
    [SerializeField] private List<SquadMember> squadMembers;
    [SerializeField] private float _speed = 5f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    [Header("Debug")]
    [SerializeField] private GameObject pistolMemberPrefab;
    [SerializeField] private GameObject grenadeLauncherPrefab;
    [SerializeField] private GameObject flamethrowerPrefab;

    private Joystick _joystick;
    private FormationComponent _formation;

    private SquadMember openSquadMember;

    private void Awake()
    {
        _squad = this;

        _joystick = FindObjectOfType<Joystick>();
        _formation = FindObjectOfType<FormationComponent>();
        cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();

        squadMembers = GetComponentsInChildren<SquadMember>().ToList();

        foreach (var member in squadMembers)
        {
            member.health.Died += () => MemberDie(member);
        }

        _formation.RecreateFormation(Vector3.zero, 1f, squadMembers.Count - 1);
        SetMembersToCinemachineGroup();
    }

    private void MemberDie(SquadMember member)
    {
        if (squadMembers.Contains(member))
        {
            squadMembers.Remove(member);

            _formation.RecreateFormation(Vector3.zero, 1f, squadMembers.Count - 1);
            SetMembersToCinemachineGroup();
        }
    }

    private void Update()
    {
         Movement();
         Shoot();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var newMember = Instantiate(pistolMemberPrefab, squadMembers[0].transform.position, Quaternion.identity, transform).GetComponent<SquadMember>();
            AddMember(newMember);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var newMember = Instantiate(grenadeLauncherPrefab, squadMembers[0].transform.position, Quaternion.identity, transform).GetComponent<SquadMember>();
            AddMember(newMember);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var newMember = Instantiate(flamethrowerPrefab, squadMembers[0].transform.position, Quaternion.identity, transform).GetComponent<SquadMember>();
            AddMember(newMember);
        }
    }

    public void AddMember(SquadMember member)
    {
        if (member != null && !squadMembers.Contains(member))
        {
            squadMembers.Add(member);
            _formation.RecreateFormation(squadMembers[0].transform.position, 1f, squadMembers.Count - 1);
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