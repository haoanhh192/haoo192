using UnityEngine;

public enum UnlockableType
{
    Member = 1
}

public class UnlockableItem : ScriptableObject
{
    [SerializeField] private UnlockableType unlockableType = UnlockableType.Member;
    
    [Header("Visual")]
    [SerializeField] private Sprite icon;

    public Sprite Icon => icon;
    public UnlockableType UnlockableType => unlockableType;
}