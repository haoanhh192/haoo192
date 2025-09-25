using D2D;
using D2D.Core;
using D2D.Utilities;
using NaughtyAttributes;
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
    [SerializeField] private MemberUpgrades baseUpgrade;

    [SerializeField] private float rareChance = 10f;
    [SerializeField] private float mediumChance = 30f;

    [SerializeField] private StatsUpgrades[] statsUpgrades;

    [Header("Debug Upgrades")]
    [SerializeField] private bool isDebug = false;
    [SerializeField, ShowIf("isDebug")] private MemberUpgrades[] debugUpgrades;

    private bool createdStatsUpgrade = false;

    private UpgradeUI upgradeUI;

    private List<MemberUpgrades> m_AvailableMemberUpgrades = new();

    private void Awake()
    {
        upgradeUI = Find<UpgradeUI>();

        _upgradesHandle = this;

        _gameProgress.OnLevelUp += OnLevelUp;

        if (_db.UnlockedMembers.IsNullOrEmpty())
        {
            foreach (var member in commonMemberUpgrades)
            {
                _db.UnlockedMembers.Add(member.name);

                _db.SaveMembers();
            }
        }

        if (isDebug)
        {
            m_AvailableMemberUpgrades = new(debugUpgrades);

            return;
        }

        var allElements = rareMemberUpgrades.Concat(commonMemberUpgrades).Concat(mediumMemberUpgrades);

        foreach (var item in _db.UnlockedMembers)
        {
            m_AvailableMemberUpgrades.Add(allElements.First(x => x.name == item));
        }
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
            List<MemberUpgrades> availableMemberUpgrades = GetRandomAvailableMembers();

            if (availableMemberUpgrades.Count > 1)
            {
                memberUpgrade = availableMemberUpgrades.Except(usedMemberUpgrade).ToArray().Random();
            }
            else
            {
                memberUpgrade = availableMemberUpgrades.ToArray().Random();
            }

            buttons[index].UpgradeButton.onClick.AddListener(() => Upgrade(memberUpgrade));
            buttons[index].InitButtonUI(memberUpgrade.Icon, memberUpgrade.UpgradeText);

            usedMemberUpgrade.Add(memberUpgrade);
        }
    }

    private List<MemberUpgrades> GetRandomAvailableMembers()
    {
        MembersComparer membersComparer = new MembersComparer();
        List<MemberUpgrades> availableMemberUpgrades = new();

        if (Random.Range(0, 100) < rareChance)
        {
            availableMemberUpgrades = rareMemberUpgrades.Intersect(m_AvailableMemberUpgrades, membersComparer).ToList();

            if (availableMemberUpgrades.Count > 0)
            {
                return availableMemberUpgrades;
            }
        }

        if (availableMemberUpgrades.Count == 0 && Random.Range(0, 100) < mediumChance)
        {
            availableMemberUpgrades = mediumMemberUpgrades.Intersect(m_AvailableMemberUpgrades, membersComparer).ToList();

            if (availableMemberUpgrades.Count > 0)
            {
                return availableMemberUpgrades;
            }
        }

        if (availableMemberUpgrades.Count == 0)
        {
            availableMemberUpgrades = commonMemberUpgrades.Intersect(m_AvailableMemberUpgrades, membersComparer).ToList();
        }


        if (availableMemberUpgrades.Count == 0)
        {
            availableMemberUpgrades = new List<MemberUpgrades>() 
            { baseUpgrade };
        }

        return availableMemberUpgrades;
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

public class MembersComparer : IEqualityComparer<MemberUpgrades>
{
    public bool Equals(MemberUpgrades x, MemberUpgrades y)
    {
        return x.UpgradeText == y.UpgradeText;
    }

    public int GetHashCode(MemberUpgrades obj)
    {
        //Check whether the object is null
        if (ReferenceEquals(obj, null)) return 0;

        return obj.name.GetHashCode();
    }
}