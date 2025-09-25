using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level")]
public class LevelSO : ScriptableObject
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private float totalDuration = 300f;

    public Wave[] Waves => waves;
    public float TotalDuration => totalDuration;
}