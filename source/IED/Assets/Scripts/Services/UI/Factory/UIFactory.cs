using System;
using System.Linq;
using ConstantsValue;
using GameStates;
using Services.Assets;
using Services.Database;
using Services.PlayerData;
using Services.Progress;
using Services.Score;
using Services.Shop;
using Services.StaticData;
using Services.UserSetting;
using StaticData.UI;
using UI.Audio;
using UI.Base;
using UI.Windows;
using UI.Windows.Inventories;
using UI.Windows.Leaderboard;
using UI.Windows.Menus;
using UI.Windows.Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Services.UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IDatabaseService _databaseService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly IScoreService _scoreService;
        private readonly IShopService _shopService;
        private readonly IStaticDataService _staticData;
        private readonly IUserSettingService _userSettingService;
        
        private Camera _mainCamera;
        private Transform _uiRoot;
        private Button[] _buttons;
            
        public UIFactory(
            IGameStateMachine gameStateMachine,
            IAssetProvider assets,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IShopService shopService,
            IScoreService scoreService,
            IDatabaseService databaseService,
            IUserSettingService userSettingService)
        {
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
            _staticData = staticData ?? throw new ArgumentNullException(nameof(staticData));
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
            _shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
        }

        public event Action<WindowId, BaseWindow> Spawned;

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate<GameObject>(AssetsPath.UIRootPath).transform;
            _uiRoot.GetComponent<UIRoot>().SetCamera(GetCamera());
        }

        public void CreateWindow(WindowId id)
        {
            var config = LoadWindowInstantiateData(id);
            switch (id)
            {
                case WindowId.Inventory:
                    CreateInventoryWindow(config, id);
                    break;
                case WindowId.Shop:
                    CreateShopWindow(config, id, _progressService.Player.Monies);
                    break;
                case WindowId.PauseMenu:
                    CreatePauseMenuWindow(config, id);
                    break;
                case WindowId.MainMenu:
                    CreateMainMenuWindow(config, id);
                    break;
                case WindowId.DeathMenu:
                    CreateDeathMenuWindow(config, id);
                    break;
                case WindowId.Leaderboard:
                    CreateLeaderboardWindow(config, id);
                    break;
                case WindowId.Settings:
                    CreateSettingsWindow(config, id);
                    break;
                default:
                    CreateWindow(config, id);
                    break;
            }

            _buttons = _uiRoot.GetComponentsInChildren<Button>();
            var buttonSong = _uiRoot.GetComponent<AudioButton>();
            var audioSource = buttonSong.GetComponentInChildren<AudioSource>();
            audioSource.volume = _userSettingService.GetUserSettings().ActionsVolume;
            
            foreach (var button in _buttons)
            {
                var eventTriggerComponent = button.gameObject.AddComponent<EventTrigger>();
                var eventTrigger = new EventTrigger.Entry();
                eventTrigger.callback.AddListener((eventData) => {  buttonSong.OnClick(); });
                eventTriggerComponent.triggers.Add(eventTrigger);
            }
        }

        private void CreateSettingsWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            ((UserSettingsWindow) window).Construct(_userSettingService);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreateInventoryWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            ((InventoryWindow) window).Construct(_progressService.Player);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreateShopWindow(WindowInstantiateData config, WindowId id, PlayerMoney monies)
        {
            var window = InstantiateWindow(config);
            ((ShopWindow) window).Construct(_shopService, monies);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreatePauseMenuWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            ((PauseMenuWindow) window).Construct(_gameStateMachine);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreateMainMenuWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            ((MainMenuWindow) window).Construct(_gameStateMachine);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreateDeathMenuWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            ((DeathMenuWindow) window).Construct(_gameStateMachine, _scoreService);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreateLeaderboardWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            ((LeaderboardWindow) window).Construct(_databaseService);
            NotifyAboutCreateWindow(id, window);
        }

        private void CreateWindow(WindowInstantiateData config, WindowId id)
        {
            var window = InstantiateWindow(config);
            NotifyAboutCreateWindow(id, window);
        }

        private void NotifyAboutCreateWindow(WindowId id, BaseWindow window)
        {
            Spawned?.Invoke(id, window);
        }

        private BaseWindow InstantiateWindow(WindowInstantiateData config)
        {
            return _assets.Instantiate(config.Window, _uiRoot);
        }

        private WindowInstantiateData LoadWindowInstantiateData(WindowId id)
        {
            return _staticData.ForWindow(id);
        }

        private Camera GetCamera()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
}