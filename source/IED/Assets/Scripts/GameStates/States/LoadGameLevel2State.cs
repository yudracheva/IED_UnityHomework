using System;
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

namespace GameStates.States
{
    public class LoadGameLevel2State : BaseLoadGameLevelState
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        public LoadGameLevel2State(
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
            IAssetProvider assetProvider) 
            : base(
                persistentProgressService, 
                sceneLoader,
                gameStateMachine,
                gameFactory,
                uiFactory,
                staticData,
                waveServices,
                lootService,
                lootSpawner,
                shopService,
                userSettingService,
                assetProvider)
        {
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
        }

        private protected override void OnLoaded()
        {
            InitGameWorld(false);
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}