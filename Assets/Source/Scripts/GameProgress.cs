using D2D;
using D2D.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class GameProgress : Unit
{
    [SerializeField] private LevelSO[] levels;

    private float XPforLevelUp;
    private float totalXP;

    private float needToFinish;

    private int level = 0;

    private Dictionary<int, float> xpToLevelUps = new();

    private XPPicker xpPicker;

    public Action<int> OnLevelUp;

    public float GetValueForFinish() => totalXP / needToFinish;
    public float GetValueForLevelUP() => XPforLevelUp / xpToLevelUps[level];

    private void Awake()
    {
        _levelSO = levels[0];
        _gameProgress = this;

        xpPicker = Find<XPPicker>();
        
        var multiplier = Mathf.Pow(_gameData.baseSpeedMultiplier, _db.PassedLevels.Value);

        for (int i = 0; i < LevelSO.LevelUps; i++)
        {
            var xpToLevelUp = _levelSO.BaseXPToLevelUp * multiplier + (i * _levelSO.StepXPOnLevelUp * multiplier);
            needToFinish += xpToLevelUp;

            xpToLevelUps.Add(i, xpToLevelUp);
        }

        xpPicker.OnPickUp += CheckForFinish;
        xpPicker.OnPickUp += CheckForLevelUp;
    }
    private void CheckForLevelUp(float xp)
    {
        if (level + 1 >= LevelSO.LevelUps)
        {
            return;
        }

        XPforLevelUp += xp;

        if (xpToLevelUps[level] <= XPforLevelUp)
        {
            XPforLevelUp = 0;
            level++;

            _audioManager.PlayOneShot(_gameData.spawnClip, 0.4f);

            OnLevelUp?.Invoke(level);
        }
    }
    private void CheckForFinish(float xp)
    {
        totalXP += xp;

        if (totalXP >= needToFinish)
        {
            _stateMachine.Push(new WinState());
        }
    }
}