using System;
using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D.Utils;
using UnityEngine;

namespace D2D.Utilities
{
    /// <summary>
    /// Make a live template and go on...
    /// </summary>
    public static class CommonGameplayFacade
    {
        /// <summary>
        /// Or any player later initialization with on spawn setup.
        /// </summary>
        public static Player _player
        {
            get
            {
                if (__player == null)
                    __player = GameObject.FindObjectOfType<Player>();

                // if (__player == null)
                   // throw new Exception("Cant find player from facade!");

                return __player;
            }
        }
        private static Player __player;
        
        public static Camera _mainCamera
        {
            get
            {
                if (__camera == null)
                    __camera = GameObject.FindObjectOfType<MainCameraMark>().Get<Camera>();

                return __camera;
            }
        }
        private static Camera __camera;

        
        public static Level _level
        {
            get
            {
                if (__level == null)
                    __level = GameObject.FindObjectOfType<Level>();
                
                // if (__level == null)
                   // throw new Exception("Cant find level from facade!");

                return __level;
            }
        }
        private static Level __level;

        public static SquadComponent _squad
        {
            get
            {
                if (__squad == null)
                    __squad = GameObject.FindObjectOfType<SquadComponent>();

                return __squad;
            }
            set
            {
                __squad = value;
            }
        }
        private static SquadComponent __squad;
        public static FormationComponent _formation
        {
            get
            {
                if (__formation == null)
                    __formation = GameObject.FindObjectOfType<FormationComponent>();

                return __formation;
            }
            set
            {
                __formation = value;
            }
        }
        private static FormationComponent __formation;
        public static EnemySpawn _enemySpawn
        {
            get
            {
                if (__enemySpawn == null)
                    __enemySpawn = GameObject.FindObjectOfType<EnemySpawn>();

                return __enemySpawn;
            }
            set
            {
                __enemySpawn = value;
            }
        }
        private static EnemySpawn __enemySpawn;

        public static FlyingUISpawner _flyingSpawner
        {
            get
            {
                if (__flyingSpawner == null)
                    __flyingSpawner = GameObject.FindObjectOfType<FlyingUISpawner>();
                
                // if (__level == null)
                // throw new Exception("Cant find level from facade!");

                return __flyingSpawner;
            }
        }
        private static FlyingUISpawner __flyingSpawner;
        
        
        
        public static GameplaySettings _gameData => GameplaySettings.Instance;
        
        public static CoreSettings _coreData => CoreSettings.Instance;
        
        public static HapticSettings _hapticData => HapticSettings.Instance;
        
        
        public static SceneLoader _sceneLoader
        {
            get
            {
                if (__sceneLoader == null)
                    __sceneLoader = LazySugar.FindLazy<SceneLoader>();

                return __sceneLoader;
            }
        }
        private static SceneLoader __sceneLoader;
        
        public static GameProgressionDatabase _db
        {
            get
            {
                if (__db == null)
                    __db = LazySugar.FindLazy<GameProgressionDatabase>();

                return __db;
            }
        }
        private static GameProgressionDatabase __db;
        
        public static GameStateMachine _stateMachine
        {
            get
            {
                if (__stateMachine == null)
                    __stateMachine = LazySugar.FindLazy<GameStateMachine>();

                return __stateMachine;
            }
        }
        private static GameStateMachine __stateMachine;
        
        public static RoutineResolver _routineResolver
        {
            get
            {
                if (__routineResolver == null)
                    __routineResolver = LazySugar.FindLazy<RoutineResolver>();

                return __routineResolver;
            }
        }
        private static RoutineResolver __routineResolver;
    }
}