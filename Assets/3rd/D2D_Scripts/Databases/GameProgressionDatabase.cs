using System;
using D2D.Common;
using D2D.Utilities;
using D2D.Core;
using D2D.Gameplay;
using NaughtyAttributes;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Databases
{
    public class GameProgressionDatabase : GameStateMachineUser, ILazy
    {
        #region For analytics

        public readonly DataContainer<int> Reloads = 
            new DataContainer<int>("ReloadsCount", 0);
        
        public readonly DataContainer<int> CompletedLevelsPerSession = 
            new DataContainer<int>("CompletedLevelsPerSession", 0);

        #endregion

        public readonly DataContainer<int> PassedLevels = 
            new DataContainer<int>("PassedLevels", 0);

        public readonly DataContainer<float> PowerIncreasePercent =
            new DataContainer<float>("PowerIncreasePercent", 0);
        public readonly DataContainer<float> FireRateDecreasePercent =
            new DataContainer<float>("FireRateDecreasePercent", 0);

        public readonly DataContainer<float> PowerIncreaseLevel =
            new DataContainer<float>("PowerIncreaseLevel", 0);
        public readonly DataContainer<float> FireRateDecreaseLevel =
            new DataContainer<float>("FireRateDecreaseLevel", 0);

        public readonly TrackableValue<float> Money =
            new TrackableValue<float>(value: 0, firstGet: () => PlayerPrefs.GetInt("Money"));
        
        public readonly DataContainer<int> LastSceneNumber 
            = new DataContainer<int>("LastSceneNumber", 1);

        public float TimeOfSceneLoad { get; private set; }

        public float TimeElapsedFromSceneLoad => Time.time - TimeOfSceneLoad;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ResetLevelsPerSession()
        {
            LazySugar.FindLazy<GameProgressionDatabase>().CompletedLevelsPerSession.Value = 0;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Money.Changed += SaveMoney;
            TimeOfSceneLoad = Time.time;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Money.Changed -= SaveMoney;
        }

        private void SaveMoney(float val)
        {
            PlayerPrefs.SetInt("Money", Money.Value.Round());
        }

        private void OnApplicationQuit()
        {
            SaveMoney(Money.Value.Round());
        }

        private void Start()
        {
            var level = this.Find<Level>();
            if (level != null)
                LastSceneNumber.Value = level.SceneNumber;
        }
        
        protected override void OnGameWin()
        {
            CompletedLevelsPerSession.Value++;
            PassedLevels.Value++;
        }
        
        public static void Clear()
        {
            ES3.Save("ReloadsCount", 0);
            ES3.Save("CompletedLevelsPerSession", 0);
            ES3.Save("PassedLevels", 0);
            ES3.Save("LastSceneNumber", 1);
            ES3.Save("PassedLevels", 0);
            ES3.Save("LastSceneNumber", 1);

            PlayerPrefs.SetInt("Money", 0);
        }
    }
}

