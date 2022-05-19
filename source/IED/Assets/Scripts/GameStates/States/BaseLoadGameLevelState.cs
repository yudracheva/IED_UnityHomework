using System;
using System.Collections.Generic;
using Audio;
using Systems.Healths;
using CodeBase.CameraLogic;
using ConstantsValue;
using Enemies;
using Enemies.Spawn;
using Environment.DoorObject;
using GameStates.States.Interfaces;
using SceneLoading;
using Services.Assets;
using Services.Factories.GameFactories;
using Services.Factories.Loot;
using Services.Loot;
using Services.Progress;
using Services.Shop;
using Services.StaticData;
using Services.UI.Factory;
using Services.UserSetting;
using Services.Waves;
using StaticData.Level;
using UI.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameStates.States
{
    public class BaseLoadGameLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticData;
        private readonly IWaveServices _waveServices;
        private readonly ILootService _lootService;
        private readonly ILootSpawner _lootSpawner;
        private readonly IShopService _shopService;
        private readonly IUserSettingService _userSettingService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IAssetProvider _assets;
        
        private Button[] _buttons;
        
        protected BaseLoadGameLevelState(
            IPersistentProgressService persistentProgressService, 
            ISceneLoader sceneLoader,
            IGameStateMachine gameStateMachine,
            IGameFactory gameFactory,
            IUIFactory uiFactory,
            IStaticDataService staticData,
            IWaveServices waveServices,
            ILootService lootService,
            ILootSpawner lootSpawner,
            IShopService shopService,
            IUserSettingService userSettingService,
            IAssetProvider assets)
        {
            _persistentProgressService = persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gameFactory = gameFactory ?? throw new ArgumentNullException(nameof(gameFactory));
            _uiFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));
            _staticData = staticData ?? throw new ArgumentNullException(nameof(staticData));
            _waveServices = waveServices ?? throw new ArgumentNullException(nameof(waveServices));
            _lootService = lootService ?? throw new ArgumentNullException(nameof(lootService));
            _lootSpawner = lootSpawner ?? throw new ArgumentNullException(nameof(lootSpawner));
            _shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
        }

        public void Enter(string payload)
        {
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit()
        {
        }

        private protected virtual void OnLoaded()
        {
            InitGameWorld();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private protected void InitGameWorld(bool needToReset = true)
        {
            var uiRoot = InitUIRoot();
            InitAudio(uiRoot);
            
            var levelData = GetLevelData();
            InitEnemySpawners(levelData.EnemySpawners, levelData.SpawnPointPrefab);
            InitBonusSpawner(levelData.BonusSpawners, levelData.SpawnPointPrefab);
            InitWaves(levelData.LevelWaves);
            InitLootService(levelData.LevelKey);

            var hero = _gameFactory.CreateHero(needToReset);
            var hud = CreateHud(hero);

            CleanupLootSpawner();

            var camera = Camera.main;
            CameraFollow(hero, camera);
            SetCameraToHud(hud, camera);
            
            _shopService.InitSlots();

            if (levelData.NeedDoor)
                InitDoor();
        }
        
        private void InitDoor()
        {
            var doorStaticData = _staticData.GetDoor();
            var door = _assets.Instantiate(doorStaticData.DoorPrefab, doorStaticData.SpawnPoint, doorStaticData.SpawnRotation);
            door.GetComponent<DoorStateMachine>().Construct(_persistentProgressService);

            var nextLevelDoor = door.GetComponentInChildren<NextLevel>();
            nextLevelDoor.Construct(_gameStateMachine);
        }

        private void InitAudio(GameObject uiRoot)
        {
            _userSettingService.UpdateGameSettings();
            
            var buttonSong = uiRoot.GetComponentInChildren<AudioButton>();
            buttonSong.Construct(_userSettingService);

            var audio = _staticData.AudioForLevel(SceneManager.GetActiveScene().name);
            
            var audioBackground = uiRoot.GetComponentInChildren<AudioBackground>();
            audioBackground.Construct(_userSettingService, audio.AudioClip);
        }

        private LevelStaticData GetLevelData()
        {
            var sceneKey = SceneManager.GetActiveScene().name;
            return _staticData.ForLevel(sceneKey);
        }

        private void InitEnemySpawners(List<SpawnPointStaticData> enemySpawners, SpawnPoint pointPrefab) =>
            _gameFactory.CreateEnemySpawnPoints(enemySpawners, pointPrefab);

        private void InitBonusSpawner(List<SpawnPointStaticData> bonusSpawners, SpawnPoint pointPrefab) =>
            _gameFactory.CreateBonusSpawnPoints(bonusSpawners, pointPrefab);

        private void CleanupLootSpawner() =>
            _lootSpawner.Cleanup();

        private void InitWaves(LevelWaveStaticData waves) =>
            _waveServices.SetLevelWaves(waves);

        private GameObject CreateHud(GameObject hero) =>
            _gameFactory.CreateHud(hero);

        private GameObject InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private void InitLootService(string sceneName) =>
            _lootService.SetSceneName(sceneName);

        private void CameraFollow(GameObject hero, Camera camera) =>
            camera.GetComponent<CameraFollow>().Follow(hero);

        private void SetCameraToHud(GameObject hud, Camera camera) =>
            hud.GetComponent<Canvas>().worldCamera = camera;
    }
}