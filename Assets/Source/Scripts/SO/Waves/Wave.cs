using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave Settings")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float duration;

    public GameObject[] Enemies => enemies;
    public float Duration => duration;
}