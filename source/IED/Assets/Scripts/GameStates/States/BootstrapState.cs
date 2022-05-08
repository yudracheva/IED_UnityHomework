using Bootstrap;
using Enemies.Spawn;
using GameStates.States.Interfaces;
using Input;
using Loots;
using SceneLoading;
using Services;
using Services.Assets;
using Services.Bonuses;
using Services.Database;
using Services.Factories.Enemy;
using Services.Factories.GameFactories;
using Services.Factories.Loot;
using Services.Hero;
using Services.Input;
using Services.Loot;
using Services.Progress;
using Services.Random;
using Services.Score;
using Services.Shop;
using Services.StaticData;
using Services.UI.Factory;
using Services.UI.Windows;
using Services.UserSetting;
using Services.Waves;

namespace GameStates.States
{
    public class BootstrapState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly AllServices _services;

        public BootstrapState(
            IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            ref AllServices services,
            ICoroutineRunner coroutineRunner, 
            LootContainer lootContainer)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices(coroutineRunner, lootContainer);
        }

        public void Enter()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
        }

        private void RegisterServices(ICoroutineRunner coroutineRunner, LootContainer lootContainer)
        {
            RegisterUserSettings();
            RegisterStateMachine();
            RegisterInputService();
            RegisterRandom();
            RegisterStaticDataService();
            RegisterProgress();
            RegisterAssets();
            RegisterShopService();
            RegisterDatabaseService();
            RegisterEnemiesFactory();
            RegisterEnemiesSpawner();
            RegisterScoreService();
            RegisterUIFactory();
            RegisterWindowsService();
            RegisterHeroDeathService();
            RegisterBonusFactory();
            RegisterBonusSpawner();
            RegisterGameFactory();
            RegisterWaveService(coroutineRunner);
            RegisterLootSpawner();
            RegisterLootService(lootContainer);
        }

        private void RegisterUserSettings()=>
            _services.RegisterSingle<IUserSettingService>(new UserSettingService());

        private void RegisterWaveService(ICoroutineRunner coroutineRunner) =>
            _services.RegisterSingle<IWaveServices>(new WaveServices(_services.Single<IEnemySpawner>(), coroutineRunner,
                _services.Single<IBonusSpawner>()));

        private void RegisterEnemiesSpawner() =>
            _services.RegisterSingle<IEnemySpawner>(new EnemySpawner(_services.Single<IEnemiesFactory>(),
                _services.Single<IRandomService>()));

        private void RegisterEnemiesFactory() =>
            _services.RegisterSingle<IEnemiesFactory>(new EnemiesFactory(_services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>()));

        private void RegisterLootSpawner() =>
            _services.RegisterSingle<ILootSpawner>(new LootSpawner(_services.Single<IAssetProvider>()));

        private void RegisterLootService(LootContainer lootContainer) =>
            _services.RegisterSingle<ILootService>(new LootService(_services.Single<ILootSpawner>(),
                _services.Single<IRandomService>(), _services.Single<IStaticDataService>(),
                _services.Single<IEnemySpawner>(), lootContainer));

        private void RegisterGameFactory()
        {
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IInputService>(),
                    _services.Single<IEnemySpawner>(),
                    _services.Single<IWindowsService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IBonusSpawner>(),
                    _services.Single<IHeroDeathService>(),
                    _services.Single<IUserSettingService>()));
        }

        private void RegisterStateMachine() =>
            _services.RegisterSingle(_gameStateMachine);

        private void RegisterInputService()
        {
            IInputService inputService = new InputService(new HeroControls());
            _services.RegisterSingle<IInputService>(inputService);
        }

        private void RegisterAssets()
        {
            IAssetProvider provider = new AssetProvider();
            _services.RegisterSingle(provider);
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private void RegisterProgress() =>
            _services.RegisterSingle(
                new PersistentProgressService(_services.Single<IStaticDataService>().ForHeroCharacteristics()));

        private void RegisterRandom() =>
            _services.RegisterSingle(new RandomService());

        private void RegisterUIFactory() =>
            _services.RegisterSingle(new UIFactory(
                _services.Single<IGameStateMachine>(),
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IShopService>(),
                _services.Single<IScoreService>(),
                _services.Single<IDatabaseService>(),
                _services.Single<IUserSettingService>()));

        private void RegisterWindowsService() =>
            _services.RegisterSingle(new WindowsService(_services.Single<IUIFactory>()));

        private void RegisterHeroDeathService() =>
            _services.RegisterSingle(new HeroDeathService(_services.Single<IWindowsService>()));

        private void RegisterShopService()
        {
            var progressService = _services.Single<IPersistentProgressService>();
            _services.RegisterSingle(new ShopService(progressService.Player.Monies, progressService.Player.Inventory,
                _services.Single<IRandomService>(), _services.Single<IStaticDataService>().ForShop()));
        }

        private void RegisterBonusSpawner() =>
            _services.RegisterSingle(new BonusSpawner(_services.Single<IBonusFactory>(),
                _services.Single<IRandomService>()));

        private void RegisterBonusFactory() =>
            _services.RegisterSingle(new BonusFactory(_services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>()));

        private void RegisterScoreService()
        {
            _services.RegisterSingle(new ScoreService(_services.Single<IEnemySpawner>(),
                _services.Single<IStaticDataService>().ForScore(),
                _services.Single<IPersistentProgressService>().Player.Score,
                _services.Single<IDatabaseService>()));
        }

        private void RegisterDatabaseService() =>
            _services.RegisterSingle(new DatabaseService());
    }
}