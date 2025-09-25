using UnityEngine;

public enum UpgradesType
{
    Stats = 1,
    Member = 2
}

public class Upgrades : ScriptableObject
{
    [SerializeField] private UpgradesType upgradesType;
    [SerializeField] private Sprite icon;
    [SerializeField] private string upgradeText;

    public UpgradesType UpgradesType => upgradesType;
    public Sprite Icon => icon;
    public string UpgradeText => upgradeText;
}