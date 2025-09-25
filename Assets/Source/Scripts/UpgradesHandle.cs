using D2D;
using SRF;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class UpgradesHandle : Unit
{
    [SerializeField] private MemberUpgrades[] memberUpgrades;
    [SerializeField] private StatsUpgrades[] statsUpgrades;

    private bool createdStatsUpgrade = false;

    private UpgradeUI upgradeUI;

    private void Awake()
    {
        upgradeUI = Find<UpgradeUI>();

        _upgradesHandle = this;

        _gameProgress.OnLevelUp += OnLevelUp;
    }
    
    private void OnLevelUp()
    {
        ShowUpgradeUI();
    }
    private void ShowUpgradeUI()
    {
        upgradeUI.ShowUI();

        var buttons = upgradeUI.GetButtons();

        createdStatsUpgrade = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            var index = i;

            buttons[index].UpgradeButton.onClick.RemoveAllListeners();

            if (!createdStatsUpgrade)
            {
                var statsUpgrade = statsUpgrades.Random();

                buttons[index].UpgradeButton.onClick.AddListener(() => Upgrade(statsUpgrade));
                buttons[index].InitButtonUI(statsUpgrade.Icon, statsUpgrade.UpgradeText);

                createdStatsUpgrade = true;

                continue;
            }

            var memberUpgrade = memberUpgrades.Random();

            buttons[index].UpgradeButton.onClick.AddListener(() => Upgrade(memberUpgrade));
            buttons[index].InitButtonUI(memberUpgrade.Icon, memberUpgrade.UpgradeText);
        }
    }

    private void Upgrade(Upgrades upgrades)
    {
        switch (upgrades.UpgradesType)
        {
            case UpgradesType.Member:

                UpgradeMember(upgrades as MemberUpgrades);
                break;

            case UpgradesType.Stats:

                UpgradeStats(upgrades as StatsUpgrades);
                break;
        }

        upgradeUI.HideUI();
    }
    private void UpgradeStats(StatsUpgrades upgrade)
    {
        switch (upgrade.StatsUpgradesType)
        {
            case StatsUpgradesType.AttackPower:

                _squad.IncreaseFirePower(upgrade.IncreasePercent);
                break;

            case StatsUpgradesType.AttackRate:

                _squad.IncreaseFireRate(upgrade.IncreasePercent);
                break;

            case StatsUpgradesType.Heal:

                _squad.HealSquad(upgrade.IncreasePercent);
                break;
        }
    }

    private void UpgradeMember(MemberUpgrades upgrade)
    {
        var newMember = Instantiate(upgrade.MemberPrefab, _squad.FirstSquadMember.transform.position, Quaternion.identity, _squad.transform).GetComponent<SquadMember>();
        _squad.AddMember(newMember);
    }
}