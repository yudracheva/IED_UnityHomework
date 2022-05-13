using System;
using System.Collections.Generic;
using ConstantsValue;
using Enemies.Spawn;
using Hero;
using Services.Assets;
using Services.Bonuses;
using Services.Hero;
using Services.Input;
using Services.Progress;
using Services.StaticData;
using Services.UI.Buttons;
using Services.UI.Windows;
using Services.UserSetting;
using StaticData.Level;
using Systems.Healths;
using UI.Displaying;
using UnityEngine;

namespace Services.Factories.GameFactories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IBonusSpawner _bonusSpawner;
        private readonly IHeroDeathService _deathService;
        private readonly IEnemySpawner _enemySpawner;
        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IWindowsService _windowsService;
        private readonly IUserSettingService _userSettingService;
        private GameObject heroGameObject;

        public GameFactory(
            IAssetProvider assets,
            IStaticDataService staticData,
            IInputService inputService,
            IEnemySpawner enemySpawner,
            IWindowsService windowsService,
            IPersistentProgressService progressService,
            IBonusSpawner bonusSpawner,
            IHeroDeathService deathService,
            IUserSettingService userSettingService)
        {
            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
            _staticData = staticData ?? throw new ArgumentNullException(nameof(staticData));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _windowsService = windowsService ?? throw new ArgumentNullException(nameof(windowsService));
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
            _bonusSpawner = bonusSpawner ?? throw new ArgumentNullException(nameof(bonusSpawner));
            _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
            _deathService = deathService ?? throw new ArgumentNullException(nameof(deathService));
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
        }

        public GameObject CreateHero()
        {
            var spawnData = _staticData.ForHero();
            heroGameObject = InstantiateObject(spawnData.HeroPrefab, spawnData.SpawnPoint);

            _progressService.SetPlayerToDefault();
            var health = heroGameObject.GetComponentInChildren<IHealth>();
            health.SetHp(_progressService.Player.Characteristics.Health(),
                _progressService.Player.Characteristics.Health());

            heroGameObject.GetComponent<HeroInput>().Construct(_inputService);
            heroGameObject.GetComponent<HeroStateMachine>().Construct(
                _progressService.Player.AttackData,
                _progressService.Player.ImpactsData,
                _progressService.Player.Characteristics,
                _userSettingService);

            heroGameObject.GetComponentInChildren<HeroStamina>().Construct(_progressService.Player.StaminaStaticData,
                _progressService.Player.Characteristics);

            heroGameObject.GetComponent<HeroMoney>().Construct(_progressService.Player.Monies);
            heroGameObject.GetComponent<HeroInventory>().Construct(_progressService.Player.Inventory);
            heroGameObject.GetComponent<HeroNumberOfWaves>().Construct(_progressService.Player.NumberOfWaves);
            heroGameObject.GetComponent<HeroNumberOfKilledEnemies>().Construct(_progressService.Player.NumberOfKilledEnemies);

            heroGameObject.GetComponent<HeroDeath>().Construct(_deathService, health);
            return heroGameObject;
        }

        public GameObject CreateHud(GameObject hero)
        {
            var hud = _assets.Instantiate<GameObject>(AssetsPath.Hud);
            hud.GetComponentInChildren<HPDisplayer>().Construct(hero.GetComponentInChildren<IHealth>());
            hud.GetComponentInChildren<StaminaDisplayer>().Construct(hero.GetComponentInChildren<IStamina>());
            hud.GetComponentInChildren<HeroMoneyDisplayer>().Construct(_progressService.Player.Monies);
            hud.GetComponentInChildren<HeroScoreDisplayer>().Construct(_progressService.Player.Score);
            hud.GetComponentInChildren<HeroWaveDisplayer>().Construct(_progressService.Player.NumberOfWaves);
            hud.GetComponentInChildren<HeroDeathDisplayer>().Construct(_progressService.Player.NumberOfKilledEnemies);
            InitButtons(hud);
            return hud;
        }

        public void CreateEnemySpawnPoints(List<SpawnPointStaticData> spawnPoints, SpawnPoint pointPrefab)
        {
            _enemySpawner.Cleanup();
            for (var i = 0; i < spawnPoints.Count; i++)
                _enemySpawner.AddPoint(CreateEnemySpawnPoint(spawnPoints[i], pointPrefab));
        }

        public void CreateBonusSpawnPoints(List<SpawnPointStaticData> spawnPoints, SpawnPoint pointPrefab)
        {
            _bonusSpawner.Cleanup();
            for (var i = 0; i < spawnPoints.Count; i++)
                _bonusSpawner.AddPoint(CreateEnemySpawnPoint(spawnPoints[i], pointPrefab));
        }

        private void InitButtons(GameObject hud)
        {
            var buttons = hud.GetComponentsInChildren<OpenWindowButton>(true);
            for (var i = 0; i < buttons.Length; i++) buttons[i].Construct(_windowsService);
        }

        private SpawnPoint CreateEnemySpawnPoint(SpawnPointStaticData data, SpawnPoint prefab)
        {
            var spawner = _assets.Instantiate(prefab, data.Position).GetComponent<SpawnPoint>();
            spawner.Construct(data.Id);
            return spawner;
        }

        private GameObject InstantiateObject(GameObject prefab, Vector3 at)
        {
            return _assets.Instantiate(prefab, at);
        }
    }
}