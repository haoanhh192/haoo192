using System;
using System.Collections.Generic;
using D2D.Utilities;
using D2D.Utils;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D2D.Gameplay
{
    [Serializable]
    public class Sounds
    {
        public SoundPreset onWin;
    }
    
    /// <summary>
    /// Maybe it will be better to split to more specific settings.
    /// But if it is a small game prototype maybe it is more convenient to keep it all there. 
    /// </summary>
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "SO/GameplaySettings")]
    public class GameplaySettings : SingletonData<GameplaySettings>
    {
        [TabGroup("Common")] public List<GameObject> levels;

        [Header("Death")]
        [TabGroup("Other")] public Color grayDeathColor;
        [TabGroup("Other")] public float grayDeathDuration;
        [TabGroup("Other")] public float grayCorpseLifetime;
        [TabGroup("Other")] public GameObject muzzleFlash;
        [TabGroup("Other")] public PoolType bulletMuzzleFlash;
        [TabGroup("Other")] public float enemyDespawnDistance = 14f;
        [TabGroup("Other")] public float enemyMaxSpeedLevel = 10f;

        [Header("Layers")]
        [TabGroup("Layers")] public LayerMask GroundLayer;
        [TabGroup("Layers")] public LayerMask EnemyLayer;
        [TabGroup("Layers")][Layer] public string XPLayer;

        [Header("PickUP")]
        [TabGroup("PickUp")] public float timeBeforeXPActivate;
        [TabGroup("PickUp")] public float pickUpFlyForce;

        [Header("Upgrades")]
        [TabGroup("Upgrades")] public float baseUpgradePrice;
        [TabGroup("Upgrades")] public float[] upgradesPercentByLevel;
        [TabGroup("Upgrades")] public int maxLevelUpgrade;
        [TabGroup("Upgrades")] public int baseIncrease;
        [TabGroup("Upgrades")] public GameObject levelUpVFX;

        [Header("Level Up Tween")]
        [TabGroup("Level Up Tween")] public float punchScale;
        [TabGroup("Level Up Tween")] public float punchDuration;
        [TabGroup("Level Up Tween")] public float punchDelay;

        [Header("Sounds")]
        [TabGroup("Sounds")] public AudioClip pistolShotClip;
        [TabGroup("Sounds")] public AudioClip machineGunShotClip;
        [TabGroup("Sounds")] public AudioClip rifleShotClip;
        [TabGroup("Sounds")] public AudioClip shotgunShotClip;
        [TabGroup("Sounds")] public AudioClip explosionClip;
        [TabGroup("Sounds")] public AudioClip pickUpClip;
        [TabGroup("Sounds")] public AudioClip spawnClip;

        [Header("Enemies")]
        [TabGroup("Enemies")] public float baseSpeedMultiplier;
    }
}