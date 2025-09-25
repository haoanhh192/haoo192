using System;
using System.Collections.Generic;
/*using AppsFlyerSDK;*/
using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D.Utilities;
/*using Facebook.Unity;*/
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class Analytics : GameStateMachineUser
    {
        /*private const string LevelCountKey = "LevelCount"; 
            
        private int SceneNumber => _level.SceneNumber;
        public bool IsBootScene => !IsLevelScene;

        private bool IsLevelScene => _level != null;
        private int LevelNumber => _db.PassedLevels.Value + 1;

        private DataContainer<int> CompletedLevelsCount
            = new DataContainer<int>("CompletedLevelsCount", 0);

        private Dictionary<string, object> DefaultData => 
            new Dictionary<string, object>
            {
                {"level_number", LevelNumber.ToString()}, 
                {"level_name", LevelNumber.ToString()},
                {"level_count", CompletedLevelsCount.Value.ToString()}, 
                {"level_diff", "normal"},
                {"level_loop", LevelNumber.ToString()}, 
                {"level_random", "1"}, 
                {"level_type", "normal"},
                {"game_mode", "classic"}
            };

        private float _timeSinceStart;

        private Level _level;

        private void Start()
        {
            _level = Find<Level>();
            _timeSinceStart = Time.time;
            
            if (IsBootScene)
            {
                if (!FB.IsInitialized)
                    FB.Init(InitCallback, OnHideUnity);
                else
                    FB.ActivateApp();
                
                AppsFlyer.initSDK("r9vNC83N8nYpCzYGigyjUh", "");
                AppsFlyer.startSDK();
                
                OnAppOpen();
            }
            else
            {
                SendDataToYandex(DefaultData, "level_start");
            }
        }

        private void OnAppOpen()
        {
            CompletedLevelsCount.Value = 0;
        }

        protected override void OnGameFinish()
        {
            SendLevelFinishDataToYandex(isLeave: false);
        }

        private void OnApplicationQuit()
        {
            if (IsLevelScene)
            {
                SendLevelFinishDataToYandex(isLeave: true);
            }
        }

        private void SendLevelFinishDataToYandex(bool isLeave)
        {
            var result = _stateMachine.Was<WinState>() ? "win" : "lose";
            
            var progress = _stateMachine.Was<WinState>() ? "100" : "0";
            if (isLeave)
                progress = "leave";
            
            var time = Time.time - _timeSinceStart;
            
            var data = DefaultData;
            data.Add("result", result);
            data.Add("time", time.Round().ToString());
            data.Add("progress", progress);

            SendDataToYandex(data, "level_finish");
        }

        private void SendDataToYandex(Dictionary<string, object> data, string eventName)
        {
            AppMetrica.Instance.ReportEvent(eventName, data);
            AppMetrica.Instance.SendEventsBuffer();
        }

        private void InitCallback()
        {
            if (FB.IsInitialized) 
            {
                FB.ActivateApp();
            }
            else 
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            Time.timeScale = isGameShown ? 1 : 0;
        }

        protected override void OnGameWin()
        {
            // The player wins
        }

        protected override void OnGameLose()
        {
            // The player loses
        }*/
    }
}