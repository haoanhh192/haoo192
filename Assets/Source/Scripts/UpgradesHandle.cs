using D2D;
using D2D.Core;
using SRF;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class UpgradesHandle : Unit
{
    [SerializeField] private MemberUpgrades[] rareMemberUpgrades;
    [SerializeField] private MemberUpgrades[] mediumMemberUpgrades;
    [SerializeField] private MemberUpgrades[] commonMemberUpgrades;

    [SerializeField] private float rareChance = 10f;
    [SerializeField] private float mediumChance = 30f;

    [SerializeField] private StatsUpgrades[] statsUpgrades;

    private bool createdStatsUpgrade = false;

    private UpgradeUI upgradeUI;

    private void Awake()
    {
        upgradeUI = Find<UpgradeUI>();

        _upgradesHandle = this;

        _gameProgress.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp(int level)
    {
        if (_stateMachine.Last.Is<LoseState>() || _stateMachine.Last.Is<WinState>())
        {
            return;
        }

        ShowUpgradeUI();
    }
    private void ShowUpgradeUI()
    {
        upgradeUI.ShowUI();

        var buttons = upgradeUI.GetButtons();

        createdStatsUpgrade = false;
        List<MemberUpgrades> usedMemberUpgrade = new List<MemberUpgrades>();

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

            MemberUpgrades memberUpgrade;
            List<MemberUpgrades> availableMemberUpgrades = new List<MemberUpgrades>();

            if (Random.Range(0, 100) < rareChance)
            {
                availableMemberUpgrades = rareMemberUpgrades.ToList();
            }
            else if (Random.Range(0, 100) < mediumChance) 
            {
                availableMemberUpgrades = mediumMemberUpgrades.ToList();
            }
            else
            {
                availableMemberUpgrades = commonMemberUpgrades.ToList();
            }

            memberUpgrade = availableMemberUpgrades.Except(usedMemberUpgrade).ToArray().Random();

            buttons[index].UpgradeButton.onClick.AddListener(() => Upgrade(memberUpgrade));
            buttons[index].InitButtonUI(memberUpgrade.Icon, memberUpgrade.UpgradeText);

            usedMemberUpgrade.Add(memberUpgrade);
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

        _audioManager.PlayOneShot(_gameData.spawnClip, 0.4f);
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