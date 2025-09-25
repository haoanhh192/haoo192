using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Upgrades/Member Upgrade")]
public class MemberUpgrades : Upgrades
{
    [SerializeField] private GameObject memberPrefab;
    [SerializeField] private Sprite silhouetteIcon;

    public GameObject MemberPrefab => memberPrefab;
    public Sprite SilhouetteIcon => silhouetteIcon;
}