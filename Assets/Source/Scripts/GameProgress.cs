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

    public Action OnLevelUp;

    public float GetValueForFinish() => totalXP / needToFinish;
    public float GetValueForLevelUP() => XPforLevelUp / xpToLevelUps[level];

    private void Awake()
    {
        _levelSO = levels[0];
        _gameProgress = this;

        xpPicker = Find<XPPicker>();

        for (int i = 0; i < LevelSO.LevelUps; i++)
        {
            var xpToLevelUp = _levelSO.BaseXPToLevelUp + (i * _levelSO.StepXPOnLevelUp);
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

            OnLevelUp?.Invoke();
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