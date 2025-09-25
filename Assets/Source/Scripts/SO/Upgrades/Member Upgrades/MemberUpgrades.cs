using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Upgrades/Member Upgrade")]
public class MemberUpgrades : Upgrades
{
    [SerializeField] private GameObject memberPrefab;

    public GameObject MemberPrefab => memberPrefab;
}