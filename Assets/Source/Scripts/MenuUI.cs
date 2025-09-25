using D2D.Core;
using D2D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static D2D.Utilities.CommonGameplayFacade;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private MenuUIButton fireRateIncreaseButton;
    [SerializeField] private MenuUIButton firePowerIncreaseButton;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        fireRateIncreaseButton.Button.onClick.AddListener(IncreaseFireRate);
        firePowerIncreaseButton.Button.onClick.AddListener(IncreasePowerUp);

        continueButton.onClick.AddListener(() => _stateMachine.Push(new RunningState()));

        UpdateStats();
    }
    private void IncreasePowerUp()
    {
        _db.PowerIncreaseLevel.Value++;

        _db.Money.Value -= _db.PowerIncreaseLevel.Value * _gameData.baseUpgradePrice;

        UpdateStats();
    }
    private void IncreaseFireRate()
    {
        _db.FireRateDecreaseLevel.Value++;

        _db.Money.Value -= _db.FireRateDecreaseLevel.Value * _gameData.baseUpgradePrice;
        UpdateStats();
    }
    
    private void CheckForDeactivatingButtons()
    {
        if (_db.FireRateDecreaseLevel.Value >= _gameData.maxLevelUpgrade)
        {
            fireRateIncreaseButton.Button.interactable = false;
        }

        bool isEnoughForRate = _db.Money.Value >= (_db.FireRateDecreaseLevel.Value + 1) * _gameData.baseUpgradePrice;
        bool isEnoughForPower = _db.Money.Value >= (_db.PowerIncreaseLevel.Value + 1) * _gameData.baseUpgradePrice;
        
        fireRateIncreaseButton.Button.interactable = _gameData.upgradesPercentByLevel.Length <= _db.FireRateDecreaseLevel.Value ? false : isEnoughForRate;
        firePowerIncreaseButton.Button.interactable = _gameData.upgradesPercentByLevel.Length <= _db.FireRateDecreaseLevel.Value ? false : isEnoughForPower;
    }
    private void UpdateStats() 
    {
        _db.PowerIncreasePercent.Value = _gameData.upgradesPercentByLevel[(int) _db.PowerIncreaseLevel.Value] / 100;
        _db.FireRateDecreasePercent.Value = _gameData.upgradesPercentByLevel[(int)_db.FireRateDecreaseLevel.Value] / 100;

        firePowerIncreaseButton.LevelText.text = $"LEVEL {_db.PowerIncreaseLevel.Value}";
        fireRateIncreaseButton.LevelText.text = $"LEVEL {_db.FireRateDecreaseLevel.Value}";
        firePowerIncreaseButton.PriceText.text = $"{(_db.PowerIncreaseLevel.Value + 1) * _gameData.baseUpgradePrice} <sprite=0>";
        fireRateIncreaseButton.PriceText.text = $"{(_db.FireRateDecreaseLevel.Value + 1) * _gameData.baseUpgradePrice} <sprite=0>";

        CheckForDeactivatingButtons();
    }
}