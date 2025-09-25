using System;
using System.Collections.Generic;
using D2D.Utilities;
using D2D.Utils;
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
    }
}