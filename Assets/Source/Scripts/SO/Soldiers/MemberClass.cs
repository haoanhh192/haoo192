using UnityEngine;

[CreateAssetMenu(menuName = "Game/Member Class")]
public class MemberClass : ScriptableObject
{
    public float baseReloadDuration;
    public float baseDamage;
    public LayerMask obstacleLayer;
    public LayerMask enemyLayer;
}