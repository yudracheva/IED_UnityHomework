using System;
using Audio;
using ConstantsValue;
using GameStates.States.Interfaces;
using SceneLoading;
using Services.StaticData;
using Services.UI.Factory;
using Services.UI.Windows;
using Services.UserSetting;
using UI.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStates.States
{
    public class MainMenuState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly IWindowsService _windowsService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUserSettingService _userSettingService;
        private readonly IStaticDataService _staticDataService;
        
        private Button[] _buttons;
        
        public MainMenuState(
            IUIFactory uiFactory,
            IWindowsService windowsService,
            ISceneLoader sceneLoader,
            IUserSettingService userSettingService,
            IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));
            _windowsService = windowsService ?? throw new ArgumentNullException(nameof(windowsService));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
            _staticDataService = staticDataService ?? throw new ArgumentNullException(nameof(staticDataService));
        }

        public void Enter()
        {
            Time.timeScale = 1;
            _sceneLoader.Load(Constants.MainMenuScene, OnLoaded);
        }

        private void OnLoaded()
        {
            var uiRoot = _uiFactory.CreateUIRoot();
            InitAudio(uiRoot);
            _windowsService.Open(WindowId.MainMenu);
        }

        public void Exit()
        {
        }
        
        private void InitAudio(GameObject uiRoot)
        {
            _userSettingService.UpdateGameSettings();
            
            var buttonSong = uiRoot.GetComponentInChildren<AudioButton>();
            buttonSong.Construct(_userSettingService);

            var audio = _staticDataService.AudioForLevel(SceneManager.GetActiveScene().name);
            
            var audioBackground = uiRoot.GetComponentInChildren<AudioBackground>();
            audioBackground.Construct(_userSettingService, audio.AudioClip);
        }
    }
}